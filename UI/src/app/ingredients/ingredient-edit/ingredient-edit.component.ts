import { Component, EventEmitter, NgZone, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { ingredient } from 'src/app/_models/ingredient';
import { IngredientService } from 'src/app/_services/ingredient.service';
import { ManagementDataService } from 'src/app/_services/management-data.service';
import * as MessagesTitle from 'src/app/Globals/globalMessages'; 
 
@Component({
  selector: 'app-ingredient-edit',
  templateUrl: './ingredient-edit.component.html',
  styleUrls: ['./ingredient-edit.component.css'],
 
})
export class IngredientEditComponent implements OnInit 

{

  title!:string;

  form :FormGroup;

  ingredient :ingredient;

  id:number;

  iseditmode:boolean;

  @Output() saveEvent = new EventEmitter();

  constructor(private ingredientservice :IngredientService,
    public managementDataService:ManagementDataService,
    private snackbar :MatSnackBar,
    private toastr:ToastrService
    )
  {
    // this.managementDataService.selectedIngredient$.subscribe(res=>this.ingredient=res);
  }
  ngOnInit(): void 
  
  {
    this.form =new FormGroup
    (
     {
      ingredientName:new FormControl(''),
      ingredientCode:new FormControl(''),
       
     }
    );
   // this.loadIngredient();
   this.ingredient={} as ingredient;  
  }

   loadIngredient()
   {
   this.managementDataService.selectedIngredient$.subscribe(
    res=>
    {
      this.ingredient=res;
      this.form.patchValue(this.ingredient);
    }
    );
  
   }

   onnew()
   {
      this.ingredient={} as ingredient;
      this.ingredient.id=0;
      this.ingredient.ingredientName="";
      this.ingredient.ingredientCode="";
      this.form.patchValue(this.ingredient);
   }


   isEmptyOrSpaces(str){
    return str === null || str.match(/^ *$/) !== null;
}

  onSubmit()
  {
  
     this.ingredient.ingredientName  = this.form.get("ingredientName")?.value;
     this.ingredient.ingredientCode  = this.form.get("ingredientCode")?.value;
     if(this.isEmptyOrSpaces( this.ingredient.ingredientName )){
      this.toastr.error("Name Is Required","");
      return;
     }

     if (this.ingredient.id)
     {
        // update...
         this.ingredientservice.update(this.ingredient.id,this.ingredient).subscribe
         (
          result=>
          {
            //let config = new MatSnackBarConfig();
            //config.duration = 2500;
            //config.panelClass = "snack-success-style"
            console.log("success");
            this.toastr.info(MessagesTitle.onSaveSuccess,"");
            //success,info,warn ,error
             
            /*
            this.snackbar.open(MessagesTitle.onSaveSuccess,'close',
            
              {
                duration: 1000,
                panelClass: ['success'],
              });
              */
              
            //reload ....
            this.saveEvent.emit(null);
          }
          ,
          error=>
          {
            console.log(error);  this.toastr.error(error,"");
          }
          
         )
        
     }
     else
     {
        // add...
        this.ingredientservice.add(this.ingredient).subscribe
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
            console.log(error); this.toastr.error(error,"");
          }
          
         )
     }

  }

}
