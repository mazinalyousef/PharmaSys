import { Injectable } from '@angular/core';
import { BaseTaskTypes } from '../_enums/BaseTaskTypes';
import { TaskTypes } from '../_enums/TaskTypes';

@Injectable({
  providedIn: 'root'
})
export class MappingHelperService 
{


 
  tasktypeMapper =new Map<TaskTypes,BaseTaskTypes>();
  taskSecondsMapper =new Map<TaskTypes,number>();

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


    this.taskSecondsMapper.set(TaskTypes.RawMaterialsWeighting,25);
    this.taskSecondsMapper.set(TaskTypes.Equipments_Machines,1);
    this.taskSecondsMapper.set(TaskTypes.Manufacturing,22);
    this.taskSecondsMapper.set(TaskTypes.Enviroment,1);
    this.taskSecondsMapper.set(TaskTypes.FillingTubes,1);
    this.taskSecondsMapper.set(TaskTypes.Cartooning,1);
    this.taskSecondsMapper.set(TaskTypes.Packaging,1);
    this.taskSecondsMapper.set(TaskTypes.RoomCleaning,15);
    this.taskSecondsMapper.set(TaskTypes.Sampling,1);

   }

   getBaseTaskType(tasktype : TaskTypes) : BaseTaskTypes
   {
    return this.tasktypeMapper.get(tasktype);
   }
   getTaskSeconds(tasktype : TaskTypes) : number
   {
    return this.taskSecondsMapper.get(tasktype);
   }



}
