import { Component, OnInit } from '@angular/core';
import { batch } from '../_models/batch';
import { batchManufacturingRecord } from '../_models/batchManufacturingRecord';
import { rawMaterialsTask } from '../_models/rawMaterialsTask';
import { checkedListTask } from '../_models/checkedListTask';
import { BatchtaskService } from '../_services/batchtask.service';
import { ActivatedRoute } from '@angular/router';
import { TaskTypes } from '../_enums/TaskTypes';
import { DepartmentsEnum } from '../_enums/DepartmentsEnum';
import { MatTableDataSource } from '@angular/material/table';
import { taskStates } from '../_enums/taskStates';

@Component({
  selector: 'app-batch-records-report',
  templateUrl: './batch-records-report.component.html',
  styleUrls: ['./batch-records-report.component.css']
})
export class BatchRecordsReportComponent implements OnInit
{
   batchRecord :batchManufacturingRecord;
   
   // main info 
   batch:batch={}as batch;
   
   // weightting material tasks
   weightingMaterialTask : rawMaterialsTask={}as rawMaterialsTask;
   // cleaning Room ...
   cleaningRoomTask :checkedListTask={}as checkedListTask;

    // QA Check equipements
    QAcheckEquipementsTask :checkedListTask; 

    // QA raw materials 
     QARawMaterialTask:rawMaterialsTask;
    // Production Check Equipments
     ProductionCheckEquipementTask:checkedListTask;

    // Accountant
     AccountantWeightingMaterialsTask:rawMaterialsTask;
   

   // manufacturing 
    manufacturingTask :checkedListTask;
    
    

    // QA Sampling 
    QASamplingTask : checkedListTask;
    // Filling Check Equpments
    FillingCheckEquipementsTask :checkedListTask; 
      

    // fillings Tasks....

     fillingTubeTask : checkedListTask;
     cartooningTask :checkedListTask;
     packagingTask : checkedListTask;


      // data sources ....

      // cleaning rooms
      CleaningRooms_displayedColumns = ['title','isChecked'];
      CleaningRooms_MatdataSource :any;

      // raw materials 
      RawMaterials_displayedColumns = ['ingredientName','qtyPerTube', 'qtyPerBatch','isChecked'];
      RawMaterials_MatdataSource :any;

      // check equipments -qa
      QACheckEquipments_displayedColumns = ['title','isChecked'];
      QACheckEquipments_MatdataSource :any;

       // check equipments -production
      ProductionCheckEquipments_displayedColumns = ['title','isChecked'];
      ProductionCheckEquipments_MatdataSource :any;


       //  // manufacturing 
       Manufacturing_displayedColumns = ['title','isChecked'];
       Manufacturing_MatdataSource :any;

        // QA Sampling 
      Sampling_displayedColumns = ['title','isChecked'];
      Sampling_MatdataSource :any;

       //  Filling Check Equpments
       FillingCheckEquipments_displayedColumns = ['title','isChecked'];
       FillingCheckEquipments_MatdataSource :any;

        // filling tubes
      FillingTubes_displayedColumns = ['title','isChecked'];
      FillingTubes_MatdataSource :any;

       // filling cartoons
       FillingCartoons_displayedColumns = ['title','isChecked'];
       FillingCartoons_MatdataSource :any;

        // filling packaging
      FillingPackaging_displayedColumns = ['title','isChecked'];
      FillingPackaging_MatdataSource :any;






     
     constructor(private batchTaskService :BatchtaskService
      ,  private activatedRoute:ActivatedRoute 
      ) 
     {
     }

        
  ngOnInit(): void 
  {
    this.loadReport();
  }

  loadReport()
  {
       let Id = this.activatedRoute.snapshot.params['id'];
       if (Id)
       {
        // fill the  batchManufacturingRecord object
        this.batchTaskService.getBatchReport(Id).subscribe
        (
           res =>
           {
            this.batchRecord = res;
            if (this.batchRecord)
            {
              console.log(" enter batch record:"+this.batchRecord);
               // exctracting data from the batchManufacturingRecord object ...
             this.batch  = this.batchRecord.batchForEditDTO; // not necessary assign though
             
             // cleaning rooms data ....
             const roomCleaningfilteredItems=  this.batchRecord.checkedListTaskForViewDTOs.filter(x=>x.taskTypeId===TaskTypes.RoomCleaning)
             if (roomCleaningfilteredItems)
             {
              console.log("roomCleaningfilteredItems :"+roomCleaningfilteredItems);
                this.cleaningRoomTask = roomCleaningfilteredItems[0];
                
                if (this.cleaningRoomTask.taskStateId==taskStates.finished)
                {
                  // alter the checked property
                  this.cleaningRoomTask.taskTypeCheckLists.forEach
                  (function(item, index)
                  {  
                    item.isChecked = true;  
                  }
                ); 
                }
                this.CleaningRooms_MatdataSource = new MatTableDataSource<any>(this.cleaningRoomTask.taskTypeCheckLists);
                
             }
            
             // raw material data
             const rawMaterialfilteredItems = this.batchRecord.rawMaterialsTaskForViewDTOs.filter(x=>x.taskTypeId==TaskTypes.RawMaterialsWeighting&&
              x.departmentId===DepartmentsEnum.Warehouse);
              if (rawMaterialfilteredItems)
              {
                this.weightingMaterialTask = rawMaterialfilteredItems[0];

                if (this.weightingMaterialTask.taskStateId==taskStates.finished)
                {
                  // alter the checked property
                  this.weightingMaterialTask.batchIngredientDTOs.forEach
                  (function(item, index)
                  {  
                    item.isChecked = true;  
                  }
                ); 
                }


                this.RawMaterials_MatdataSource = new MatTableDataSource<any>(this.weightingMaterialTask.batchIngredientDTOs);
              }
    
              //  QA Check Equipments 
              const qaCheckEquipmentsfilteredItems =  this.batchRecord.checkedListTaskForViewDTOs.filter(x=>x.taskTypeId==TaskTypes.Equipments_Machines&&
                x.departmentId===DepartmentsEnum.QA);
                if (qaCheckEquipmentsfilteredItems)
                {
                  this.QAcheckEquipementsTask = qaCheckEquipmentsfilteredItems[0];

                  if (this.QAcheckEquipementsTask.taskStateId==taskStates.finished)
                  {
                    // alter the checked property
                    this.QAcheckEquipementsTask.taskTypeCheckLists.forEach
                    (function(item, index)
                    {  
                      item.isChecked = true;  
                    }
                  ); 
                  }

                  this.QACheckEquipments_MatdataSource = new MatTableDataSource<any>(this.QAcheckEquipementsTask.taskTypeCheckLists);
                }
    
                // qa raw material
                const QaRawmaterialsfilteredItems = this.batchRecord.rawMaterialsTaskForViewDTOs.filter(x=>x.taskTypeId==TaskTypes.RawMaterialsWeighting&&
                  x.departmentId===DepartmentsEnum.QA);
                  if (QaRawmaterialsfilteredItems)
                  {
                    this.QARawMaterialTask = QaRawmaterialsfilteredItems[0];
                  }
    
                  // production check equipments...
                   const productioncheckequipmetsfilteredItems =  this.batchRecord.checkedListTaskForViewDTOs.filter(x=>x.taskTypeId==TaskTypes.Equipments_Machines&&
                    x.departmentId===DepartmentsEnum.Production);
                    if (productioncheckequipmetsfilteredItems)
                    {
                      this.ProductionCheckEquipementTask = productioncheckequipmetsfilteredItems[0];

                      if (this.ProductionCheckEquipementTask.taskStateId==taskStates.finished)
                      {
                        // alter the checked property
                        this.ProductionCheckEquipementTask.taskTypeCheckLists.forEach
                        (function(item, index)
                        {  
                          item.isChecked = true;  
                        }
                      ); 
                      }

                      this.ProductionCheckEquipments_MatdataSource = new MatTableDataSource<any>(this.ProductionCheckEquipementTask.taskTypeCheckLists);
                    }
    
                    // accountant...
                     const accountantfilteredItems = this.batchRecord.rawMaterialsTaskForViewDTOs.filter(x=>x.taskTypeId==TaskTypes.RawMaterialsWeighting&&
                    x.departmentId===DepartmentsEnum.Accounting);
                    if (accountantfilteredItems)
                    {
                      this.AccountantWeightingMaterialsTask = accountantfilteredItems[0];
                    }
    
                     // manufacturing ....
                     const manufacturingfilteredItems =  this.batchRecord.checkedListTaskForViewDTOs.filter(x=>x.taskTypeId==TaskTypes.Manufacturing&&
                      x.departmentId===DepartmentsEnum.Production);
                      if (manufacturingfilteredItems)
                      {
                        this.manufacturingTask = manufacturingfilteredItems[0];

                        if (this.manufacturingTask.taskStateId==taskStates.finished)
                        {
                          // alter the checked property
                          this.manufacturingTask.taskTypeCheckLists.forEach
                          (function(item, index)
                          {  
                            item.isChecked = true;  
                          }
                        ); 
                        }


                        this.Manufacturing_MatdataSource = new MatTableDataSource<any>(this.manufacturingTask.taskTypeCheckLists);
                      }
    
                      // qa sampling
                      const qasamplingfilteredItems =  this.batchRecord.checkedListTaskForViewDTOs.filter(x=>x.taskTypeId==TaskTypes.Sampling&&
                        x.departmentId===DepartmentsEnum.QA);
                        if (qasamplingfilteredItems)
                        {
                          this.QASamplingTask = qasamplingfilteredItems[0];

                          if (this.QASamplingTask.taskStateId==taskStates.finished)
                          {
                            // alter the checked property
                            this.QASamplingTask.taskTypeCheckLists.forEach
                            (function(item, index)
                            {  
                              item.isChecked = true;  
                            }
                          ); 
                          }


                          this.Sampling_MatdataSource = new MatTableDataSource<any>(this.QASamplingTask.taskTypeCheckLists);
                        }
    
                     // Filling Check Equpments
                     const fillingcheckfilteredItems =  this.batchRecord.checkedListTaskForViewDTOs.filter(x=>x.taskTypeId==TaskTypes.Equipments_Machines&&
                      x.departmentId===DepartmentsEnum.Filling);
                      if (fillingcheckfilteredItems)
                      {
                        this.FillingCheckEquipementsTask = fillingcheckfilteredItems[0];

                        if (this.FillingCheckEquipementsTask.taskStateId==taskStates.finished)
                        {
                          // alter the checked property
                          this.FillingCheckEquipementsTask.taskTypeCheckLists.forEach
                          (function(item, index)
                          {  
                            item.isChecked = true;  
                          }
                        ); 
                        }


                        this.FillingCheckEquipments_MatdataSource = new MatTableDataSource<any>(this.FillingCheckEquipementsTask.taskTypeCheckLists);
                      }
    
                      // fillings Tasks....
                       
                      // tubes
                      const tubesfilteredItems =  this.batchRecord.checkedListTaskForViewDTOs.filter(x=>x.taskTypeId==TaskTypes.FillingTubes&&
                        x.departmentId===DepartmentsEnum.Filling);
                        if (tubesfilteredItems)
                        {
                          this.fillingTubeTask = tubesfilteredItems[0];

                          if (this.fillingTubeTask.taskStateId==taskStates.finished)
                          {
                            // alter the checked property
                            this.fillingTubeTask.taskTypeCheckLists.forEach
                            (function(item, index)
                            {  
                              item.isChecked = true;  
                            }
                          ); 
                          }


                          this.FillingTubes_MatdataSource = new MatTableDataSource<any>(this.fillingTubeTask.taskTypeCheckLists);
                        }
                      // cartoon
                      const cartoonfilteredItems =  this.batchRecord.checkedListTaskForViewDTOs.filter(x=>x.taskTypeId==TaskTypes.Cartooning&&
                        x.departmentId===DepartmentsEnum.Filling);
                        if (cartoonfilteredItems)
                        {
                          this.cartooningTask = cartoonfilteredItems[0];

                          if (this.cartooningTask.taskStateId==taskStates.finished)
                          {
                            // alter the checked property
                            this.cartooningTask.taskTypeCheckLists.forEach
                            (function(item, index)
                            {  
                              item.isChecked = true;  
                            }
                          ); 
                          }


                          this.FillingCartoons_MatdataSource = new MatTableDataSource<any>(this.cartooningTask.taskTypeCheckLists);
                        }
                      // package
                      const packagefilteredItems =  this.batchRecord.checkedListTaskForViewDTOs.filter(x=>x.taskTypeId==TaskTypes.Packaging&&
                        x.departmentId===DepartmentsEnum.Filling);
                        if (packagefilteredItems)
                        {
                          this.packagingTask = packagefilteredItems[0];

                          if (this.packagingTask.taskStateId==taskStates.finished)
                          {
                            // alter the checked property
                            this.packagingTask.taskTypeCheckLists.forEach
                            (function(item, index)
                            {  
                              item.isChecked = true;  
                            }
                          ); 
                          }


                          this.FillingPackaging_MatdataSource = new MatTableDataSource<any>(this.packagingTask.taskTypeCheckLists);
                        }
            }  // if batch record exists....
           }
           ,error=>
           {
            console.log(error);
           }
          
        );



      
        
    
         
       } // if id route parameter exists .... 
       
       

  }


}
