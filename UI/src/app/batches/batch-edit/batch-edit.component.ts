import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup,Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject, filter, map, take } from 'rxjs';
import { BatchStates } from 'src/app/_enums/batchStates';
import { batch } from 'src/app/_models/batch';
import { batchIngredient } from 'src/app/_models/batchIngredient';
import { BarcodeService } from 'src/app/_services/barcode.service';
import { BatchService } from 'src/app/_services/batch.service';
import { ProductService } from 'src/app/_services/product.service';
import { UsersService } from 'src/app/_services/users.service';
import * as MessagesTitle from 'src/app/Globals/globalMessages'; 
import { environment } from 'src/environments/environment';
@Component
({
  selector: 'app-batch-edit',
  templateUrl: './batch-edit.component.html',
  styleUrls: ['./batch-edit.component.css']
})

export class BatchEditComponent  implements OnInit
{
   
  baseUrl = environment.apiUrl;
  displayedIngredientsColumns = ['ingredientName','ingredientCode', 'qtyPerTube','qtyPerBatch']; 


   title:string;

   form : FormGroup;

   iseditmode:boolean;

   stateIsNotInitialized:boolean;

    batch : batch;

    batchingredients : batchIngredient[];

    idparameter?: number;

    loggedUserId : string;


    uploadTubeFile: File | null;
    uploadTubeFileLabel: string | undefined = 'Choose Tube image to upload';
     tubeUrl:string;
  
  
    uploadCartoonFile: File | null;
    uploadCartoonFileLabel: string | undefined = 'Choose Cartoon image to upload';
    cartoonUrl:string;

    masterCaseSize:number;

    constructor(private activatedRoute:ActivatedRoute,
      private router:Router,private batchservice:BatchService,
      private barcodeservice:BarcodeService,
      private userservice:UsersService, private toastr:ToastrService,
      private productService:ProductService)
    {
      this.masterCaseSize=24;
    }

    ngOnInit(): void
    {

         /*
        this.userservice.loggedUser$.subscribe( 
          ite =>
          {
            console.log(ite.username);
            this.loggedUserId=ite.id;
          }
        ,
        error=>
        {
          console.log(error);
          this.loggedUserId="";
        }
        ) ;

        */

        this.userservice.loggedUser$.pipe(
         take(1) 
        ).subscribe(
          res=> {
            console.log(res.username);
          this.loggedUserId=res.id;
          }
        )


        //#region  form group
        this.form =new FormGroup
        ( 
          {
          batchNO:new FormControl('',Validators.required),
          batchSize:new FormControl('',Validators.required),
          mFgDate:new FormControl('',Validators.required),
          expDate:new FormControl('',Validators.required),
          revision:new FormControl(''),
          revisionDate:new FormControl('',Validators.required),
          barcode:new FormControl('',Validators.required),
          mfno:new FormControl(''),
          ndcno:new FormControl('',Validators.required),
          productId:new FormControl(''),
          productName:new FormControl(''),
          tubeWeight:new FormControl(''),
          tubesCount:new FormControl(''),
          cartoonsCount:new FormControl(''),
          masterCasesCount:new FormControl(''),
        }
        )
       
        //#endregion
        
      

        this.loadBatch();

    }

   
    
   
    
    loadBatch()
    {
      this.clearFilesData();
     // this.idparameter = +this.activatedRoute.snapshot.params['id'];
      this.idparameter = this.activatedRoute.snapshot.params['id'];

      if (this.idparameter)
      {
        this.iseditmode=true;

        // load 
        this.batchservice.getBatch(this.idparameter).subscribe
        (
res=>
{
  this.batch = res;
  this.batchingredients = this.batch.batchIngredients; // remove later...no need
  this.title="Editing Batch No :"+this.batch.batchNO;
  this.form.patchValue( this.batch);

  this.stateIsNotInitialized = this.batch.batchStateId!==BatchStates.initialized;

   // load images...
   this.tubeUrl = this.baseUrl+'images/'+this.batch.id.toString()+'_Tube.jpg'
   this.cartoonUrl =this.baseUrl+'images/'+this.batch.id.toString()+'_Cartoon.jpg'
}
,error=>
{
console.log(error);  this.stateIsNotInitialized=true;
}
        )
      }
      else
      {
        this.stateIsNotInitialized=true;
        this.iseditmode=false;
        this.title="Add a New Batch...";
        this.batch = {} as batch;
         this.form.patchValue(this.batch);
      }
    }



    handleTubeFileInput(files: FileList)
  {
    if (files.length > 0)
     {
      this.uploadTubeFile = files.item(0);
      this.uploadTubeFileLabel = this.uploadTubeFile?.name;

      const reader = new FileReader();
      reader.readAsDataURL(this.uploadTubeFile); 
      reader.onload = (_event) => { 
          this.tubeUrl = reader.result as string; 
      }

    }
  }
  handleCartoonFileInput(files: FileList)
  {
    if (files.length > 0)
     {
      this.uploadCartoonFile = files.item(0);
      this.uploadCartoonFileLabel = this.uploadCartoonFile?.name;

      const reader = new FileReader();
      reader.readAsDataURL(this.uploadCartoonFile); 
      reader.onload = (_event) => { 
          this.cartoonUrl = reader.result as string; 
      }

    }
  }

  
 uploadTubePhoto(id:number)
 {
  const formData = new FormData();
  // files ...
   if (this.uploadTubeFile)
    {
    // formData.append(this.product.id.toString(), this.uploadTubeFile);
     formData.append(id.toString(), this.uploadTubeFile);
      // call service
    this.batchservice.addTubeImage(formData).subscribe(
      res=>
      {
        console.log(res);
        this.toastr.info(MessagesTitle.onSaveSuccess,"");
      }
      ,error=>
      {
        console.log(error);
        this.toastr.error(error,"");
      }
    )
    }
   
 }

 
 uploadCartoonPhoto(id:number)
 {
  const formData = new FormData();
    // files ...
     if (this.uploadCartoonFile)
      {
       formData.append(id.toString(), this.uploadCartoonFile);
         // call service
      this.batchservice.addCartoonImage(formData).subscribe(
        res=>
        {
          console.log(res);
          this.toastr.info(MessagesTitle.onSaveSuccess,"");
        }
        ,error=>
        {
          console.log(error);
          this.toastr.error(error,"");
        }
      )
      }
    
  
 }


  clearFilesData()
  {
    this.uploadTubeFile = null; this.uploadTubeFileLabel='';
    this.uploadCartoonFile=null; this.uploadCartoonFileLabel='';
     this.tubeUrl='';
     this.cartoonUrl='';
  }


    onSubmit()
    {


      if (!this.form.valid)
      {
          this.toastr.error("Invalid Data,Please Check Again","error");
          return
      }
       this.userservice.loggedUser$.pipe(
        take(1) 
       ).subscribe(
         res=> {
           console.log(res.username);
         this.loggedUserId=res.id;
         }
       )
      //#region  Getting The Batch Values from Form
    
      this.batch.batchNO  = this.form.get("batchNO")?.value;
      this.batch.batchSize  = +this.form.get("batchSize")?.value;
      this.batch.barcode  = this.form.get("barcode")?.value;
      this.batch.revision = this.form.get("revision")?.value;
      this.batch.revisionDate = this.form.get("revisionDate")?.value;
      this.batch.revisionDate =new Date(this.GetDateFromIsoString(this.batch.revisionDate));
      // testing...
    //  console.log("rev Date:"+ this.batch.revisionDate)
    //  console.log("rev Date toLocaleString:"+ this.batch.revisionDate.toLocaleString())
      //console.log("rev Date toJSON:"+ this.batch.revisionDate.toJSON())

      
      // This will return an ISO string matching your local time.
    

      this.batch.expDate = this.form.get("expDate")?.value;
      this.batch.expDate =new Date(this.GetDateFromIsoString(this.batch.expDate));
      
      this.batch.cartoonPictureURL  = "";
      this.batch.mfno  = this.form.get("mfno")?.value;


      this.batch.mFgDate  = this.form.get("mFgDate")?.value; // re- check for dates
      this.batch.mFgDate =new Date(this.GetDateFromIsoString(this.batch.mFgDate));


      this.batch.ndcno  = this.form.get("ndcno")?.value;
      this.batch.productId  = +this.form.get("productId")?.value;
   //   this.batch.userId  = "0b17e502-1da0-45f4-80c9-6d104734a8dd"; // replace with logged user Id....
       this.batch.userId  =this.loggedUserId;
      this.batch.tubePictureURL  = "";
      this.batch.cartoonPictureURL  = "";
      this.batch.tubeWeight  = +this.form.get("tubeWeight")?.value;
      this.batch.tubesCount  = +this.form.get("tubesCount")?.value;
      this.batch.cartoonsCount  = +this.form.get("cartoonsCount")?.value;
      this.batch.masterCasesCount  = +this.form.get("masterCasesCount")?.value;
       //#endregion


       if (this.idparameter)
       {
         // update...
         this.batchservice.updateBatch(this.idparameter,this.batch).subscribe
         (
          res=>
          {

              // upload photos
            this.uploadTubePhoto(this.idparameter);
            this.uploadCartoonPhoto(this.idparameter);

             console.log("Batch"+this.batch.batchNO+"has been updated");
             this.toastr.info(MessagesTitle.onSaveSuccess,"");
             this.router.navigate(['/batches']);
          }
          ,error=>
          {
             console.log(error);
             this.toastr.error(error,"");
          }
         )
       }
       else
       {

           this.batch.batchStateId  = BatchStates.initialized; // replace with enumeration ... later
          // insert....
          this.batchservice.addBatch(this.batch).subscribe(res=>
            {
              if (res)
              {
                // add photos
              this.uploadTubePhoto(res);
              this.uploadCartoonPhoto(res);
              }
              console.log("Batch " + this.batch.batchNO + " has been Added.");
              this.toastr.info(MessagesTitle.onSaveSuccess,"");
              this.router.navigate(['/batches']);
            }
            ,error=>
            {
              console.log(error);
              this.toastr.error(error,"");
            }
            )
       }
    }

     

    GetDateFromIsoString(_date:Date) :string
    {
      var d =new Date(_date);
      var ss=  new Date(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours(), d.getMinutes() - d.getTimezoneOffset()).toISOString();
      
      return ss;

    }


    ontubeWeightBlur(event:any)
    {
      this.calculateTubesCount();
    }

    onbatchSizeBlur(event:any)
    {
      this.calculateTubesCount();

    }
    onBarcodeBlur(event:any)
    {

       
  this.barcodeservice.getBarcode(event.target.value).subscribe
   (
  res=>
  {
    
     console.log(res);
     // do i need to reset the form value ?
     this.batch.productId = res.productId;
     this.batch.productName = res.productName;
     this.batch.ndcno = res.ndcno;
     this.batch.tubeWeight = res.tubeWeight;

     

     this.form.patchValue({
      productId:this.batch.productId,
      productName:this.batch.productName,
      ndcno:this.batch.ndcno,
      tubeWeight:this.batch.tubeWeight,
     // tubesCount:this.batch.tubesCount
     });

     // after that i need to set the tubes count ...

     this.calculateTubesCount();
     
  }

   ,error=>
   {
    console.log(error);
    this.batch.productId = 0;
    this.batch.productName="";
    this.batch.ndcno = "";
    this.batch.tubeWeight = 0;
    this.batch.tubesCount = 0;
    this.batch.cartoonsCount = 0;
    this.batch.masterCasesCount = 0;
    
    this.form.patchValue({
      productId:this.batch.productId,
      productName:this.batch.productName,
      ndcno:this.batch.ndcno,
      tubeWeight:this.batch.tubeWeight,
     });

     // after that i need to set the tubes count ...
     this.calculateTubesCount();
   }
)

    

   
   }


   calculateTubesCount():void
   {
     
    let  batchsize :number  = +this.form.get("batchSize")?.value;
    let  tubewight :number = +this.form.get("tubeWeight")?.value;
   // let  batchsize = +this.batch.batchSize ;
   // let  tubewight =+ this.batch.tubeWeight;

    let    batchsizegrams = batchsize * 1000;
   // console.log(batchsizegrams);
   let tubescount :number = 1;
   let mastercasescount :number=1;
    if (tubewight!=0)
    {
    tubescount = Math.ceil((batchsizegrams) / (tubewight)) ;
    this.batch.tubesCount =  +tubescount ;
    this.batch.cartoonsCount = + tubescount;
    mastercasescount= Math.ceil((tubescount) / ( this.masterCaseSize)) ;
    this.batch.masterCasesCount = + mastercasescount;
    this.form.patchValue(
      {
        tubesCount:this.batch.tubesCount,
        cartoonsCount:this.batch.cartoonsCount
        , masterCasesCount:this.batch.masterCasesCount
      }
    );

   // console.log(tubescount);
    }
    else
    {
      this.batch.tubesCount = 0;
      this.batch.cartoonsCount = 0;
      this.batch.masterCasesCount = 0;
      this.form.patchValue(
        {
          tubesCount:this.batch.tubesCount,
        cartoonsCount:this.batch.cartoonsCount
        , masterCasesCount:this.batch.masterCasesCount
        }
      )
    }
     
   }




   SendBatch()
   {
    if (this.idparameter)
    {
      this.batchservice.sendBatch(this.idparameter).subscribe(res=>
        {
            if (res==true)
            {
              
              this.toastr.info("Batch Sended Successfully","");
            }
            else
            {
              
              this.toastr.error("Batch Send Was Unsuccessfull","");
            }
        }
        ,error=>
        {
          console.log(error);
          this.toastr.error("Batch Send Was Unsuccessfull","");
        }
        )
    }
    else
    {
      console.log("No Batch Id Was Provided....");
    }
    
    
   }


 
  





}
