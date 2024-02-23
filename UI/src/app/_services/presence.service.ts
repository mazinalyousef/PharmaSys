import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { AuthenticatedResponse } from '../_models/AuthenticatedResponse';
import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';
import { notification } from '../_models/notification';
import { NotificationService } from './notification.service';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {



  hubUrl = environment.hubUrl;
  private hubConnection : HubConnection;
  private NewnotificationsCount =new Subject<number>();
  NewnotificationsCount$ = this.NewnotificationsCount.asObservable();

  public notificationsGlobalCounter :number;

  public tasktimerData :number; // testing


  private timerTick =new Subject<number>();
  timerTick$ = this.timerTick.asObservable();
  public timerTickGlobalCounter :number;

  private timerTickDisplay =new Subject<string>();
  timerTickDisplay$ = this.timerTickDisplay.asObservable();
  public timerTickDisplayGlobalCounter :string;



 // testing
  private  userNotifications = new ReplaySubject<notification[]>(1);
  userNotifications$=this.userNotifications.asObservable();


  constructor(private notificationService : NotificationService)
   {

    this.userNotifications.next([]); // may commit later

    this.notificationsGlobalCounter=0; // must have initial value .... read from database ...

    this.timerTickGlobalCounter=-1; // testing
     
   }
  setnotificationsGlobalCounter(countValue:number){
    this.notificationsGlobalCounter+= countValue; 
  }

  resetnotificationsGlobalCounter(countValue:number){
    this.notificationsGlobalCounter= countValue; 
  }
  getnotificationsGlobalCounter():number{
    return this.notificationsGlobalCounter;
  }

  settimerTickGlobalCounter(countValue:number){
    this.timerTickGlobalCounter= countValue; 
  }
  gettimerTickGlobalCounter():number{
    return this.timerTickGlobalCounter;
  }

  settimerTickDisplayGlobalCounter(countValue:string){
    this.timerTickDisplayGlobalCounter= countValue; 
  }
  gettimerTickDisplayGlobalCounter():string{
    return this.timerTickDisplayGlobalCounter;
  }

  createHubConnection(user:AuthenticatedResponse)
  {
    this.hubConnection =new HubConnectionBuilder()
    .withUrl(this.hubUrl+'notification',
    {
      accessTokenFactory:()=>user.token
    })
    .withAutomaticReconnect()
    .build()

    this.hubConnection.start()
    .catch(error=>console.log(error));

    /*
    this.hubConnection.on('UserIsOnline',connid=>
    {
      alert(connid + ' has connected');
    })
    */

    /*
    this.hubConnection.on('UserIsOffline',connid=>
    {
      alert(connid + ' has disconnected');
    })
    */

    this.hubConnection.on('ReceiveMessage',msg=>
    {
     // alert(msg);
    this.setnotificationsGlobalCounter(1);
    this.NewnotificationsCount.next(this.getnotificationsGlobalCounter());
   //  this.NewnotificationsCount.next(1);
      
    })
  }
  // test 

  createHubConnectiontesttimer(user:AuthenticatedResponse)
  {
    this.hubConnection =new HubConnectionBuilder()
    .withUrl(this.hubUrl+'taskTimer',
    {
      accessTokenFactory:()=>user.token
    })
    .withAutomaticReconnect()
    .build()

    this.hubConnection.start()
    .catch(error=>console.log(error));

    

    this.hubConnection.on('TransferTimerData',(data)=>
    {
     // alert(msg);
    this.tasktimerData=data;
    console.log(this.tasktimerData);

    this.settimerTickGlobalCounter(data);
    this.timerTick.next(this.gettimerTickGlobalCounter());
    
      
    })
  }

  joinGroups(user:AuthenticatedResponse)
  {
    this.hubConnection =new HubConnectionBuilder()
    .withUrl(this.hubUrl+'notification',
    {
      accessTokenFactory:()=>user.token
    })
    .withAutomaticReconnect()
    .build()

    this.hubConnection.start().then(
      ()=>{
        this.hubConnection.invoke("AddUserToGroup",user.username).catch(err=>{console.log(err)});
      }
    )
    .catch(error=>console.log(error));
     
    this.hubConnection.on('ReceiveMessage',msg=>
    {
      this.setnotificationsGlobalCounter(1);
      this.NewnotificationsCount.next(this.getnotificationsGlobalCounter());
     // this.notificationsGlobalCounter++;
     // alert(msg);
     // this.NewnotificationsCount.next(1);

    });

    // will also listen to update notifications
    this.hubConnection.on('UpdateNotifications',msg=>
    {
      // update the notifications ....
      this.getUsernotifications(user.id);
    })


  }

  joinTaskGroups(user:AuthenticatedResponse,taskid:number)
  {
    this.hubConnection =new HubConnectionBuilder()
    .withUrl(this.hubUrl+'taskTimer',
    {
      accessTokenFactory:()=>user.token
    })
    .withAutomaticReconnect()
    .build()
    
    this.hubConnection.start().then(
      ()=>{
        this.hubConnection.invoke("AddUserToGroup",taskid).catch(err=>{console.log(err)});
      }
    )
    .catch(error=>console.log(error));
     
   

    this.hubConnection.on('TransferTimerData',(data)=>
    {
     // alert(msg);
    this.tasktimerData=data;
    console.log(this.tasktimerData);

    this.settimerTickGlobalCounter(data);
    this.timerTick.next(this.gettimerTickGlobalCounter());


    // convert to hh mm ss
   ;
    this.settimerTickDisplayGlobalCounter(  this.secondsToHms(this.tasktimerData) );
      this.timerTickDisplay.next(this.gettimerTickDisplayGlobalCounter());
    })
  }

  stopHubConnection()
  {
    this.hubConnection.stop().catch(error=>{console.log(error);})
  }

  getUsernotifications( userId:string)
  {
      this.notificationService.getallForUser(userId).subscribe(
        res=>
        {
          this.userNotifications.next(res);
          console.log(res);
        }
        ,error=>
        {
          console.log(error);
        }
      )
  }


  setInitialNotificationsCount(userId:string,resetmode:boolean)
  {

    this.notificationService.getallunreadForUser(userId).subscribe(
      res=>
      {
        if (resetmode)
        {
          console.log(res+" from getallunreadForUser "+userId)
          this.resetnotificationsGlobalCounter(res.length);
        }
        else
        {
          this.setnotificationsGlobalCounter( res.length);
        }
      
        this.NewnotificationsCount.next(this.getnotificationsGlobalCounter());
       
        console.log(res);
      }
      ,error=>
      {
        console.log(error);
      }
    )
   
  }
    // move to another layer...later...
   secondsToHms(d) :string
    {
    d = Number(d);
    var h = Math.floor(d / 3600);
    var m = Math.floor(d % 3600 / 60);
    var s = Math.floor(d % 3600 % 60);

    var hDisplay = h > 0 ? h + (h == 1 ? " hour, " : " hours, ") : "";
    var mDisplay = m > 0 ? m + (m == 1 ? " minute, " : " minutes, ") : "";
    var sDisplay = s > 0 ? s + (s == 1 ? " second" : " seconds") : "";
    return hDisplay + mDisplay + sDisplay; 
   }
}
