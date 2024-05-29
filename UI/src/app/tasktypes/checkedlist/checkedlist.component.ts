import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, NgModel } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, take } from 'rxjs';
import { DepartmentsEnum } from 'src/app/_enums/DepartmentsEnum';
import { taskStates } from 'src/app/_enums/taskStates';
import { TaskTypes } from 'src/app/_enums/TaskTypes';
import { AuthenticatedResponse } from 'src/app/_models/AuthenticatedResponse';
import { checkedListTask } from 'src/app/_models/checkedListTask';
import { EmailMessage } from 'src/app/_models/EmailMessage';
import { message } from 'src/app/_models/message';
import { BatchtaskService } from 'src/app/_services/batchtask.service';
import { EmailService } from 'src/app/_services/email.service';
import { MessageService } from 'src/app/_services/message.service';
import { PresenceService } from 'src/app/_services/presence.service';
import { UsersService } from 'src/app/_services/users.service';
import * as MessagesTitle from 'src/app/Globals/globalMessages'; 
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-checkedlist',
  templateUrl: './checkedlist.component.html',
  styleUrls: ['./checkedlist.component.css']
})
export class CheckedlistComponent  implements OnInit,OnDestroy
{

  baseUrl = environment.apiUrl;
   idparam?:number;
   checkedlistTask : checkedListTask;
   
   isCheckRoomTask:boolean;
   isManufacturing:boolean;
    AuthenticatedUser:AuthenticatedResponse;

    note:string;
    message:message;

    tubeUrl:string;
    cartoonUrl:string;

    firstMangerEmail:string;

    iscompleted:boolean;

   constructor(private activatedRoute : ActivatedRoute, private batchtaskService : BatchtaskService,
     private router : Router,public presenceservice :PresenceService,private userservice :UsersService
     ,private messageService:MessageService,private toastr :ToastrService,
     private emailservice:EmailService
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
             
            this.iscompleted=false;
           

            if (this.checkedlistTask)
            {

              if (this.checkedlistTask.taskStateId===taskStates.finished)
              {
                 this.iscompleted=true;
              }
             // this.tubeUrl = this.baseUrl+'images/'+this.checkedlistTask.productInfo.id.toString()+'_Tube.jpg'
             // this.cartoonUrl =this.baseUrl+'images/'+this.checkedlistTask.productInfo.id.toString()+'_Cartoon.jpg'

                this.tubeUrl = this.baseUrl+'images/'+this.checkedlistTask.batchInfo.id.toString()+'_Tube.jpg'
                this.cartoonUrl =this.baseUrl+'images/'+this.checkedlistTask.batchInfo.id.toString()+'_Cartoon.jpg'

            }
           
            /*
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
            */
			
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

              this.presenceservice.stopReminderHubConnection();
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



  sendEmail()
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

      // get the first manager 
       this.emailservice.getFirstManager().subscribe(
        res=>
        {
          this.firstMangerEmail = res.email;
        }
        ,error=>
        {
          console.log(error);
        }
       )


      // testing....
      if (this.note.length>3)
      {

        if (this.firstMangerEmail)
        {

          let emailmessage : EmailMessage ={to:this.firstMangerEmail,
          subject:'from:'+this.AuthenticatedUser.username,
          content:this.note};
           this.emailservice.sendEmail(emailmessage).subscribe(
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
          this.toastr.warning('There Was An Error getting Manager E-Mail To Send To..','');
        }
       
      }
      else
      {
       this.toastr.warning('Message Lenght is Too Short','');
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
