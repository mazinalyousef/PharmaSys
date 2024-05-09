using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOS;
using API.Entities;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace API.Interfaces
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DataContext _dataContext;

        public TaskRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            
        }
        public async Task<int> Add (BatchTask batchTask)
        {

            var result = _dataContext.BatchTasks.Add(batchTask);
            await _dataContext.SaveChangesAsync();
            return result.Entity.Id;
        }

        public async Task<BatchTask> getBatchTaskInfo(int Id)
        {
           return await _dataContext.BatchTasks.FirstOrDefaultAsync(x=>x.Id==Id);
        }

        public async Task<CheckedListTaskForViewDTO> getCheckedListTaskForView(int Id)
        {

            
            CheckedListTaskForViewDTO checkedListTaskForViewDTO=null;
            // first get the task info along with batch info and task type info 
           var BatchTaskDataSet=  await _dataContext.BatchTasks.AsNoTracking()
           .Include(x=>x.Department)
           .Include(x=>x.User).
            Include(x=>x.TaskType)
            .ThenInclude(x=>x.TaskTypeCheckLists).AsNoTracking().
             FirstOrDefaultAsync(x=>x.Id==Id);

            if (BatchTaskDataSet!=null)
            {

                // get original batch info  along with produc
                var BatchEntity=  await _dataContext.Batches.AsNoTracking().
                Include(x=>x.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(x=>x.Id==BatchTaskDataSet.BatchId);


                // mapping ... here done manually  ... 
                checkedListTaskForViewDTO = new CheckedListTaskForViewDTO();
                checkedListTaskForViewDTO.Id = BatchTaskDataSet.Id;
                checkedListTaskForViewDTO.BatchId = BatchTaskDataSet.BatchId;
                checkedListTaskForViewDTO.DepartmentId = BatchTaskDataSet.DepartmentId;
                checkedListTaskForViewDTO.DurationInSeconds = BatchTaskDataSet.DurationInSeconds;
                checkedListTaskForViewDTO.EndDate = BatchTaskDataSet.EndDate;
                checkedListTaskForViewDTO.StartDate =BatchTaskDataSet.StartDate;
                checkedListTaskForViewDTO.TaskTypeId =BatchTaskDataSet.TaskTypeId;
                checkedListTaskForViewDTO.UserId=BatchTaskDataSet.UserId;


                if (BatchTaskDataSet.User!=null)
                {
                    checkedListTaskForViewDTO.UserName =BatchTaskDataSet.User.UserName;
                }
                //added....
                checkedListTaskForViewDTO.TaskStateId = BatchTaskDataSet.TaskStateId;



               

                string departmentTitle=string.Empty;
                if (BatchTaskDataSet.Department!=null)
                {
                    departmentTitle = BatchTaskDataSet.Department.Title;
                }
                checkedListTaskForViewDTO.Title = string.Format("Task : {0} , Department : {1} ",
                  BatchTaskDataSet.TaskType.Title,departmentTitle
                );

                //  checkedListTaskForViewDTO.taskTypeCheckLists = BatchTaskDataSet.TaskType.TaskTypeCheckLists;

               
                checkedListTaskForViewDTO.taskTypeCheckLists = new List<TaskTypeCheckList>();
                foreach(TaskTypeCheckList taskTypeCheckListItem in BatchTaskDataSet.TaskType.TaskTypeCheckLists)
                {
                    TaskTypeCheckList taskTypeCheckListNewItem = new TaskTypeCheckList()
                    {
                        Id = taskTypeCheckListItem.Id,
                        TaskTypeId= taskTypeCheckListItem.TaskTypeId,
                        TaskTypeGroupId = taskTypeCheckListItem.TaskTypeGroupId,
                        Title = taskTypeCheckListItem.Title,
                        isChecked = taskTypeCheckListItem.isChecked
                    };
                    checkedListTaskForViewDTO.taskTypeCheckLists.Add(taskTypeCheckListNewItem);

                }

                    //added....
                 if (BatchEntity!=null)
                {
                     Batch batchforview = new Batch ();
                     batchforview.Id = BatchEntity.Id;
                    batchforview.Barcode = BatchEntity.Barcode;
                    batchforview.BatchNO = BatchEntity.BatchNO;
                    batchforview.BatchSize = BatchEntity.BatchSize;
                    batchforview.StartDate = BatchEntity.StartDate;
                    batchforview.ExpDate = BatchEntity.ExpDate;
                    batchforview.MFgDate = BatchEntity.MFgDate;
                    batchforview.MFNO = BatchEntity.MFNO;
                    batchforview.NDCNO=BatchEntity.NDCNO;
                    batchforview.TubeWeight = BatchEntity.TubeWeight;
                     batchforview.TubesCount = BatchEntity.TubesCount;
                     batchforview.CartoonsCount =BatchEntity.CartoonsCount;
                    batchforview.MasterCasesCount = BatchEntity.MasterCasesCount;

                    checkedListTaskForViewDTO.BatchInfo=batchforview;
                    if (BatchEntity.Product!=null)
                    {
                        Product productforview = new Product ();
                        productforview.ProductName = BatchEntity.Product.ProductName;
                        productforview.Id =  BatchEntity.Product.Id;
                        checkedListTaskForViewDTO.ProductInfo=productforview;
                    }
                }
               
            }

            return checkedListTaskForViewDTO;


        }

        public async Task<RangeSelectTaskForViewDTO> GetRangeSelectTaskForViewDTO(int Id)
        {
           RangeSelectTaskForViewDTO rangeSelectTaskForViewDTO=null;

            // get the task info along with range data....

            var BatchTaskDataSet = await _dataContext.BatchTasks.AsNoTracking()
            .Include(x=>x.Department)
            .Include(x=>x.TaskType).ThenInclude(x=>x.taskTypeRanges).AsNoTracking()
            .Include(x=>x.TaskType).ThenInclude(x=>x.TaskTypeGroups).AsNoTracking()
            .FirstOrDefaultAsync(x=>x.Id==Id);

            if (BatchTaskDataSet!=null)
            {
                // mapping ... here done manually  ... 
                rangeSelectTaskForViewDTO = new RangeSelectTaskForViewDTO();
                rangeSelectTaskForViewDTO.Id = BatchTaskDataSet.Id;
                rangeSelectTaskForViewDTO.BatchId = BatchTaskDataSet.BatchId;
                rangeSelectTaskForViewDTO.DepartmentId = BatchTaskDataSet.DepartmentId;
                rangeSelectTaskForViewDTO.DurationInSeconds = BatchTaskDataSet.DurationInSeconds;
                rangeSelectTaskForViewDTO.EndDate = BatchTaskDataSet.EndDate;
                rangeSelectTaskForViewDTO.StartDate =BatchTaskDataSet.StartDate;
                rangeSelectTaskForViewDTO.TaskTypeId =BatchTaskDataSet.TaskTypeId;
                rangeSelectTaskForViewDTO.UserId=BatchTaskDataSet.UserId;

                string departmentTitle=string.Empty;
                if (BatchTaskDataSet.Department!=null)
                {
                    departmentTitle = BatchTaskDataSet.Department.Title;
                }
                rangeSelectTaskForViewDTO.Title = string.Format("Task : {0} , Department : {1} ",
                  BatchTaskDataSet.TaskType.Title,departmentTitle
                );

                rangeSelectTaskForViewDTO.taskTypeRangeDTOs = new List<TaskTypeRangeDTO>();
                foreach(TaskTypeRange TaskTypeRangeitem in BatchTaskDataSet.TaskType.taskTypeRanges)
                {
                    TaskTypeRangeDTO TaskTypeRangeDTONewItem = new TaskTypeRangeDTO();
                        TaskTypeRangeDTONewItem.Id = TaskTypeRangeitem.Id;
                        TaskTypeRangeDTONewItem. RangeValue =TaskTypeRangeitem.RangeValue;
                        TaskTypeRangeDTONewItem.TaskTypeGroupId = TaskTypeRangeitem.TaskTypeGroupId;
                        TaskTypeRangeDTONewItem.TaskTypeId = TaskTypeRangeitem.TaskTypeId;
                        TaskTypeRangeDTONewItem.TaskTypeGroupTitle = "";
                        if (BatchTaskDataSet.TaskType.TaskTypeGroups!=null)// not the best way to find the groupt Title...
                        {
                            if (TaskTypeRangeitem.TaskTypeGroupId!=null)
                            {
                                var groupItem= BatchTaskDataSet.TaskType.TaskTypeGroups.Where(x=>x.Id==TaskTypeRangeitem.TaskTypeGroupId).
                                 FirstOrDefault();
                                 if (groupItem!=null)
                                 {
                                     TaskTypeRangeDTONewItem.TaskTypeGroupTitle = groupItem.Title;
                                 }
                            }
                              
                        }

                    rangeSelectTaskForViewDTO.taskTypeRangeDTOs.Add(TaskTypeRangeDTONewItem);

                }
               
            }
           return   rangeSelectTaskForViewDTO;

        }

        public async Task<RawMaterialsTaskForViewDTO> GetRawMaterialsTaskForView(int Id)
        {
             RawMaterialsTaskForViewDTO rawMaterialsTaskForViewDTO=null;
             // first get the task info along with batch info and task type info 
              var BatchTaskDataSet=  await _dataContext.BatchTasks.AsNoTracking()
            .Include(x=>x.Department)
            .Include(x=>x.TaskType)
            .Include(x=>x.User)
            .Include(x=>x.Batch).ThenInclude(x=>x.BatchIngredients).ThenInclude(x=>x.Ingredient).AsNoTracking().
             FirstOrDefaultAsync(x=>x.Id==Id);

                if (BatchTaskDataSet!=null)
                {

                    // get original batch info  along with produc
                var BatchEntity=  await _dataContext.Batches.AsNoTracking().
                Include(x=>x.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(x=>x.Id==BatchTaskDataSet.BatchId);


                        // mapping ... here done manually  ... 
                rawMaterialsTaskForViewDTO = new RawMaterialsTaskForViewDTO();
                rawMaterialsTaskForViewDTO.Id = BatchTaskDataSet.Id;
                rawMaterialsTaskForViewDTO.BatchId = BatchTaskDataSet.BatchId;
                rawMaterialsTaskForViewDTO.DepartmentId = BatchTaskDataSet.DepartmentId;
                rawMaterialsTaskForViewDTO.DurationInSeconds = BatchTaskDataSet.DurationInSeconds;
                rawMaterialsTaskForViewDTO.EndDate = BatchTaskDataSet.EndDate;
                rawMaterialsTaskForViewDTO.StartDate =BatchTaskDataSet.StartDate;
                rawMaterialsTaskForViewDTO.TaskTypeId =BatchTaskDataSet.TaskTypeId;
                rawMaterialsTaskForViewDTO.UserId=BatchTaskDataSet.UserId;

                //added...
                if (BatchTaskDataSet.User!=null)
                {
                    rawMaterialsTaskForViewDTO.UserName = BatchTaskDataSet.User.UserName;
                }
               
                //added..
                rawMaterialsTaskForViewDTO.TaskStateId = BatchTaskDataSet.TaskStateId;
                  
                string departmentTitle=string.Empty;
                if (BatchTaskDataSet.Department!=null)
                {
                    departmentTitle = BatchTaskDataSet.Department.Title;
                }
                rawMaterialsTaskForViewDTO.Title = string.Format("Task : {0} , Department : {1} ",
                  BatchTaskDataSet.TaskType.Title,departmentTitle
                );

                // mapping the ingredients.....(manually for now..avoiding injecting automapper here)
                 rawMaterialsTaskForViewDTO.batchIngredientDTOs =new List<BatchIngredientDTO>();
                 foreach(var item in BatchTaskDataSet.Batch.BatchIngredients)
                 {
                     BatchIngredientDTO batchIngredientNewItem =new BatchIngredientDTO ();
                     batchIngredientNewItem.Id=item.Id; // not necessary thuogh
                     batchIngredientNewItem.BatchId = item.BatchId;
                     batchIngredientNewItem.IngredientId = item.IngredientId;
                     batchIngredientNewItem.IngredientName = item.Ingredient.IngredientName;
                     batchIngredientNewItem.IngredientCode = item.Ingredient.IngredientCode;
                     batchIngredientNewItem.IsChecked=false;
                     batchIngredientNewItem.QTYPerBatch = item.QTYPerBatch;
                     batchIngredientNewItem.QTYPerTube = item.QTYPerTube;
                     rawMaterialsTaskForViewDTO.batchIngredientDTOs.Add(batchIngredientNewItem);
                 }

                      //added....
                 if (BatchEntity!=null)
                {
                     Batch batchforview = new Batch ();
                     batchforview.Id=BatchEntity.Id;
                    batchforview.Barcode = BatchEntity.Barcode;
                    batchforview.BatchNO = BatchEntity.BatchNO;
                    batchforview.BatchSize = BatchEntity.BatchSize;
                    batchforview.StartDate = BatchEntity.StartDate;
                    batchforview.ExpDate = BatchEntity.ExpDate;
                    batchforview.MFgDate = BatchEntity.MFgDate;
                    batchforview.MFNO = BatchEntity.MFNO;
                    batchforview.NDCNO=BatchEntity.NDCNO;
                    batchforview.TubeWeight = BatchEntity.TubeWeight;
                    batchforview.TubesCount = BatchEntity.TubesCount;
                    batchforview.CartoonsCount =BatchEntity.CartoonsCount;
                    batchforview.MasterCasesCount = BatchEntity.MasterCasesCount;


                    rawMaterialsTaskForViewDTO.BatchInfo=batchforview;
                    if (BatchEntity.Product!=null)
                    {
                        Product productforview = new Product ();
                        productforview.ProductName = BatchEntity.Product.ProductName;
                        productforview.Id = BatchEntity.Product.Id;
                        rawMaterialsTaskForViewDTO.ProductInfo=productforview;
                    }
                }

                }

             return rawMaterialsTaskForViewDTO;
        }
        
        public async Task<bool> SetAsAssigned(int TaskId,string UserId)
        {

            // use transaction .. later

            bool completed=false;
                var originalEntity = _dataContext.BatchTasks.FirstOrDefault(x=>x.Id==TaskId);
             
            
            
            if (originalEntity!=null)
            {

                // additional check Task is not assigned by another user

                 if (originalEntity.UserId==null) // the task is not assigned by  a user 
                {
                    
                  using (IDbContextTransaction transaction=await _dataContext.Database.BeginTransactionAsync())
				  {
					  try 
					  {
						  

                            			     // setting new properties for the original entity
                    originalEntity.UserId = UserId;
                    originalEntity.StartDate =DateTime.Now;
                    originalEntity.TaskStateId = (int) Enumerations.TaskStatesEnum.processing;

                    _dataContext.Entry(originalEntity).Property(x=>x.UserId).IsModified=true;
                    _dataContext.Entry(originalEntity).Property(x=>x.StartDate).IsModified=true;
                    _dataContext.Entry(originalEntity).Property(x=>x.TaskStateId).IsModified=true;

                    // update also the notification-assigned by user property 
                    // note : update all the notifications related the the task....
                    var originalNotificationEntities = _dataContext.Notifications.Where(x=>x.BatchTaskId==TaskId).ToList();
                    if (originalNotificationEntities!=null)
                    {
                    foreach(var _item in originalNotificationEntities)
                    {
                     _item.AssignedByUserId=UserId;
                    _dataContext.Entry(_item).Property(x=>x.AssignedByUserId).IsModified=true;
                    
                    }
                  // we may want to push notifications here so other users can see that the user takes this specific task
                     }
                        await _dataContext.SaveChangesAsync();

                        await transaction.CommitAsync();
                        completed=true;

					  }  // try 
					    catch(Exception)
						{
							await transaction.RollbackAsync();
						}
				  }
                } // if task is not assigned by a user...
                                  
            }    // if original entity isnot null     
            return completed;
        }

         public async Task<bool> SetAsCompleted(int TaskId)
         {
            bool completed=false;

            var originalEntity = _dataContext.BatchTasks.FirstOrDefault(x=>x.Id==TaskId);
            if (originalEntity!=null)
            {
                // setting new properties for the original entity
                originalEntity.EndDate =DateTime.Now;
                originalEntity.TaskStateId = (int) Enumerations.TaskStatesEnum.finished;
                _dataContext.Entry(originalEntity).Property(x=>x.EndDate).IsModified=true;
                _dataContext.Entry(originalEntity).Property(x=>x.TaskStateId).IsModified=true;
                  await _dataContext.SaveChangesAsync();
                  completed=true;

            } 
            return completed;
         }

         public List<BatchTask> getBatchTasks(int _batchId)
         {
            List<BatchTask> batchTasks=new  List<BatchTask>();
            batchTasks = _dataContext.BatchTasks.Where(x=>x.BatchId==_batchId).ToList();
            return batchTasks;
         }

         public BatchTask GetBatchTask(int _batchId,int _taskIdtypeId,int _departmentId)
         {
            BatchTask batchTask= new BatchTask();
            batchTask= _dataContext.BatchTasks.Where(x=>x.TaskTypeId==_taskIdtypeId&&x.BatchId==
            _batchId&&x.DepartmentId==_departmentId).FirstOrDefault();
            return batchTask;
         }

         public async Task<IEnumerable<BatchTask>>GetUserRunningTasks(string userId)
         {
            return await _dataContext.BatchTasks.
             Include(x=>x.Batch).ThenInclude(x=>x.Product)
            .Include(x=>x.TaskType)
            .Where(x=>x.UserId==userId&&x.TaskStateId==(int)Enumerations.TaskStatesEnum.processing).ToListAsync();
         }

        public async Task<IEnumerable<BatchTaskSummaryDTO>> GetBatchTaskSummaries(int Id)
        {

            List<BatchTaskSummaryDTO> batchTaskSummaryDTOs =new List<BatchTaskSummaryDTO> ();
            var batchtasks =await _dataContext.BatchTasks.AsNoTracking()
            .Include(x=>x.Department)
            .Include(x=>x.TaskType)
            .Include(x=>x.User)
            .Include(x=>x.TaskState)
            .Where(x=>x.BatchId==Id&&x.TaskTypeId!=(int)Enumerations.TaskTypesEnum.Enviroment)
            .OrderBy(x=>x.Id)
            .ToListAsync();
            if (batchtasks!=null)
            {
                foreach(var item in batchtasks)
                {
                    string departmentTitle = item.Department!=null? item.Department.Title:"";
                    string taskTitle =  item.TaskType.Title;
                    string userName = item.User!=null? item.User.UserName:"";
                    string taskState = item.TaskState.Title;
                      

                    string TitleForDisplay = string.Format("{0} - {1} ",taskTitle,departmentTitle);

                    // create new batchtasksummaryDTO 
                    BatchTaskSummaryDTO batchTaskSummaryDTO=new BatchTaskSummaryDTO ()
                    {
                        Id=item.Id,//added..
                        TaskTypeId= item.TaskTypeId,//added...
                       StartDate=item.StartDate,
                       EndDate= item.EndDate,
                       TaskTitle = TitleForDisplay,
                        User=userName,
                        TaskState = taskState,
                       

                       
                    };
                    
                     batchTaskSummaryDTO.Totalminutes = item.StartDate.HasValue&&item.EndDate.HasValue? (int) (item.EndDate.Value-item.StartDate.Value).TotalMinutes:0;
                     batchTaskSummaryDTOs.Add(batchTaskSummaryDTO);

                }
            }
             return batchTaskSummaryDTOs;
        }
    }
}