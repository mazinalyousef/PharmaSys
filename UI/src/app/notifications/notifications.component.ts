import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../_services/notification.service';
import { UsersService } from '../_services/users.service';
import { take } from 'rxjs';
import { notification } from '../_models/notification';
import { MappingHelperService } from '../_services/mapping-helper.service';
import { TaskTypes } from '../_enums/TaskTypes';
import { BaseTaskTypes } from '../_enums/BaseTaskTypes';
import { BatchtaskService } from '../_services/batchtask.service';
import { Router } from '@angular/router';
import { taskAssign } from '../_models/taskAssign';
import { PresenceService } from '../_services/presence.service';
import { AuthenticatedResponse } from '../_models/AuthenticatedResponse';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit
{
  usernotifications : notification[];
  displayedColumns = ['notificationMessage','takenDisplayTitle', 'dateSent','actions']; 
  loggedUserId : string;
  baseTaskType : BaseTaskTypes;
  TaskSeconds:number;
  taskDepartmentId:number;
  taskTypeId:number;
  taskassign : taskAssign={taskId:0,userId:'',seconds:0,departmentId:0,taskTypeId:0};
  assignSuccess:boolean;
  loggedUser:AuthenticatedResponse;



  constructor(private notificationService : NotificationService,
    private userservice:UsersService,
    private mappinghelper : MappingHelperService,
    private batchtaskservice:BatchtaskService,
    private router:Router,
    public presenseService : PresenceService,
 
    )
  {

  }
  ngOnInit(): void
  {
    
    this.userservice.loggedUser$.pipe(
    take(1) 
     ).subscribe(
       res=> {
         console.log(res.username);
       this.loggedUserId=res.id;
       this.loggedUser = res;
       console.log( this.loggedUserId+" from on init notifications")
       }
     )

     // note : committed to test the observable 
     // this.loadNotifications();  
     
     // must reload the user (logged user) notifications....
  }

  loadNotifications()
  {
    console.log( this.loggedUserId+" from on   notifications....loadNotifications function");
    this.notificationService.getallForUser(this.loggedUserId).subscribe
    (
      res=>
      {
      this.usernotifications = res;
      }
      ,
      error=>
      {
        console.log(error);
      }
    )
  }

  
  assignTask(batchTaskId :number)
  {

         this.userservice.loggedUser$.pipe(
         take(1) 
           ).subscribe(
         res=> {
          
         this.loggedUserId=res.id;
         this.loggedUser = res;
         console.log(res.id+" from assign component");
         }
         )

        this.taskassign.taskId=batchTaskId;
        this.taskassign.userId= this.loggedUserId;
        // todo : later use switchmap ...to handle one service after another call...
        this.batchtaskservice.getBatchTask(batchTaskId).subscribe(res=>
          {
            this.taskDepartmentId = res.departmentId;
            this.taskTypeId = res.taskTypeId;


            // added...
            this.taskassign.departmentId = res.departmentId;
            this.taskassign.taskTypeId =  res.taskTypeId;


            this.baseTaskType =  this.mappinghelper.
            getBaseTaskType(res.taskTypeId);
            


            this.TaskSeconds = this.mappinghelper.
            getTaskSeconds(res.taskTypeId);
              

              /*
            if (res.taskTypeId===(TaskTypes.RawMaterialsWeighting))
            {this.TaskSeconds=25;}
            if (res.taskTypeId===(TaskTypes.Manufacturing))
            {this.TaskSeconds=22;}
            if (res.taskTypeId===(TaskTypes.RoomCleaning))
            {this.TaskSeconds=15;}
              */
            
           
           
             
          },error=>
          {
            console.log(error)
          });

       // 1- set task as assigned....
       this.batchtaskservice.assign(this.taskassign).subscribe
       (
        res=>
        {
        this.assignSuccess=res;
        if (this.assignSuccess)
        {
           

           // route to suitable path....
    
           if (this.baseTaskType == BaseTaskTypes.CheckedList)
           {
               this.router.navigate(["/checkedList",batchTaskId]);
           }
           else  if (this.baseTaskType == BaseTaskTypes.WeightingMaterialCheckedList)
           {
             this.router.navigate(["/rawMaterial",batchTaskId]);
           }
           else if (this.baseTaskType == BaseTaskTypes.RangeSelect)
           {
             this.router.navigate(["/rangeSelect",batchTaskId]);
              
           }

           // fire the controller 
          
           // join the task group 
            this.presenseService.joinTaskGroups(this.loggedUser ,batchTaskId,this.taskDepartmentId,this.taskTypeId);

            this.taskassign.seconds=  this.TaskSeconds;
           console.log( "TaskAssign Object Department:"+this.taskassign.departmentId);
            this.batchtaskservice.WaitForTaskTimer(this.taskassign).subscribe(res=>{
              
            });
        }
       
        },
        error=>
        {
         this.assignSuccess=false;
        }
       )



  }


}
