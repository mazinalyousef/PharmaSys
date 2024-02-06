using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOS;
using API.Entities;
using AutoMapper;

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

              CreateMap<BatchIngredient,BatchIngredientDTO>().ForMember(des=>des.IngredientName,opt=>opt.MapFrom(src=>src.Ingredient.IngredientName));
              CreateMap<BatchIngredientDTO,BatchIngredient>().ForMember(des=>des.Ingredient,opt=>opt.Ignore());

             CreateMap<Barcode,BarcodeForViewDTO>().ForMember(des=>des.ProductName,opt=>opt.MapFrom(src=>src.product.ProductName));;

             CreateMap<Notification,NotificationDTO>();
              CreateMap<NotificationDTO,Notification>();


             CreateMap<BatchTask,BatchTaskInfoDTO>(); 
        }
    }
}