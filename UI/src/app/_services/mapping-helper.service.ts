import { Injectable } from '@angular/core';
import { BaseTaskTypes } from '../_enums/BaseTaskTypes';
import { TaskTypes } from '../_enums/TaskTypes';

@Injectable({
  providedIn: 'root'
})
export class MappingHelperService 
{


 
  tasktypeMapper =new Map<TaskTypes,BaseTaskTypes>();

  constructor() 
  {
    this.tasktypeMapper.set(TaskTypes.RawMaterialsWeighting,BaseTaskTypes.WeightingMaterialCheckedList);
    this.tasktypeMapper.set(TaskTypes.Equipments_Machines,BaseTaskTypes.CheckedList);
    this.tasktypeMapper.set(TaskTypes.Manufacturing,BaseTaskTypes.CheckedList);
    this.tasktypeMapper.set(TaskTypes.Enviroment,BaseTaskTypes.RangeSelect);
    this.tasktypeMapper.set(TaskTypes.FillingTubes,BaseTaskTypes.CheckedList);
    this.tasktypeMapper.set(TaskTypes.Cartooning,BaseTaskTypes.CheckedList);
    this.tasktypeMapper.set(TaskTypes.Packaging,BaseTaskTypes.CheckedList);
    this.tasktypeMapper.set(TaskTypes.RoomCleaning,BaseTaskTypes.CheckedList);
    this.tasktypeMapper.set(TaskTypes.Sampling,BaseTaskTypes.CheckedList);
   }

   getBaseTaskType(tasktype : TaskTypes) : BaseTaskTypes
   {
    return this.tasktypeMapper.get(tasktype);
   }


}
