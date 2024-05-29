using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public static class DepartmentRoles
    {
        public static Dictionary<int,List<string>> department_roles = new Dictionary<int, List<string>>()
        {
                 
                    {
                            (int)Enumerations.DepartmentsEnum.Warehouse,
                            new List<string>{Helpers.UserRoles.Warehouse_CheckRooms,Helpers.UserRoles.Warehouse_RawMaterials}
                    }
                    ,
                    {
                           (int)Enumerations.DepartmentsEnum.QA,
                            new List<string>{Helpers.UserRoles.QA_RawMaterials,Helpers.UserRoles.QA_CheckEquipements
                            ,Helpers.UserRoles.QA_Sampling}
                    }
                    ,
                     {
                           (int)Enumerations.DepartmentsEnum.Production,
                            new List<string>{Helpers.UserRoles.Production_CheckEquipements,Helpers.UserRoles.Production_Manager
                            ,Helpers.UserRoles.Production_Manufacturing}
                    }
                    ,
                      {
                           (int)Enumerations.DepartmentsEnum.Management,
                            new List<string>{Helpers.UserRoles.Manager
                            }
                    }
                     ,
                     {
                           (int)Enumerations.DepartmentsEnum.Filling,
                            new List<string>{Helpers.UserRoles.Filling_Cartooning,Helpers.UserRoles.Filling_CheckEquipements
                            ,Helpers.UserRoles.Filling_FillingTubes,Helpers.UserRoles.Filling_Packaging}
                    }
                     ,
                     {
                           (int)Enumerations.DepartmentsEnum.Accounting,
                            new List<string>{Helpers.UserRoles.Accountant}
                    }

        }
        ;



        
    }
}