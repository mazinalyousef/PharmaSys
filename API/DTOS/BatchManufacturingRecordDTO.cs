using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{


    public class BatchManufacturingRecordDTO
    {

      public BatchManufacturingRecordDTO()
      {
        batchForEditDTO=new BatchForEditDTO ();
        rawMaterialsTaskForViewDTOs = new List<RawMaterialsTaskForViewDTO> ();
        checkedListTaskForViewDTOs =new List<CheckedListTaskForViewDTO> ();
      }
      public BatchForEditDTO batchForEditDTO {get;set;}

      public List<RawMaterialsTaskForViewDTO> rawMaterialsTaskForViewDTOs{get;set;}
      public List<CheckedListTaskForViewDTO> checkedListTaskForViewDTOs {get;set;}

    }
}