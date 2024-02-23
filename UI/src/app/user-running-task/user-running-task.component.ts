import { Component, Input, OnInit } from '@angular/core';
import { userTask } from '../_models/userTask';
import { MappingHelperService } from '../_services/mapping-helper.service';
import { Router } from '@angular/router';
import { BaseTaskTypes } from '../_enums/BaseTaskTypes';
import { UsersService } from '../_services/users.service';
import { PresenceService } from '../_services/presence.service';
import { take } from 'rxjs';
import { AuthenticatedResponse } from '../_models/AuthenticatedResponse';

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
    constructor( private mappinghelper : MappingHelperService, private router:Router,
      private userservice:UsersService, public presenseService : PresenceService
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
 // got to task
 this.baseTaskType =  this.mappinghelper.
 getBaseTaskType(takstypeId);
 

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
       this.presenseService.joinTaskGroups(this.loggedUser ,id);
  }
       
     }

}
