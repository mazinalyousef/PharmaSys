import { Component, OnInit } from '@angular/core';
import { FormGroup, NgModel } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { checkedListTask } from 'src/app/_models/checkedListTask';
import { BatchtaskService } from 'src/app/_services/batchtask.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-checkedlist',
  templateUrl: './checkedlist.component.html',
  styleUrls: ['./checkedlist.component.css']
})
export class CheckedlistComponent  implements OnInit
{

   timerdata : number;
   idparam?:number;
   checkedlistTask : checkedListTask;
   

   constructor(private activatedRoute : ActivatedRoute, private batchtaskService : BatchtaskService,
     private router : Router,public presenceservice :PresenceService
    ) { }


  ngOnInit(): void 
  {
      this.loadTask();
      this.timerdata=  this.presenceservice.tasktimerData;
      console.log( this.timerdata);
      console.log( this.presenceservice.timerTick$);
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
  
}
