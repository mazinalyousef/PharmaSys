import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { map } from 'rxjs';
import { DialogconfirmService } from 'src/app/_services/dialogconfirm.service';
import { IngredientService } from 'src/app/_services/ingredient.service';
import { ManagementDataService } from 'src/app/_services/management-data.service';

@Component({
  selector: 'app-ingredient-list',
  templateUrl: './ingredient-list.component.html',
  styleUrls: ['./ingredient-list.component.css']
})
export class IngredientListComponent implements  OnInit 


{

  MatdataSource :any;
  @Output() editEvent = new EventEmitter();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  displayedColumns = ['ingredientName','actions']; 

  pageNumber=1;
  pageSize=5;
  totalItems=1;
  totalPages=1;


  constructor(public managementDataService :ManagementDataService,private ingredientService:IngredientService,
    private dialogservice:DialogconfirmService)
  {

  }
    ngOnInit(): void 
    {
      
      // refresh the ingredients list....
     this.loadItems();
       
    }

      
     handlePageEvent(event: PageEvent)
     {
      this.pageNumber =event.pageIndex;
      this.pageSize=event.pageSize;

      this.loadItems();
     }

    loadItems()
    {


      // client side pagination

    
      this.managementDataService.getIngredients();
      this.managementDataService.ingredients$.subscribe(
        res=>{
          this.MatdataSource = new MatTableDataSource<any>(res);
          this.MatdataSource.paginator = this.paginator;
        }
      )
      
        
     // server side pagination
     /*
     this.managementDataService.getIngredientsPagedResult(this.pageNumber,this.pageSize);

     this.managementDataService.ingredientPagination$.subscribe(res=>
      {
         this.totalItems = res.totalItems;
         this.totalPages =res.totalPages;
         console.log( "Total Items : "+this.totalItems);
      });
      this.managementDataService.Pagedingredients$.subscribe
      (
        res=>
        {
          this.MatdataSource = new MatTableDataSource<any>(res);
        }
      )
     */
     
    }
    edit(id:number)
    {
        // set the selected igredient item 
        console.log("edit ingredient id :"+id.toString())
        this.managementDataService.getIngredient(id);
        this.editEvent.emit(null);

    }
    delete(id:number)
    {

      this.dialogservice.confirmDialog({title:'confirm?',message:'are you sure'}).subscribe(
        dialogres=>
        {
          if(dialogres)
          {
             // delete ....
             this.ingredientService.delete(id).subscribe(
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


    // server side pagination
    gotopage()
    {
      this.loadItems();
    }

}
