using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

             public DbSet<Product> Products { get; set; } 
             public DbSet<ProductType> ProductTypes { get; set; } 

             public DbSet<Ingredient> Ingredients { get; set; } 
             public DbSet<ProductIngredient> ProductIngredients { get; set; } 

             public DbSet<Department> Departments { get; set; }

             public DbSet<Batch> Batches{get;set;}


            // added......was not included but the ef generate the entity...
             public DbSet<BatchIngredient> BatchIngredients {get;set;}



             public DbSet<TaskType> TaskTypes {get;set;}
             public DbSet<TaskTypeGroup> TaskTypeGroups {get;set;}
              public DbSet<TaskTypeCheckList> TaskTypeCheckLists {get;set;}
             public DbSet<TaskTypeRange> TaskTypeRanges {get;set;} 
             public DbSet<BatchState> BatchStates { get; set; }
            public DbSet<TaskState> TaskStates { get; set; }

             public DbSet<BatchTask> BatchTasks { get; set; }
             
            //  public DbSet<BatchTaskIngredientCheckList> BatchTaskIngredientCheckLists{get;set;}

             // public DbSet<BatchTaskCheckedList> BatchTaskCheckedLists{get;set;}

              public DbSet<BatchTaskRange> BatchTaskRanges {get;set;}

               public DbSet<BatchTaskNote> BatchTaskNotes {get;set;}
                public DbSet<BatchTaskCertificate> BatchTaskCertificates {get;set;}
                public DbSet<Barcode> Barcodes {get;set;}
                public DbSet<Notification> Notifications {get;set;}


        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            // these statements are necessary to complete the many-to-many relation between product and ingredients
            builder.Entity<ProductIngredient>().HasKey(pi=>new {pi.ProductId,pi.IngredientId});
            builder.Entity<ProductIngredient>().HasOne(pi=>pi.Product).WithMany(p=>p.ProductIngredients)
            .HasForeignKey(pi=>pi.ProductId);
             builder.Entity<ProductIngredient>().HasOne(pi=>pi.Ingredient).WithMany(p=>p.ProductIngredients)
            .HasForeignKey(pi=>pi.IngredientId);

            // the next two statements can be removed ....
            // these next two statements added to avoid circular refernce issue in the database related tables....
            builder.Entity<TaskTypeGroup>().HasMany(tg=>tg.TaskTypeCheckLists).WithOne(tg=>tg.TaskTypeGroup).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<TaskTypeGroup>().HasMany(tg=>tg.taskTypeRanges).WithOne(tg=>tg.TaskTypeGroup).OnDelete(DeleteBehavior.NoAction);


            builder.Entity<Department>().HasMany(tg=>tg.BatchTasks).WithOne(tg=>tg.Department).OnDelete(DeleteBehavior.NoAction);
             builder.Entity<User>().HasMany(tg=>tg.BatchTasks).WithOne(tg=>tg.User).OnDelete(DeleteBehavior.NoAction);




            
        }
    }
}