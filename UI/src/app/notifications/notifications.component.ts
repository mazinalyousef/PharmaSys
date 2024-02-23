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
  taskassign : taskAssign={taskId:0,userId:''};
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
       }
     )

     // note : committed to test the observable 
    // this.loadNotifications();   
  }

  loadNotifications()
  {
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


       // 1- set task as assigned....
       this.batchtaskservice.assign(this.taskassign).subscribe
       (
        res=>
        {
        this.assignSuccess=res;
        if (this.assignSuccess)
        {
            this.batchtaskservice.getBatchTask(batchTaskId).subscribe(res=>
            {
              this.baseTaskType =  this.mappinghelper.
              getBaseTaskType(res.taskTypeId);
              console.log(this.baseTaskType);
      
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
              this.presenseService.joinTaskGroups(this.loggedUser ,batchTaskId);
                
           
            },error=>
            {
              console.log(error)
            })
        }
       
        },
        error=>
        {
         this.assignSuccess=false;
        }
       )



  }


}
