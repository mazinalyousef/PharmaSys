import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, Subject, map, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { observableToBeFn } from 'rxjs/internal/testing/TestScheduler';
import { LoginModel } from '../_models/login';
import { AuthenticatedResponse } from '../_models/AuthenticatedResponse';
import { JwtHelperService } from '@auth0/angular-jwt';
import { PresenceService } from './presence.service';
import { changepassword } from '../_models/changepassword';
import { BatchtaskService } from './batchtask.service';
import { TaskTypes } from '../_enums/TaskTypes';



 
@Injectable({
  providedIn: 'root'
})


export class UsersService 

{

   
   CurrentloggedUser : AuthenticatedResponse; 
   jwtHelper = new JwtHelperService();
   decodedToken: any;

   
   private loggedUser = new ReplaySubject<AuthenticatedResponse>(1) ;

   loggedUser$ = this.loggedUser.asObservable();

 //  private LoggedUserName =new Subject<string>();
 //  LoggedUserName$ = this.LoggedUserName.asObservable();

   private LoggedUserName =new ReplaySubject<string>(1);
   LoggedUserName$ = this.LoggedUserName.asObservable();

   baseUrl = environment.apiUrl;


   // move this logic to interceptor....
  constructor(private http:HttpClient, private presenseService:PresenceService,
    private batchTaskservice:BatchtaskService
    ) 
  {
     
  }

  getUsers():Observable<User[]>
  {
     return this.http.get<User[]>(this.baseUrl+'Users');
  }

  getUser(username:string)
  {
    return this.http.get<User>(this.baseUrl+'Users/'+username);
  }

  adduser(user:User) : Observable<User>
  {
    return  this.http.post<User>(this.baseUrl+'Users/register',user);
  }

  updateuser(username:string,user:User) :Observable<User>
  {
    
    return  this.http.put<User>(this.baseUrl+'Users/'+username,user);
  }

  /*
  performlogin(loginmodel:LoginModel) : Observable<AuthenticatedResponse>
  {
    return  this.http.post<AuthenticatedResponse>(this.baseUrl+'Users/login',loginmodel);
  }
  */

 
  performlogin(loginmodel:LoginModel)    
  {
    return  this.http.post
    (this.baseUrl+'Users/login',loginmodel).pipe
    (
      map
      (
        (res :AuthenticatedResponse)=>
        {
          const user = res;
          console.log(user.username+"  from user service");
          if (user)
          {
            sessionStorage.setItem('user',JSON.stringify(user));
            this.loggedUser.next(user);
            this.LoggedUserName.next(user.username);
            this.decodedToken = this.jwtHelper.decodeToken(user.token);
            this.CurrentloggedUser = user;
             
             // notifications
             this.presenseService.createHubConnection(this.CurrentloggedUser);
              this.presenseService.setInitialNotificationsCount(user.id,true);
             this.presenseService.getUsernotifications(user.id);
            this.presenseService.joinGroups( this.CurrentloggedUser);

            // messages...
              this.presenseService.createMessageHubConnection(this.CurrentloggedUser);
              this.presenseService.setInitialMessagesCount(user.id,true);
              this.presenseService.getUsermessages(user.id);
              this.presenseService.joinMessageGroups(this.CurrentloggedUser);

              this.batchTaskservice.getallRunningForUser(user.id).subscribe(
                res=>
                {
                  if (res)
                  { 
                     const userTasks = res;
                     userTasks.forEach(element => {
                       
                        if (element.taskTypeId===TaskTypes.Cartooning||element.taskTypeId===TaskTypes.FillingTubes)
                        {
                           // join the reminders ...
                           // the department is not  necessary ...
                           this.presenseService.joinReminderGroups(user,element.id,0,element.taskTypeId);
                        }
                       
                     });
                  }
                },
                error=>
                {
                  console.log(error);
                }
              )

          }
        }
      )
    );
  }
 

    /*
  performlogin(loginmodel:LoginModel) :Observable<AuthenticatedResponse>   
  {
    return  this.http.post<AuthenticatedResponse>
    (this.baseUrl+'Users/login',loginmodel);
  }
  */

  setCurrentUser(user:AuthenticatedResponse)
  {
    console.log("inside setCurrentUser for user "+user.username);
    this.loggedUser.next(user);
    this.LoggedUserName.next(user.username);
     

    // here we nedd to maintain (restore) the hub connection
    this.presenseService.createHubConnection(user); //added
    this.presenseService.setInitialNotificationsCount(user.id,true);
    this.presenseService.getUsernotifications(user.id);
    this.presenseService.joinGroups( user);//added
   
  
    this.presenseService.createMessageHubConnection(user); //added
     this.presenseService.setInitialMessagesCount(user.id,true); 
    this.presenseService.getUsermessages(user.id);
    this.presenseService.joinMessageGroups(user); //added

    // timer hub not habdeled here ...
    // consider it 
    // may need to handle as well
    

    // we need to check the user current runnig tasks... so the user will join reminders ...
    this.batchTaskservice.getallRunningForUser(user.id).subscribe(
      res=>
      {
        if (res)
        { 
           const userTasks = res;
           userTasks.forEach(element => {
             
              if (element.taskTypeId===TaskTypes.Cartooning||element.taskTypeId===TaskTypes.FillingTubes)
              {
                 // join the reminders ...
                 // the department is not  necessary ...
                 this.presenseService.joinReminderGroups(user,element.id,0,element.taskTypeId);
              }
             
           });
        }
      },
      error=>
      {
        console.log(error);
      }
    )

    
  }

   

  performlogout()
  {
    sessionStorage.removeItem('user');
    this.loggedUser.next(null);
    this.LoggedUserName.next("");
    this.CurrentloggedUser = null;
    this.presenseService.stopHubConnection();
  }

  changePassword(changepasswordModel : changepassword)
  {
      return this.http.post(this.baseUrl+'Users/ChangePassword',changepasswordModel);
  }


  getUserRoles(userName :string) : Observable<string[]>
  {
    return this.http.get<string[]>(this.baseUrl+'Users/Roles/'+userName);
  }

  updateUserRoles(userName :string,roleNames : string[])
  {
    return this.http.post(this.baseUrl+'Users/editRoles/'+userName,roleNames);
  }


  

}
