import { Component, Input, OnInit } from '@angular/core';
import { batch } from '../_models/batch';

@Component({
  selector: 'app-batch-report-header',
  templateUrl: './batch-report-header.component.html',
  styleUrls: ['./batch-report-header.component.css']
})
export class BatchReportHeaderComponent implements OnInit

{ 
   @Input() _batch:batch ;
   
   
   constructor() {
    
    
   }
  ngOnInit(): void 
  {
      
  }

}
