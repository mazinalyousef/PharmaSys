import { Component, OnInit } from '@angular/core';
import { batchTasksSummary } from '../_models/batchTasksSummary';
import { BatchtaskService } from '../_services/batchtask.service';
import { ActivatedRoute } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { BatchService } from '../_services/batch.service';

@Component({
  selector: 'app-batch-task-summary',
  templateUrl: './batch-task-summary.component.html',
  styleUrls: ['./batch-task-summary.component.css']
})
export class BatchTaskSummaryComponent implements OnInit
{

    batchtasksummaries : batchTasksSummary[];
    batchno:string;
    displayedColumns = ['taskTitle','taskState', 'user','startDate','endDate'];
    MatdataSource :any;

    Statuscolors = [{ status: "initialized", color: "#0AA2EE" }
                   , { status: "processing", color: "#FECA07" }, 
                   { status: "finished", color: "#2FD294" }]
    
    
    constructor ( private batchTaskService:BatchtaskService,
      private activatedRoute:ActivatedRoute ,
      private batchservice :BatchService
      )
    {

    }
    ngOnInit(): void
     {
       this.loaddata();
     }


     loaddata()
     {
       let Id = this.activatedRoute.snapshot.params['id'];
       if (Id)
       {
        

        // getting bacth main info 
        // this could be done in one model...keep for now 
         this.batchservice.getBatch(Id).subscribe(
          res=>
          {
              this.batchno=  res.batchNO;
          }
          ,error=>
          {
            console.log(error);
          }
         )

        this.batchTaskService.getBatchTaskSummaries(Id).subscribe(
          res=>
          {
              this.batchtasksummaries =res;
              this.MatdataSource = new MatTableDataSource<any>(this.batchtasksummaries);
          },
          error=>
          {
                console.log(error);
          }
        );
       }
      
     }


     getColor(status) {
      return this.Statuscolors.filter(item => item.status === status)[0].color 
      // could be better written, but you get the idea
}

}
