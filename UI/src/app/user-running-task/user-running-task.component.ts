import { Component, Input, OnInit } from '@angular/core';
import { userTask } from '../_models/userTask';
import { MappingHelperService } from '../_services/mapping-helper.service';
import { Router } from '@angular/router';
import { BaseTaskTypes } from '../_enums/BaseTaskTypes';
import { UsersService } from '../_services/users.service';
import { PresenceService } from '../_services/presence.service';
import { take } from 'rxjs';
import { AuthenticatedResponse } from '../_models/AuthenticatedResponse';
import { BatchtaskService } from '../_services/batchtask.service';

@Component({
  selector: 'app-user-running-task',
  templateUrl: './user-running-task.component.html',
  styleUrls: ['./user-running-task.component.css']
})
export class UserRunningTaskComponent implements OnInit

{
 
    @Input() userrunningTask:userTask;
    baseTaskType : BaseTaskTypes;
    loggedUserId : string;
    loggedUser:AuthenticatedResponse;
    taskDepartmentId:number;
    taskTypeId:number;
    constructor( private mappinghelper : MappingHelperService, private router:Router,
      private userservice:UsersService, public presenseService : PresenceService,
      private batchtaskservice:BatchtaskService
      )
    {
      
    }
    ngOnInit(): void
     {
    
     }

     enterTask(id:number,takstypeId:number)
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
        

        if (this.loggedUserId)
        {


            this.batchtaskservice.getBatchTask(id).subscribe(res=>
            {
              this.taskDepartmentId = res.departmentId;
              this.taskTypeId = res.taskTypeId;
              this.baseTaskType =  this.mappinghelper.
              getBaseTaskType(res.taskTypeId);
              console.log(this.baseTaskType);
      
              // route to suitable path....
      
              if (this.baseTaskType == BaseTaskTypes.CheckedList)
              {
                  this.router.navigate(["/checkedList",id]);
              }
              else  if (this.baseTaskType == BaseTaskTypes.WeightingMaterialCheckedList)
              {
                this.router.navigate(["/rawMaterial",id]);
              }
              else if (this.baseTaskType == BaseTaskTypes.RangeSelect)
              {
                this.router.navigate(["/rangeSelect",id]);
                 
              }

            
             
              // join the task group 
              this.presenseService.joinTaskGroups(this.loggedUser ,id,this.taskDepartmentId,this.taskTypeId);
               
               
            },error=>
            {
              console.log(error)
            })
  }
       
     }

}
