using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class TaskTypeCheckList
    {
        public TaskTypeCheckList( )
        {
            
        }

         [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Title { get; set; }

        public int TaskTypeId { get; set; }
        public TaskType TaskType { get; set; }

        public int? TaskTypeGroupId { get; set; }


        [NotMapped]
        public bool isChecked{get;set;}


         #nullable enable
        public TaskTypeGroup? TaskTypeGroup { get; set; }

       

    }
}