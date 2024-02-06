import { Component, OnInit } from '@angular/core';
import { MatSelect, MatSelectChange } from '@angular/material/select';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { UserRoles } from 'src/app/_enums/userRoles';
import { UsersService } from 'src/app/_services/users.service';

@Component({
  selector: 'app-roles-edit',
  templateUrl: './roles-edit.component.html',
  styleUrls: ['./roles-edit.component.css']
})



export class RolesEditComponent implements OnInit

{

   displayedColumns = ['userroleID','actions']; 
   title:string;
   Allroles : string[];
   userroles : string[];
   rolesenum = UserRoles;
   username :string;
   userrolesdataSource :any;
   selectedRoleItem :string;

   constructor(private activatedRoute:ActivatedRoute,
    private  userservice: UsersService, private router:Router)
   {
   
   }
  ngOnInit(): void 
  {
     
     this.Allroles =  Object.keys(this.rolesenum);
     this.username = this.activatedRoute.snapshot.params['userName'];
     this.title = "Editing  " + this.username + "  Roles";
     this.loadroles();
    
  }



  selectedValue(event: MatSelectChange) {
    this.selectedRoleItem =  event.value;
   // console.log(this.selectedItem);
  }


  addRole()
  {
   // var e = document.getElementById('RoleName').value;

   if (this.selectedRoleItem)
   { if (!this.userroles.includes(this.selectedRoleItem))
    {
      
      this.userroles.push(this.selectedRoleItem);
      this.userrolesdataSource = new MatTableDataSource<any>(this.userroles);
    }
  }
   
  }


  save()
  {    
    // save roles to database ... 
    this.userservice.updateUserRoles(this.username,this.userroles).subscribe
    (
      res=>
      {console.log(res);
      }
      ,
      error=>
      {
        console.log(error);
      }
    )
    // navigate back to users list...
     this.router.navigate(['/users']);
  }

  deleteRole(roleid:string)
  {
 
    const index: number = this.userroles.indexOf(roleid);
    if (index !== -1) {
        this.userroles.splice(index, 1);
        this.userrolesdataSource = new MatTableDataSource<any>(this.userroles);
    }    
      
  }


  loadroles()
  {
    this.userservice.getUserRoles( this.username).subscribe(
      res=>
      {
        this.userroles = res;
        this.userrolesdataSource = new MatTableDataSource<any>(this.userroles);
        console.log(res);
      },
      error=>
      {
        console.log(error);
      }
    )
  }

}
