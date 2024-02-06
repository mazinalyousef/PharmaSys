import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { checkedListTask } from 'src/app/_models/checkedListTask';
import { BatchtaskService } from 'src/app/_services/batchtask.service';

@Component({
  selector: 'app-checkedlist',
  templateUrl: './checkedlist.component.html',
  styleUrls: ['./checkedlist.component.css']
})
export class CheckedlistComponent  implements OnInit
{

   
   idparam?:number;
   checkedlistTask : checkedListTask;
   

   constructor(private activatedRoute : ActivatedRoute, private batchtaskService : BatchtaskService,
     private router : Router
    ) { }


  ngOnInit(): void 
  {
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
   var allchecked= this.checkedlistTask.taskTypeCheckLists.every(x=>x.isChecked);
  //console.log(allchecked);
   if (allchecked)
   {
    this.router.navigate(['/home']);
   }
   
  }
  
}
