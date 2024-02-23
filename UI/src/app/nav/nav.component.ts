import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UsersService } from '../_services/users.service';
import { Observable, take, takeLast } from 'rxjs';
import { AuthenticatedResponse } from '../_models/AuthenticatedResponse';
import { PresenceService } from '../_services/presence.service';
import { NotificationService } from '../_services/notification.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})


export class NavComponent implements OnInit {


  notificationsCount$:Observable<number>;
 // notificationsIsHidden:boolean;
  notificationCounter :number;
  //loggedin : boolean;
  currentUser$:Observable<AuthenticatedResponse>;

  loggedUserId : string;
  loggedUserName : string;

  constructor(public userservice:UsersService,
    public presenceservice:PresenceService,
    private notificationservice : NotificationService,
     private router: Router)
  {
    //this.notificationsIsHidden=false;
    this.notificationCounter=0;
  }


    ngOnInit(): void 
    
    {


      this.userservice.loggedUser$.pipe(
        take(1) 
       ).subscribe(
         res=> {
           console.log(res.username+" from nav ...");
         this.loggedUserId=res.id;
         this.loggedUserName=res.username;
         }
       )

     //this.router.navigate(['home'])
   
   //  this.getCurrentUser();

     this.currentUser$ = this.userservice.loggedUser$;
     if (this.loggedUserId)
     {
      
     // this.router.navigate(['home']);
   
     }
     else
     {
       this.router.navigate(['login']);
     }


     this.notificationsCount$ = this.presenceservice.NewnotificationsCount$.pipe(takeLast(1));
     

    }

    getCurrentUser()
    {
      this.userservice.loggedUser$.subscribe(
        res=>
        {
         // this.loggedin = !!res;
         console.log(res);
        }
        ,err=>
        {
         console.log(err);
        }
      )
    }

    onlogout()
    {
      this.userservice.performlogout();
      this.router.navigate(['login']);
    }

    showNotifications()
    {


      // testing... re-update the logged user id 
       this.userservice.loggedUser$.pipe(
        take(1) 
       ).subscribe(
         res=> 
         {
           console.log(res.username+" from nav ...");
         this.loggedUserId=res.id;
         
         }
       )
       
         this.notificationservice.setAllasRead(this.loggedUserId).subscribe(res=>
        {
            if (res)
            {
              console.log(res+" from nav click notifications")
              this.presenceservice.setInitialNotificationsCount(this.loggedUserId,true);
            }
        },error=>{console.log(error);}
        )
    
      this.router.navigate(['notifications']);
    }

}
