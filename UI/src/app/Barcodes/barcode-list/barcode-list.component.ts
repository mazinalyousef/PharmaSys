import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { BarcodeService } from 'src/app/_services/barcode.service';
import { DialogconfirmService } from 'src/app/_services/dialogconfirm.service';
import { ManagementDataService } from 'src/app/_services/management-data.service';

@Component({
  selector: 'app-barcode-list',
  templateUrl: './barcode-list.component.html',
  styleUrls: ['./barcode-list.component.css']
})
export class BarcodeListComponent implements OnInit

{

  MatdataSource :any;
  @Output() editEvent = new EventEmitter();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  displayedColumns = ['barcode','productName','actions']; 


  constructor(public managementDataService :ManagementDataService,
    private barcodeService :BarcodeService,	  private dialogservice:DialogconfirmService)
  {
    
  }
  ngOnInit(): void
  {
    this.loadItems();
  }
  applyFilter(event: Event) {
    
    const filterValue = (event.target as HTMLInputElement).value;
    this.MatdataSource.filter = filterValue.trim().toLowerCase();
}

  loadItems()
  {
    this.managementDataService.getBarcodes();
    this.managementDataService.barcodes$.subscribe
    (
      res=>
      {
        this.MatdataSource = new MatTableDataSource<any>(res);
        this.MatdataSource.paginator = this.paginator;
        this.MatdataSource.filterPredicate = function(data, filter: number): boolean {
          return data.barcode.toLowerCase().includes(filter);
        };
      }
    )
  }
  edit(id:string)
  {
      // set the selected igredient item 
      console.log("editing barcode :"+id.toString())
      this.managementDataService.getBarcode(id);
      this.editEvent.emit(null);

  }
  delete(id:number)
  {


    this.dialogservice.confirmDialog({title:'confirm?',message:'are you sure'}).subscribe(
      dialogres=>
      {
        if(dialogres)
        {
          console.log('confirmed');
            // delete ....
    this.barcodeService.delete(id).subscribe(
      res=>
      {
        console.log("deleted...");
        // refresh data
        this.loadItems();
      }
    )
        }
        else
        {
          console.log('Not confirmed');
        }
      }
      );


  
  }
}
