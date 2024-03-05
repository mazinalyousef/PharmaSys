import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { ingredient } from '../_models/ingredient';
import { IngredientService } from './ingredient.service';
import { product } from '../_models/product';
import { barcode } from '../_models/barcode';
import { ProductService } from './product.service';
import { BarcodeService } from './barcode.service';
import { Pagination } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class ManagementDataService

{

  private ingredients = new ReplaySubject<ingredient[]>(1);
  ingredients$=this.ingredients.asObservable();

   private selectedIngredient = new ReplaySubject<ingredient>(1);
   selectedIngredient$=this.selectedIngredient.asObservable();


   private Pagedingredients = new ReplaySubject<ingredient[]>(1);
   Pagedingredients$=this.Pagedingredients.asObservable();

   private ingredientPagination = new ReplaySubject<Pagination>(1);
   ingredientPagination$=this.ingredientPagination.asObservable();






   private products = new ReplaySubject<product[]>(1);
   products$=this.products.asObservable();


   private selectedProduct = new ReplaySubject<product>(1);
   selectedProduct$=this.selectedProduct.asObservable();


   private barcodes = new ReplaySubject<barcode[]>(1);
   barcodes$=this.barcodes.asObservable();


   private selectedBarcode = new ReplaySubject<barcode>(1);
   selectedBarcode$=this.selectedBarcode.asObservable();


  constructor(private ingredientService:IngredientService,
  private  productService :ProductService,private  barcodeService :BarcodeService
    )
   { 
    this.ingredients.next([]); // not sure....
    this.selectedIngredient.next(null);

    this.Pagedingredients.next([]);  
    this.ingredientPagination.next(null);


    this.products.next([]);
    this.selectedProduct.next(null);

    this.barcodes.next([]);
    this.selectedBarcode.next(null);
   }


   getIngredients()
  {
      this.ingredientService.getall().subscribe(
        res=>
        {
           
          this.ingredients.next(res);
          console.log(res);
        }
        ,
        error=>
        {
          console.log(error);
        }
      )
  }

  getIngredientsPagedResult(page?:number,itemsPerPage?:number)
  {
        this.ingredientService.getpagedResult(page,itemsPerPage).subscribe(
        res=>
        {
          this.Pagedingredients.next(res.result);
          this.ingredientPagination.next(res.pagination);
          console.log(res);
        }
        ,
        error=>
        {
          console.log(error);
        }
      )
  }



  getIngredient(id:number)
  {
    this.ingredientService.get(id).subscribe(

      res=>
      {
      this.selectedIngredient.next(res);
     // console.log("selectedIngredient is setted in service");
      }
      ,error=>
      {
        console.log(error);
      }
    )
  }

  // product
  getProducts()
  {
      this.productService.getall().subscribe(
        res=>
        {
           
          this.products.next(res);
          console.log(res);
        }
        ,
        error=>
        {
          console.log(error);
        }
      )
  }

  getProduct(id:number)
  {
    this.productService.get(id).subscribe(

      res=>
      {
      this.selectedProduct.next(res);
     // console.log("selectedIngredient is setted in service");
      }
      ,error=>
      {
        console.log(error);
      }
    )
  }

  // Barcode
  getBarcodes()
  {
      this.barcodeService.getall().subscribe(
        res=>
        {
           
          this.barcodes.next(res);
          console.log(res);
        }
        ,
        error=>
        {
          console.log(error);
        }
      )
  }

  getBarcode(barcode:string)
  {
    this.barcodeService.getBarcode(barcode).subscribe(

      res=>
      {
      this.selectedBarcode.next(res);
     // console.log("selectedIngredient is setted in service");
      }
      ,error=>
      {
        console.log(error);
      }
    )
  }
}
