import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { Observable, map, startWith } from 'rxjs';
import { ingredient } from 'src/app/_models/ingredient';
import { product } from 'src/app/_models/product';
import { productIngredient } from 'src/app/_models/productIngredient';
import { ManagementDataService } from 'src/app/_services/management-data.service';
import { ProductService } from 'src/app/_services/product.service';
import * as MessagesTitle from 'src/app/Globals/globalMessages'; 
@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.css']
})
export class ProductEditComponent implements OnInit

{

  ingredientAutoCompletecontrol=new FormControl();
  filteredingredients :Observable<any>;
  displayedColumns = ['ingredientId','ingredientTitle','percentage','actions']; 

  product : product;
  productIngredients :productIngredient[];
  AllIngredients : ingredient[];
  form :FormGroup;
  
  MatdataSource :any;
  selectedProductIngredientItem :productIngredient;
  @Output() saveEvent = new EventEmitter();

  constructor(private productService :ProductService,
    public managementDataService:ManagementDataService,
    private toastr:ToastrService
    )
  {

  }
  ngOnInit(): void
  {

    // get all ingredients 
    this.managementDataService.ingredients$.subscribe(
      res=>
      {
        this.AllIngredients = res;
      }
    );


    this.filteredingredients  =this.ingredientAutoCompletecontrol.valueChanges.
    pipe(startWith(""),map(value=>this._filter(value)));


    this.form =new FormGroup
    (
     {
      productName:new FormControl(''),
     }
    ); 

    this.onnew();
    this.onNewProductIngredient();  
  }
  

  getIngredientFromSelection(selectedItem :ingredient)
  {
     if (selectedItem)
     {
      this.selectedProductIngredientItem.ingredientId = selectedItem.id;
      this.selectedProductIngredientItem.ingredientTitle = selectedItem.ingredientName;
     }
  }
  private _filter(value:string):any
  {
    const filtervalue=value;
    return this.AllIngredients.filter(option=>option.ingredientName.includes(filtervalue));
  }
  getOptionText(option :any) {
    return option ? option.ingredientName : undefined;
  }

  loadProduct()
  {
  this.managementDataService.selectedProduct$.subscribe(
   res=>
   {
     this.product=res;
     this.productIngredients = this.product.productIngredients;
     this.MatdataSource =new MatTableDataSource<any>(this.productIngredients);
     this.form.patchValue(this.product);
    
     this.onNewProductIngredient();
   }
   );
 
  }

  onNewProductIngredient()
  {
    this.selectedProductIngredientItem={} as productIngredient;
    
  }
  deleteProductingredient(Id:number)
  {
 
   // const index: number = this.productIngredients.indexOf(Id);
   // const index: number=-1;

   /*
    if (index !== -1) {
        this.productIngredients.splice(index, 1);
        this.MatdataSource = new MatTableDataSource<any>(this.productIngredients);
    } 
    */   

    this.productIngredients=this.productIngredients.filter((u) => u.ingredientId !== Id)
    this.MatdataSource = new MatTableDataSource<any>(this.productIngredients);
  }

  addProductIngredient()
  {
   // var e = document.getElementById('RoleName').value;

   if (this.selectedProductIngredientItem)
   { 

    let newItem: productIngredient = {
      productId:0,
      ingredientId: this.selectedProductIngredientItem.ingredientId,
      ingredientTitle: this.selectedProductIngredientItem.ingredientTitle,
      percentage: this.selectedProductIngredientItem.percentage
    };

    this.productIngredients.push(newItem);
    this.MatdataSource = new MatTableDataSource<any>(this.productIngredients);
    /*
    if (!this.productIngredients.includes(this.selectedProductIngredientItem))
    {
      this.productIngredients.push(this.selectedProductIngredientItem);
      this.MatdataSource = new MatTableDataSource<any>(this.productIngredients);
    }
    */
  }
  }
  onnew()
  {
     this.product={} as product;
     this.product.id=0;
     this.product.productName="";
    // this.productIngredients = this.product.productIngredients;
     this.productIngredients=[];
     this.form.patchValue(this.product);
     this.MatdataSource = new MatTableDataSource<any>(this.productIngredients);
  }

 isEmptyOrSpaces(str){
    return str === null || str.match(/^ *$/) !== null;
}
 onSubmit()
 {
 
    this.product.productName  = this.form.get("productName")?.value;
    this.product.productIngredients = this.productIngredients;

    if(this.isEmptyOrSpaces( this.product.productName )){
      this.toastr.error("Name Is Required","");
      return;
     }

    if (this.product.id)
    {
       // update...
        this.productService.update(this.product.id,this.product).subscribe
        (
         result=>
         {
           console.log("success");
           this.toastr.info(MessagesTitle.onSaveSuccess,"");
           //reload ....
           this.saveEvent.emit(null);
         }
         ,
         error=>
         {
           console.log(error);
           this.toastr.error(error,"");
         }
         
        )
       
    }
    else
    {
       // add...
       this.productService.add(this.product).subscribe
        (
         result=>
         {
           console.log("success ");
           this.toastr.info(MessagesTitle.onSaveSuccess,"");
            //reload ....
            this.saveEvent.emit(null);
         }
         ,
         error=>
         {
           console.log(error);
           this.toastr.error(error,"");

         }
         
        )
    }

 }


}
