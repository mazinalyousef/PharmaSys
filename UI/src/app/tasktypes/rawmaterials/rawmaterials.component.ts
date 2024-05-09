import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { DepartmentsEnum } from 'src/app/_enums/DepartmentsEnum';
import { taskStates } from 'src/app/_enums/taskStates';
import { AuthenticatedResponse } from 'src/app/_models/AuthenticatedResponse';
import { EmailMessage } from 'src/app/_models/EmailMessage';
import { message } from 'src/app/_models/message';
import { rawMaterialsTask } from 'src/app/_models/rawMaterialsTask';
import { BatchtaskService } from 'src/app/_services/batchtask.service';
import { EmailService } from 'src/app/_services/email.service';
import { MessageService } from 'src/app/_services/message.service';
import { PresenceService } from 'src/app/_services/presence.service';
import { StickersService } from 'src/app/_services/stickers.service';
import { UsersService } from 'src/app/_services/users.service';
import * as MessagesTitle from 'src/app/Globals/globalMessages'; 
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-rawmaterials',
  templateUrl: './rawmaterials.component.html',
  styleUrls: ['./rawmaterials.component.css']
})
export class RawmaterialsComponent implements OnInit,OnDestroy
{

   displayedColumns = ['ingredientName','ingredientCode','qtyPerTube','qtyPerBatch','isChecked']; 
   idparam?:number;
   rawmaterialTask : rawMaterialsTask;
   ingredientsdataSource :any;
   isQATask:boolean;
   isWareHouseTask:boolean;
   isAccountantTask:boolean;
   AuthenticatedUser :AuthenticatedResponse;
   firstMangerEmail:string;
   note:string;
   message:message;

   tubeUrl:string;
   cartoonUrl:string;
  baseUrl = environment.apiUrl;


  repeatCount:number;

  iscompleted:boolean;
   
  
   constructor(private activatedRoute : ActivatedRoute, private batchtaskService : BatchtaskService,
    private router : Router, public presenceservice :PresenceService,private userservice :UsersService,
    private messageService:MessageService,private toastr :ToastrService,
     private stickerService:StickersService,
     private emailservice:EmailService
    )
   {
    
   }
   ngOnDestroy(): void
   {

    this.presenceservice.stopTimerHubConnection();

    /*
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
     
  }
  ngOnInit(): void
  {
    this.repeatCount=1;
    this.isQATask=false;
    this.isWareHouseTask=false;
    this.isAccountantTask=false;

    this.loadTask();
  }
  loadTask()
  {
     this.idparam = this.activatedRoute.snapshot.params['id'];
     if (this.idparam)
     {
        this.batchtaskService.getrawmaterialsTask(this.idparam).subscribe
        (
          result=>
          {
            
            this.rawmaterialTask=result;
            this.ingredientsdataSource = new MatTableDataSource<any>(this.rawmaterialTask.batchIngredientDTOs);
               


            this,this.iscompleted=false;
            if (this.rawmaterialTask)
            {
              
			    	if (this.rawmaterialTask.taskStateId===taskStates.finished)
              {
              this.iscompleted=true;
              }

              this.tubeUrl = this.baseUrl+'images/'+this.rawmaterialTask.batchInfo.id.toString()+'_Tube.jpg'
              this.cartoonUrl =this.baseUrl+'images/'+this.rawmaterialTask.batchInfo.id.toString()+'_Cartoon.jpg'
            }
            /*
            if (this.rawmaterialTask.departmentId)
            {
              if (this.rawmaterialTask.departmentId===DepartmentsEnum.Warehouse)
              {
                this.isQATask=false;
                  this.isWareHouseTask=true;
                this.isAccountantTask=false;
              }
              else  if (this.rawmaterialTask.departmentId===DepartmentsEnum.Accounting)
              {
                this.isQATask=false;
                this.isWareHouseTask=false;
                this.isAccountantTask=true;
              }
              else  if (this.rawmaterialTask.departmentId===DepartmentsEnum.QA)
              {
                this.isQATask=true;
                this.isWareHouseTask=false;
                this.isAccountantTask=false;
              }
            }
            */
            console.log(result);
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
      var allchecked=  this.rawmaterialTask.batchIngredientDTOs.every(x=>x.isChecked);
  
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
              console.log("Task Failed To Complete");
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
             this.message.batchId=this.rawmaterialTask.batchId;
             this.message.batchTaskId = this.rawmaterialTask.id;
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

  generateStickers()
  {
        this.stickerService.setSticker(this.rawmaterialTask.batchInfo.batchNO,this.rawmaterialTask.batchInfo.barcode,
          this.rawmaterialTask.productInfo.productName,this.rawmaterialTask.batchIngredientDTOs,this.repeatCount);

          this.router.navigate(['stickers']);

  }
  


}
