import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { AuthenticatedResponse } from '../_models/AuthenticatedResponse';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {



  hubUrl = environment.hubUrl;
  private hubConnection : HubConnection;
  private NewnotificationsCount =new Subject<number>();
  NewnotificationsCount$ = this.NewnotificationsCount.asObservable();

  public notificationsGlobalCounter :number;
  

  constructor()
   {

    
    this.notificationsGlobalCounter=0; // must have initial value .... read from database ...

   }



  setnotificationsGlobalCounter(countValue:number){
    this.notificationsGlobalCounter+= countValue; 
  }
  getnotificationsGlobalCounter():number{
    return this.notificationsGlobalCounter;
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
    })
  }

  stopHubConnection()
  {
    this.hubConnection.stop().catch(error=>{console.log(error);})
  }
}
