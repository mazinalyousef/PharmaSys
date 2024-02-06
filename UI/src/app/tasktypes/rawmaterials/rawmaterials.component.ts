import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { rawMaterialsTask } from 'src/app/_models/rawMaterialsTask';
import { BatchtaskService } from 'src/app/_services/batchtask.service';

@Component({
  selector: 'app-rawmaterials',
  templateUrl: './rawmaterials.component.html',
  styleUrls: ['./rawmaterials.component.css']
})
export class RawmaterialsComponent implements OnInit
{

   displayedColumns = ['ingredientName','qtyPerTube','qtyPerBatch','isChecked']; 
   idparam?:number;
   rawmaterialTask : rawMaterialsTask;
   ingredientsdataSource :any;

  
   constructor(private activatedRoute : ActivatedRoute, private batchtaskService : BatchtaskService,
    private router : Router)
   {
    
   }
  ngOnInit(): void
  {
    this.loadTask();
  }
  loadTask()
  {
     this.idparam = this.activatedRoute.snapshot.params['id'];
     if (this.idparam)
     {
        this.batchtaskService.getrawmaterialsTask(this.idparam).subscribe
        (
          result=>
          {
            this.rawmaterialTask=result;
            this.ingredientsdataSource = new MatTableDataSource<any>(this.rawmaterialTask.batchIngredientDTOs);
            console.log(result);
          }
          , 
          error=>
          {
              console.log(error);
          }
        )
     }
     
  }

  onComplete()
  {
   var allchecked=  this.rawmaterialTask.batchIngredientDTOs.every(x=>x.isChecked);
  
  //console.log(allchecked);
   if (allchecked)
   {
    this.router.navigate(['/home']);
   }
   
  }

}
