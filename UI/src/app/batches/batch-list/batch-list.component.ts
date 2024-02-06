import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { switchMap } from 'rxjs';
import { batch } from 'src/app/_models/batch';
import { BatchService } from 'src/app/_services/batch.service';

@Component({
  selector: 'app-batch-list',
  templateUrl: './batch-list.component.html',
  styleUrls: ['./batch-list.component.css']
})
export class BatchListComponent implements OnInit
{
  displayedColumns = ['batchNO', 'batchSize', 'productName','actions']; 
  batches! : batch[];


  constructor(private batchservice:BatchService,private router:Router)
  {

  }
  ngOnInit(): void {
    this.loadBatches();
  }

   

  loadBatches()
  {
      this.batchservice.getBatches().subscribe(res=>
      {
        this.batches = res;
      })
      
      
  }

  delete(id:number)
  {
 
      this.batchservice.deleteBatch(id).subscribe(res=>
      this.loadBatches());
      this.batches=this.batches.filter((u) => u.id !== id)
     // location.reload(); // not the best but keep for now 
      
  }
}
