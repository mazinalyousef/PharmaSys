import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { switchMap } from 'rxjs';
import { batch } from 'src/app/_models/batch';
import { BatchService } from 'src/app/_services/batch.service';
import { DialogconfirmService } from 'src/app/_services/dialogconfirm.service';
import * as MessagesTitle from 'src/app/Globals/globalMessages'; 

@Component({
  selector: 'app-batch-list',
  templateUrl: './batch-list.component.html',
  styleUrls: ['./batch-list.component.css']
})
export class BatchListComponent implements OnInit
{
  displayedColumns = ['batchNO', 'batchSize', 'productName','actions']; 
  batches! : batch[];
  MatdataSource :any;
	@ViewChild(MatPaginator) paginator: MatPaginator;


  constructor(private batchservice:BatchService,private router:Router,
    private toastr:ToastrService,  private dialogservice:DialogconfirmService)
  {

  }
  ngOnInit(): void
   {
    this.loadBatches();

   

  }

  applyFilter(event: Event) {
    
    const filterValue = (event.target as HTMLInputElement).value;
    this.MatdataSource.filter = filterValue.trim().toLowerCase();
}
   

  loadBatches()
  {
      this.batchservice.getBatches().subscribe(res=>
      {
        this.batches = res;
        this.MatdataSource = new MatTableDataSource<any>(this.batches);
        this.MatdataSource.paginator = this.paginator;
        this.MatdataSource.filterPredicate = function(data, filter: number): boolean {
          return data.batchNO===filter;
        };
      })
      
      
  }

  delete(id:number)
  {
 


    this.dialogservice.confirmDialog({title:'confirm?',message:'are you sure'}).subscribe(
      dialogres=>
      {
        if(dialogres)
        {
          console.log('confirmed');
          this.batchservice.deleteBatch(id).subscribe
          (res=>
            {
              this.loadBatches();
              this.batches=this.batches.filter((u) => u.id !== id);
              this.MatdataSource = new MatTableDataSource<any>(this.batches);
              this.MatdataSource.paginator = this.paginator;
              this.toastr.info(MessagesTitle.onDeleteSuccess,"");
            }
        
          );
        }
        else
        {
          console.log('Not confirmed');
        }
      }
      );
	  


    
    
     // location.reload(); // not the best but keep for now 
      
  }
}
