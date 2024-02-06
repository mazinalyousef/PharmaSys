import { Component, OnInit } from '@angular/core';
import { DepartmentsService } from './_services/departments.service';
import { Department } from './_models/department';
 
import { A11yModule } from '@angular/cdk/a11y';
import { map } from 'rxjs';
import { DataService } from './_services/data.service';
import { AuthenticatedResponse } from './_models/AuthenticatedResponse';
import { UsersService } from './_services/users.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
   
})
export class AppComponent implements OnInit
 {

   // public App_departments! : Department[];
    public App_departments !: Department[];
    constructor(private userservice:UsersService,
       private departmentservice:DepartmentsService,
        private dataservice:DataService)
    {
     
    }
    ngOnInit(): void
    {
     this.getdepartments();
    // this.dataservice.filldepartments(this.App_departments);
    // this.dataservice.GlobalDeparments = this.App_departments;
     
    this.setCurrentUser();
    }

    setCurrentUser()
    {
       const user : AuthenticatedResponse =JSON.parse(localStorage.getItem('user'));
       this.userservice.setCurrentUser(user);
    }
    getdepartments()
    {
         this.departmentservice.getDepartments().subscribe(

          deps =>
          { 
          this.App_departments=deps;
          this.dataservice.filldepartments(this.App_departments);
          }
          
     )
       
    
        
    }
  
}
