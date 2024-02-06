import { Component, OnInit } from '@angular/core';
import { MatSelect } from '@angular/material/select';
import { ActivatedRoute, Router } from '@angular/router';
import { rangeSelectTask } from 'src/app/_models/rangeSelectTask';
import { BatchtaskService } from 'src/app/_services/batchtask.service';

@Component({
  selector: 'app-rangeselect',
  templateUrl: './rangeselect.component.html',
  styleUrls: ['./rangeselect.component.css']
})
export class RangeselectComponent implements OnInit
 {

  idparam?:number;
  rangeselectTask : rangeSelectTask;
  groupedRanges:any[];
  selectedgroupedRange :any[];

  constructor(private activatedRoute : ActivatedRoute, private batchtaskService : BatchtaskService,
    private router : Router
   ) { }



  ngOnInit(): void 
  
  {
    this.selectedgroupedRange=new Array();
     this.loadTask();
  }

  onselectionchanged(value)
  {
    console.log(value.id);
    console.log(value.value);

    // add item to array if groupid not exists
    // else remove then add  (update - like )

       if (value.id!=="")
       { 
        const item = this.selectedgroupedRange.find((_item) => _item.taskTypeGroupId === value.id);
        if (item)
             {
           // remove
            const filteredArray = this.selectedgroupedRange.filter(_item => _item.taskTypeGroupId !==  value.id);
            this.selectedgroupedRange =filteredArray;
 
           // push 
 
               var newItem={"taskTypeGroupId": value.id,"rangeValue":value.value};
               this.selectedgroupedRange.push(newItem);
             }
         else
            {
              // push item 
              var newItem={"taskTypeGroupId": value.id,"rangeValue":value.value};
              this.selectedgroupedRange.push(newItem);
            }
          
        }
      

  }

  loadTask()
  {
     this.idparam = this.activatedRoute.snapshot.params['id'];
     if (this.idparam)
     {
        this.batchtaskService.getrangeSelectTask(this.idparam).subscribe
        (
          result=>
          {
            this.rangeselectTask=result;
            const uniqueGroups = [...new Set(
              this.rangeselectTask.taskTypeRangeDTOs.map(item => item.taskTypeGroupTitle)
              )];  
             // console.log(uniqueGroups);

              if (uniqueGroups)
              {

                /*
                 uniqueGroups.forEach((item)=>
                {
                  
                }
                )
                */

                const grouped = Object.values
                  ( this.rangeselectTask.taskTypeRangeDTOs.reduce
                    ((acc, item) =>
                   {
                   
                    acc[item.taskTypeGroupTitle] = [...(acc[item.taskTypeGroupTitle] || []), item];
                    return acc;
                   }, {}
                   )
                   )

                   this.groupedRanges=grouped;
                //  console.log(this.groupedRanges);

               
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

    
  // console.log(this.selectedgroupedRange);

   if (this.selectedgroupedRange)
   {

    // console.log(this.selectedgroupedRange.length);
    // console.log(this.groupedRanges.length);
    if(this.selectedgroupedRange.length===this.groupedRanges.length)
    {
        // pass back data to API
        this.router.navigate(['/home']);
    }
    else
    {
       alert("condition Length Mismatch");
    }
   }

 
   
  }

}
