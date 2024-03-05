import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Observable, map, startWith } from 'rxjs';
import { barcode } from 'src/app/_models/barcode';
import { product } from 'src/app/_models/product';
import { BarcodeService } from 'src/app/_services/barcode.service';
import { ManagementDataService } from 'src/app/_services/management-data.service';
import { ProductService } from 'src/app/_services/product.service';
import {MatAutocompleteTrigger} from '@angular/material/autocomplete';
import { ToastrService } from 'ngx-toastr';
import * as MessagesTitle from 'src/app/Globals/globalMessages'; 

@Component({
  selector: 'app-barcode-edit',
  templateUrl: './barcode-edit.component.html',
  styleUrls: ['./barcode-edit.component.css']
})
export class BarcodeEditComponent implements OnInit

{

  @ViewChild('input1') ProdAuto: MatAutocompleteTrigger;
  productAutoCompletecontrol :FormControl=new FormControl();
  filteredproducts :Observable<any>;
  barcode : barcode;
  Allproducts : product[];
  form :FormGroup;
  
  selectedProductItem :product;
  @Output() saveEvent = new EventEmitter();


  constructor(private barcodeService :BarcodeService,
    public managementDataService:ManagementDataService,   private toastr:ToastrService)
  {

  }
  
  ngOnInit(): void 
  
  {
     // get all prodcuts 
    this.managementDataService.products$.subscribe(
      res=>
      {
        this.Allproducts = res;
      }
    );

    this.filteredproducts  =this.productAutoCompletecontrol.valueChanges.
    pipe(startWith(""),map(value=>this._filter(value)));


    
        //#region  form group
        this.form =new FormGroup
        ( 
          {
            barcode:new FormControl(''),
            productId:new FormControl(''),
            ndcno:new FormControl(''),
            tubeWeight:new FormControl('')
        }
        );


        this.onnew();
         this.selectedProductItem={} as product;

  }

  private _filter(value:string):any
  {
    const filtervalue=value;
    return this.Allproducts.filter(option=>option.productName.includes(filtervalue));
  }
  getOptionText(option :any) {
    return option ? option.productName : undefined;
  }
  getProductFromSelection(selectedItem :product)
  {
     if (selectedItem)
     {
      this.selectedProductItem.id = selectedItem.id;
     }
  }

  loadBarcode()
  {
  this.managementDataService.selectedBarcode$.subscribe(
   res=>
   {
     this.barcode=res;
     this.form.patchValue(this.barcode);
     // set the selected product...
    // let options = this.ProdAuto.autocomplete.options.toArray();
    // console.log(options);
    // this.productAutoCompletecontrol.setValue(options[1]);
     //Valid
     
     var prod= this.Allproducts.find(x=>x.id===this.barcode.productId);
     console.log(prod);

     this.productAutoCompletecontrol.setValue(prod);
   }
   );
 
  }

  onnew()
  {
     this.barcode={} as barcode;
     this.barcode.id=0;
     this.barcode.barcode="";
     this.barcode.ndcno="";
     this.barcode.productId=0;
     this.barcode.tubeWeight=0; 
     this.form.patchValue(this.barcode);

     //var prod= this.Allproducts.find(x=>x.id===this.barcode.productId);
     this.productAutoCompletecontrol.setValue(null);
    
  }


  isEmptyOrSpaces(str){
    return str === null || str.match(/^ *$/) !== null;
}


  onSubmit()
  {
  
     //this.barcode.productId  = +this.form.get("productId")?.value;
     this.barcode.productId= this.selectedProductItem.id 
     this.barcode.barcode  = this.form.get("barcode")?.value;
     this.barcode.ndcno  = this.form.get("ndcno")?.value;
     this.barcode.tubeWeight  = +this.form.get("tubeWeight")?.value;

     if(this.isEmptyOrSpaces( this.barcode.barcode )){
      this.toastr.error("barcode Is Required","");
      return;
     }

     if (this.barcode.id)
     {
        // update...
         this.barcodeService.update(this.barcode.id,this.barcode).subscribe
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
        this.barcodeService.add(this.barcode).subscribe
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
