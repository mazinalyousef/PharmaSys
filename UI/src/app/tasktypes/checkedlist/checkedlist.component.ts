import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, NgModel } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, take } from 'rxjs';
import { DepartmentsEnum } from 'src/app/_enums/DepartmentsEnum';
import { TaskTypes } from 'src/app/_enums/TaskTypes';
import { AuthenticatedResponse } from 'src/app/_models/AuthenticatedResponse';
import { checkedListTask } from 'src/app/_models/checkedListTask';
import { message } from 'src/app/_models/message';
import { BatchtaskService } from 'src/app/_services/batchtask.service';
import { MessageService } from 'src/app/_services/message.service';
import { PresenceService } from 'src/app/_services/presence.service';
import { UsersService } from 'src/app/_services/users.service';
import * as MessagesTitle from 'src/app/Globals/globalMessages'; 

@Component({
  selector: 'app-checkedlist',
  templateUrl: './checkedlist.component.html',
  styleUrls: ['./checkedlist.component.css']
})
export class CheckedlistComponent  implements OnInit,OnDestroy
{

   
   idparam?:number;
   checkedlistTask : checkedListTask;
   
   isCheckRoomTask:boolean;
   isManufacturing:boolean;
    AuthenticatedUser:AuthenticatedResponse;

    note:string;
    message:message;

   constructor(private activatedRoute : ActivatedRoute, private batchtaskService : BatchtaskService,
     private router : Router,public presenceservice :PresenceService,private userservice :UsersService
     ,private messageService:MessageService,private toastr :ToastrService
    ) 
     {

     }
   ngOnDestroy(): void
   {

    /*
    console.log("on destroy is reached")
    this.userservice.loggedUser$.pipe(
      take(1) 
     ).subscribe(
       res=> {
       this.AuthenticatedUser=res;
       }
     );
     if (this.AuthenticatedUser)
     {this.presenceservice.RemoveTaskGroups(this.AuthenticatedUser,this.idparam)}
     */
      this.presenceservice.stopTimerHubConnection();
     
  }
  ngOnInit(): void 
  {
       this.isCheckRoomTask=false;
       this.isManufacturing=false;
      
        this.loadTask();
        
  
  }

  loadTask()
  {
     this.idparam = this.activatedRoute.snapshot.params['id'];
     
     if (this.idparam)
     {
        this.batchtaskService.getCheckedListTask(this.idparam).subscribe
        (
          result=>
          {
            this.checkedlistTask=result;

            if (this.checkedlistTask.departmentId)
            {
              if (this.checkedlistTask.taskTypeId===TaskTypes.RoomCleaning)
              {
                this.isCheckRoomTask=true;
               this.isManufacturing=false;
              }
              else  if (this.checkedlistTask.taskTypeId===TaskTypes.Manufacturing)
              {
                this.isCheckRoomTask=false;
                 this.isManufacturing=true;
              }
              
            }
			
          }
          , 
          error=>
          {
              console.log(error);
          }
        )
     }
     
  }
  onComplete()
  {
    if (this.idparam)
    {
      var allchecked= this.checkedlistTask.taskTypeCheckLists.every(x=>x.isChecked);
      //console.log(allchecked);
       if (allchecked)
       {
    
        // call complete task -- API
        this.batchtaskService.complete(this.idparam).subscribe(

          res=>
          {
            if (res)
            {
               // we may make the user leave the task group
              this.router.navigate(['/home']);
            }
            else
            {
              console.log("Task Failed To Complete");// failed successfully..  :)
            }
          
          }
          ,error=>
          {
            console.log(error);
          }
        )
       
       }
    }
  
   
  }

  sendNote()
  {
    this.userservice.loggedUser$.pipe(
      take(1) 
     ).subscribe(
       res=> {
       this.AuthenticatedUser=res;
       }
     );
      if (this.AuthenticatedUser)
      {
        // make a message object

             if (this.note.length>3)
             {
              this.message={} as message;
              this.message.messageText=this.note;
             this.message.batchId=this.checkedlistTask.batchId;
             this.message.batchTaskId = this.checkedlistTask.id;
             this.message.userId = this.AuthenticatedUser.id;
 
               
 
              this.messageService.add(this.message).subscribe(
 
               res=>
               {
                  this.toastr.info(MessagesTitle.OnMessageSentSuccessful,'');
               }
               ,error=>
               {
                 this.toastr.error(error,'');
               }
             )
             }
             else
             {
              this.toastr.warning('Message Lenght is Too Short','');
             }
           
      }
    
  }
  
}
