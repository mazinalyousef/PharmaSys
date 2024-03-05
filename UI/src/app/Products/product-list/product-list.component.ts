import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { DialogconfirmService } from 'src/app/_services/dialogconfirm.service';
import { ManagementDataService } from 'src/app/_services/management-data.service';
import { ProductService } from 'src/app/_services/product.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit

{

  MatdataSource :any;
  @Output() editEvent = new EventEmitter();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  displayedColumns = ['productName','actions']; 

  constructor(public managementDataService :ManagementDataService,
    private productService : ProductService,private dialogservice:DialogconfirmService)
  {

  }
  ngOnInit(): void 
  
  {
     this.managementDataService.getIngredients();
    this.loadItems();
  }

  loadItems()
  {
    this.managementDataService.getProducts();
    this.managementDataService.products$.subscribe
    (
      res=>
      {
        this.MatdataSource = new MatTableDataSource<any>(res);
        this.MatdataSource.paginator = this.paginator;
      }
    )
  }

  edit(id:number)
  {
      
      this.managementDataService.getProduct(id);
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
    this.productService.delete(id).subscribe(
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
