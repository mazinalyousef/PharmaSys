import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { AuthenticatedResponse } from '../_models/AuthenticatedResponse';
import { BehaviorSubject, Observable, ReplaySubject, Subject } from 'rxjs';
import { notification } from '../_models/notification';
import { NotificationService } from './notification.service';
import { TaskTypes } from '../_enums/TaskTypes';
import { DepartmentsEnum } from '../_enums/DepartmentsEnum';
import { message } from '../_models/message';
import { MessageService } from './message.service';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {



  hubUrl = environment.hubUrl;
  private hubConnection : HubConnection; // notifications
  private TimerHubConnection : HubConnection;
  private messageHubConnection : HubConnection;



     


    // notifications...
    private NewnotificationsCount =new Subject<number>();
    NewnotificationsCount$ = this.NewnotificationsCount.asObservable();
    public notificationsGlobalCounter :number;

     
    private  userNotifications = new ReplaySubject<notification[]>(1);
    userNotifications$=this.userNotifications.asObservable();



    //  messages...
    private NewmessagesCount =new Subject<number>();
    NewmessagesCount$ = this.NewmessagesCount.asObservable();
    public messagesGlobalCounter :number;

     
    private  userMessages = new ReplaySubject<message[]>(1);
    userMessages$=this.userMessages.asObservable();

     


  private tasktimerData :number; // testing
  private timerTick =new Subject<number>();
  timerTick$ = this.timerTick.asObservable();
  private timerTickGlobalCounter :number;

  private timerTickDisplay =new Subject<string>();
  timerTickDisplay$ = this.timerTickDisplay.asObservable();
  private timerTickDisplayGlobalCounter :string;



   //#region  timers
  
  // raw material  warehouse
  private RawMaterialsWeighting_WareHouse_tasktimerData :number; // testing
  private RawMaterialsWeighting_WareHouse_timerTick =new Subject<number>();
  RawMaterialsWeighting_WareHouse_timerTick$ = this.RawMaterialsWeighting_WareHouse_timerTick.asObservable();
  private RawMaterialsWeighting_WareHouse_timerTickGlobalCounter :number;

  private RawMaterialsWeighting_WareHouse_timerTickDisplay =new Subject<string>();
  RawMaterialsWeighting_WareHouse_timerTickDisplay$ = this.RawMaterialsWeighting_WareHouse_timerTickDisplay.asObservable();
  private RawMaterialsWeighting_WareHouse_timerTickDisplayGlobalCounter :string;

  // raw material QA

  private RawMaterialsWeighting_QA_tasktimerData :number; // testing
  private RawMaterialsWeighting_QA_timerTick =new Subject<number>();
  RawMaterialsWeighting_QA_timerTick$ = this.RawMaterialsWeighting_QA_timerTick.asObservable();
  private RawMaterialsWeighting_QA_timerTickGlobalCounter :number;

  private RawMaterialsWeighting_QA_timerTickDisplay =new Subject<string>();
  RawMaterialsWeighting_QA_timerTickDisplay$ = this.RawMaterialsWeighting_QA_timerTickDisplay.asObservable();
  private RawMaterialsWeighting_QA_timerTickDisplayGlobalCounter :string;


  // raw material Accountant

  private RawMaterialsWeighting_Accountant_tasktimerData :number; // testing
  private RawMaterialsWeighting_Accountant_timerTick =new Subject<number>();
  RawMaterialsWeighting_Accountant_timerTick$ = this.RawMaterialsWeighting_Accountant_timerTick.asObservable();
  private RawMaterialsWeighting_Accountant_timerTickGlobalCounter :number;

  private RawMaterialsWeighting_Accountant_timerTickDisplay =new Subject<string>();
  RawMaterialsWeighting_Accountant_timerTickDisplay$ = this.RawMaterialsWeighting_Accountant_timerTickDisplay.asObservable();
  private RawMaterialsWeighting_Accountant_timerTickDisplayGlobalCounter :string;


    // raw checkRoome warehouse

    private CheckRoome_Warehouse_tasktimerData :number; // testing
    private CheckRoome_Warehouse_timerTick =new Subject<number>();
    CheckRoome_Warehouse_timerTick$ = this.CheckRoome_Warehouse_timerTick.asObservable();
    private CheckRoome_Warehouse_timerTick_timerTickGlobalCounter :number;
  
    private CheckRoome_Warehouse_timerTick_timerTickDisplay =new Subject<string>();
    CheckRoome_Warehouse_timerTick_timerTickDisplay$ = this.CheckRoome_Warehouse_timerTick_timerTickDisplay.asObservable();
    private CheckRoome_Warehouse_timerTickDisplayGlobalCounter :string;
    

    //#endregion


   constructor(private notificationService : NotificationService,private messageService:MessageService)
   {

    this.userNotifications.next([]); // may commit later

    this.notificationsGlobalCounter=0; // must have initial value .... read from database ...

    this.userMessages.next([]);

    this.messagesGlobalCounter=0;

    this.timerTickGlobalCounter=-1; // testing


   }


   // notifications...
  setnotificationsGlobalCounter(countValue:number){
    this.notificationsGlobalCounter+= countValue; 
  }

  resetnotificationsGlobalCounter(countValue:number){
    this.notificationsGlobalCounter= countValue; 
  }
  getnotificationsGlobalCounter():number{
    return this.notificationsGlobalCounter;
  }

  

  /// messages
  setmessagesGlobalCounter(countValue:number){
    this.messagesGlobalCounter+= countValue; 
  }

  resetmessagesGlobalCounter(countValue:number){
    this.messagesGlobalCounter= countValue; 
  }
  getmessagesGlobalCounter():number{
    return this.messagesGlobalCounter;
  }




  //#region Timers methods

   // manufacturing Timer 
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

  // raw material  warehouse timer 
  setRawMaterialsWeighting_WareHousetimerTickGlobalCounter(countValue:number){
    this.RawMaterialsWeighting_WareHouse_timerTickGlobalCounter= countValue; 
  }
  getRawMaterialsWeighting_WareHousetimerTickGlobalCounter():number{
    return this.RawMaterialsWeighting_WareHouse_timerTickGlobalCounter;
  }

  setRawMaterialsWeighting_WareHousetimerTickDisplayGlobalCounter(countValue:string){
    this.RawMaterialsWeighting_WareHouse_timerTickDisplayGlobalCounter= countValue; 
  }
  getRawMaterialsWeighting_WareHousetimerTickDisplayGlobalCounter():string{
    return this.RawMaterialsWeighting_WareHouse_timerTickDisplayGlobalCounter;
  }

  //  // raw material QA
  setRawMaterialsWeighting_QAtimerTickGlobalCounter(countValue:number){
    this.RawMaterialsWeighting_QA_timerTickGlobalCounter= countValue; 
  }
  getRawMaterialsWeighting_QAtimerTickGlobalCounter():number{
    return this.RawMaterialsWeighting_QA_timerTickGlobalCounter;
  }

  setRawMaterialsWeighting_QAtimerTickDisplayGlobalCounter(countValue:string){
    this.RawMaterialsWeighting_QA_timerTickDisplayGlobalCounter= countValue; 
  }
  getRawMaterialsWeighting_QAtimerTickDisplayGlobalCounter():string{
    return this.RawMaterialsWeighting_QA_timerTickDisplayGlobalCounter;
  }

  // raw material accountant
  setRawMaterialsWeighting_AccountanttimerTickGlobalCounter(countValue:number){
    this.RawMaterialsWeighting_Accountant_timerTickGlobalCounter= countValue; 
  }
  getRawMaterialsWeighting_AccountanttimerTickGlobalCounter():number{
    return this.RawMaterialsWeighting_Accountant_timerTickGlobalCounter;
  }

  setRawMaterialsWeighting_AccountanttimerTickDisplayGlobalCounter(countValue:string){
    this.RawMaterialsWeighting_Accountant_timerTickDisplayGlobalCounter= countValue; 
  }
  getRawMaterialsWeighting_AccountanttimerTickDisplayGlobalCounter():string{
    return this.RawMaterialsWeighting_Accountant_timerTickDisplayGlobalCounter;
  }

  // check rooom warehouse
  setCheckRoome_WarehousetimerTickGlobalCounter(countValue:number){
    this.CheckRoome_Warehouse_timerTick_timerTickGlobalCounter= countValue; 
  }
  getCheckRoome_WarehousetimerTickGlobalCounter():number{
    return this.CheckRoome_Warehouse_timerTick_timerTickGlobalCounter;
  }

  setCheckRoome_WarehousetimerTickDisplayGlobalCounter(countValue:string){
    this.CheckRoome_Warehouse_timerTickDisplayGlobalCounter= countValue; 
  }
  getCheckRoome_WarehousetimerTickDisplayGlobalCounter():string{
    return this.CheckRoome_Warehouse_timerTickDisplayGlobalCounter;
  }


    //#endregion


  // notification hub
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

  
    this.hubConnection.on('ReceiveMessage',msg=>
    {
     // alert(msg);
    this.setnotificationsGlobalCounter(1);
    this.NewnotificationsCount.next(this.getnotificationsGlobalCounter());
   //  this.NewnotificationsCount.next(1);
      
    })
  }

  // messages Hub
  createMessageHubConnection(user:AuthenticatedResponse)
  {
    this.messageHubConnection =new HubConnectionBuilder()
    .withUrl(this.hubUrl+'message',
    {
      accessTokenFactory:()=>user.token
    })
    .withAutomaticReconnect()
    .build()

    this.messageHubConnection.start()
    .catch(error=>console.log(error));

  
    this.hubConnection.on('ReceiveNote',msg=>
    {
     
    this.setmessagesGlobalCounter(1);
    this.NewmessagesCount.next(this.getmessagesGlobalCounter());
    
      
    })
  }

  // timer hub... 

  createHubConnectiontesttimer(user:AuthenticatedResponse)
  {
    this.TimerHubConnection =new HubConnectionBuilder()
    .withUrl(this.hubUrl+'taskTimer',
    {
      accessTokenFactory:()=>user.token
    })
    .withAutomaticReconnect()
    .build()

    this.TimerHubConnection.start()
    .catch(error=>console.log(error));

    

    this.TimerHubConnection.on('TransferTimerData',(data)=>
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

  // mesage groups

  joinMessageGroups(user:AuthenticatedResponse)
  {
    this.messageHubConnection =new HubConnectionBuilder()
    .withUrl(this.hubUrl+'message',
    {
      accessTokenFactory:()=>user.token
    })
    .withAutomaticReconnect()
    .build()

     this.messageHubConnection.start().then(
      ()=>{
        this.messageHubConnection.invoke("AddUserToGroup",user.username).catch(err=>{console.log(err)});
      }
    )
    .catch(error=>console.log(error));

    
     
    this.messageHubConnection.on('ReceiveNote',msg=>
    {
      this.setmessagesGlobalCounter(1);
      this.NewmessagesCount.next(this.getmessagesGlobalCounter());
    
    });

    // will also listen to update notifications
    this.messageHubConnection.on('UpdateNotes',msg=>
    {
      // update the notifications ....
      this.getUsermessages(user.id);
    })
  }



  joinTaskGroups(user:AuthenticatedResponse,taskid:number,departmentId:number,tasktypeId:number)
  {
    this.TimerHubConnection =new HubConnectionBuilder()
    .withUrl(this.hubUrl+'taskTimer',
    {
      accessTokenFactory:()=>user.token
    })
    .withAutomaticReconnect()
    .build()
    
    this.TimerHubConnection.start().then(
      ()=>{
        this.TimerHubConnection.invoke("AddUserToGroup",taskid).catch(err=>{console.log(err)});
      }
    )
    .catch(error=>console.log(error));
     
   
   // let TransferTimerDataSignalRMethodName:string=taskid.toString()+"_"+"TransferTimerData";
    this.TimerHubConnection.on("TransferTimerData",(data)=>
    {
      

      // determine which timer to set....
      // alert(msg);
        this.tasktimerData=data;
        //console.log(this.tasktimerData);

        this.settimerTickGlobalCounter(data);
        this.timerTick.next(this.gettimerTickGlobalCounter());

        // convert to hh mm ss
        this.settimerTickDisplayGlobalCounter(  this.secondsToHms(this.tasktimerData) );
        this.timerTickDisplay.next(this.gettimerTickDisplayGlobalCounter());

        /*
        if (tasktypeId==TaskTypes.Manufacturing)
        {
        // alert(msg);
        this.tasktimerData=data;
        //console.log(this.tasktimerData);

         this.settimerTickGlobalCounter(data);
        this.timerTick.next(this.gettimerTickGlobalCounter());

         // convert to hh mm ss
         this.settimerTickDisplayGlobalCounter(  this.secondsToHms(this.tasktimerData) );
           this.timerTickDisplay.next(this.gettimerTickDisplayGlobalCounter());
       }
       else  if (tasktypeId==TaskTypes.RawMaterialsWeighting)
       {
        if (departmentId==DepartmentsEnum.Warehouse)
        {
          // alert(msg);
        this.RawMaterialsWeighting_WareHouse_tasktimerData=data;
        //console.log(this.tasktimerData);

         this.setRawMaterialsWeighting_WareHousetimerTickGlobalCounter(data);
        this.RawMaterialsWeighting_WareHouse_timerTick.next(this.getRawMaterialsWeighting_WareHousetimerTickGlobalCounter());

         // convert to hh mm ss
         this.setRawMaterialsWeighting_WareHousetimerTickDisplayGlobalCounter(  this.secondsToHms(this.RawMaterialsWeighting_WareHouse_tasktimerData) );
           this.RawMaterialsWeighting_WareHouse_timerTickDisplay.next(this.getRawMaterialsWeighting_WareHousetimerTickDisplayGlobalCounter());
        }
        else if (departmentId==DepartmentsEnum.QA)
        {
          this.RawMaterialsWeighting_QA_tasktimerData=data;
          //console.log(this.tasktimerData);
  
           this.setRawMaterialsWeighting_QAtimerTickGlobalCounter(data);
          this.RawMaterialsWeighting_QA_timerTick.next(this.getRawMaterialsWeighting_QAtimerTickGlobalCounter());
  
           // convert to hh mm ss
           this.setRawMaterialsWeighting_QAtimerTickDisplayGlobalCounter(  this.secondsToHms(this.RawMaterialsWeighting_QA_tasktimerData) );
             this.RawMaterialsWeighting_QA_timerTickDisplay.next(this.getRawMaterialsWeighting_QAtimerTickDisplayGlobalCounter());
        }
        else if (departmentId==DepartmentsEnum.Accounting)
        {
          this.RawMaterialsWeighting_Accountant_tasktimerData=data;
          //console.log(this.tasktimerData);
  
           this.setRawMaterialsWeighting_AccountanttimerTickGlobalCounter(data);
          this.RawMaterialsWeighting_Accountant_timerTick.next(this.getRawMaterialsWeighting_AccountanttimerTickGlobalCounter());
  
           // convert to hh mm ss
           this.setRawMaterialsWeighting_AccountanttimerTickDisplayGlobalCounter(  this.secondsToHms(this.RawMaterialsWeighting_Accountant_tasktimerData) );
             this.RawMaterialsWeighting_Accountant_timerTickDisplay.next(this.getRawMaterialsWeighting_AccountanttimerTickDisplayGlobalCounter());
        }
       }
       else if (tasktypeId==TaskTypes.RoomCleaning)
       {
        this.CheckRoome_Warehouse_tasktimerData=data;
        //console.log(this.tasktimerData);

         this.setCheckRoome_WarehousetimerTickGlobalCounter(data);
        this.CheckRoome_Warehouse_timerTick.next(this.getCheckRoome_WarehousetimerTickGlobalCounter());

         // convert to hh mm ss
         this.setCheckRoome_WarehousetimerTickDisplayGlobalCounter(  this.secondsToHms(this.CheckRoome_Warehouse_tasktimerData) );
           this.CheckRoome_Warehouse_timerTick_timerTickDisplay.next(this.getCheckRoome_WarehousetimerTickDisplayGlobalCounter());
       }
       */
    
        console.log(data);

    }
    
    )
  }


  RemoveTaskGroups(user:AuthenticatedResponse,taskid:number)
  {
    this.TimerHubConnection =new HubConnectionBuilder()
    .withUrl(this.hubUrl+'taskTimer',
    {
      accessTokenFactory:()=>user.token
    })
    .withAutomaticReconnect()
    .build()
    
    this.TimerHubConnection.start().then(
      ()=>
      {
        this.TimerHubConnection.invoke("RemoveUserToGroup",taskid)
        .catch(err=>{console.log(err)});
      }
    )
    .catch(error=>console.log(error));


    // added.....testing....
    // clear the timer data.... no need 
    /*
    this.tasktimerData=0;
    this.settimerTickGlobalCounter(0);
    this.timerTick.next(this.gettimerTickGlobalCounter());
    // convert to hh mm ss
    this.settimerTickDisplayGlobalCounter(  this.secondsToHms(this.tasktimerData) );
    this.timerTickDisplay.next(this.gettimerTickDisplayGlobalCounter());
    */

    
  }



  stopHubConnection()
  {
    if (this.hubConnection)
    { 
      this.hubConnection.stop().catch(error=>{console.log(error);})
    }
    if (this.messageHubConnection)
    { 
      this.messageHubConnection.stop().catch(error=>{console.log(error);})
    }

   if (this.TimerHubConnection)
   { 
    this.TimerHubConnection.stop().catch(error=>{console.log(error);})
  }
   
  }

  stopTimerHubConnection()
  {
  if (this.TimerHubConnection)
   { 
    this.TimerHubConnection.stop().catch(error=>{console.log(error);})
  }
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

  getUsermessages( userId:string)
  {
      this.messageService.getallForUser(userId).subscribe(
        res=>
        {
          this.userMessages.next(res);
          console.log(res);
        }
        ,error=>
        {
          console.log(error);
        }
      )
  }

  setInitialMessagesCount(userId:string,resetmode:boolean)
  {

    this.messageService.getallunreadForUser(userId).subscribe(
      res=>
      {
         console.log( "response from getallunreadForUser :"+res);
        if (resetmode)
        {
          
          this.resetmessagesGlobalCounter(res);
        }
        else
        {
          this.setmessagesGlobalCounter( res);
        }
      
        this.NewmessagesCount.next(this.getmessagesGlobalCounter());
       
        console.log(res);
      }
      ,error=>
      {
        console.log("error from getallunreadForUser:"+error);
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
