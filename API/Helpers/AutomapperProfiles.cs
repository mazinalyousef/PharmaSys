using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOS;
using API.Entities;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;

namespace API.Helpers
{
    public class AutomapperProfiles :Profile
    {
        public AutomapperProfiles()
        {
            //CreateMap from source to destination ....
            CreateMap<User,UserForViewDTO>().ForMember(des=>des.Department,opt=>opt.MapFrom(src=>src.Department.Title));
            CreateMap<Batch,BatchForViewDTO>().ForMember(des=>des.ProductName,opt=>opt.MapFrom(src=>src.Product.ProductName));;
            CreateMap<Batch,BatchForEditDTO>()
            .ForMember(des=>des.ProductName,opt =>opt.MapFrom(src=>src.Product.ProductName))
             
            ;
            CreateMap<BatchForEditDTO,Batch>();
            CreateMap<Department,DepartmentDTO>();

              CreateMap<BatchIngredient,BatchIngredientDTO>().ForMember(des=>des.IngredientName,opt=>opt.
              MapFrom(src=>src.Ingredient.IngredientName))
              .ForMember(des=>des.IngredientCode,opt=>opt.
              MapFrom(src=>src.Ingredient.IngredientCode));

              
              CreateMap<BatchIngredientDTO,BatchIngredient>().ForMember(des=>des.Ingredient,opt=>opt.Ignore());

             CreateMap<Barcode,BarcodeForViewDTO>().
             ForMember(des=>des.ProductName,opt=>opt.MapFrom(src=>src.product.ProductName));;

             CreateMap<BarcodeForViewDTO,Barcode>();

             CreateMap<Notification,NotificationDTO>().ForMember(des=>des.TakenDisplayTitle,
             opt=>opt.MapFrom(src=>mapMotificationTakenDisplayTitle(src)));


              CreateMap<NotificationDTO,Notification>();


             CreateMap<BatchTask,BatchTaskInfoDTO>(); 

              CreateMap<BatchTask,UserRunningTaskDTO>()
             .ForMember(des=>des.TaskTypeTitle,opt=>opt.MapFrom(src=>src.TaskType.Title))
             .ForMember(des=>des.BatchNO,opt=>opt.MapFrom(src=>src.Batch.BatchNO))
             .ForMember(des=>des.BatchSize,opt=>opt.MapFrom(src=>src.Batch.BatchSize))
             .ForMember(des=>des.TubeWeight,opt=>opt.MapFrom(src=>src.Batch.TubeWeight))
             .ForMember(des=>des.TubesCount,opt=>opt.MapFrom(src=>src.Batch.TubesCount  ))
             .ForMember(des=>des.ProductName,opt=>opt.MapFrom(src=>src.Batch.Product.ProductName));
              
             CreateMap<Ingredient,IngredientDTO>();
             CreateMap<IngredientDTO,Ingredient>();
             CreateMap<Product,ProductDTO>().
             ForMember(des=>des.ProductTypeTitle,opt=>opt.MapFrom(src=>src.ProductType.Title));
             CreateMap<ProductDTO,Product>();

              CreateMap<ProductIngredientDTO,ProductIngredient>()
              .ForMember(des=>des.Ingredient,opt=>opt.Ignore())
               .ForMember(des=>des.Product,opt=>opt.Ignore())
              ;
              CreateMap<ProductIngredient,ProductIngredientDTO>()
              .ForMember(des=>des.IngredientTitle,opt=>opt.MapFrom(src=>src.Ingredient.IngredientName));;



               CreateMap<Message,MessageDTO>()
              .ForMember(des=>des.BatchNO,opt=>opt.MapFrom(src=>src.BatchTask.Batch.BatchNO))
               .ForMember(des=>des.UserName,opt=>opt.MapFrom(src=>src.User.UserName));


                CreateMap<MessageDTO,Message>()
            .ForMember(des=>des.BatchTask,opt=>opt.Ignore())
            .ForMember(des=>des.User,opt=>opt.Ignore())
            .ForMember(des=>des.DestinationUser,opt=>opt.Ignore());;
              

        }

        private string mapMotificationTakenDisplayTitle(Notification notification)
        {
                string result=string.Empty;
                if (notification.AssignedByUserId!=null)
                {

                    result = string.Format("Taken By {0}",notification.AssignedByUser.UserName);
                }
                else
                {
                    result="Available";
                }
                return result;

        }
    }
}