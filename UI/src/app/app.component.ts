import { Component, OnInit } from '@angular/core';
import { DepartmentsService } from './_services/departments.service';
import { Department } from './_models/department';
 
import { A11yModule } from '@angular/cdk/a11y';
import { map } from 'rxjs';
import { DataService } from './_services/data.service';
import { AuthenticatedResponse } from './_models/AuthenticatedResponse';
import { UsersService } from './_services/users.service';
import { PresenceService } from './_services/presence.service';
import { LoaderService } from './_services/loader.service';

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
        private dataservice:DataService,
        private presenceService:PresenceService,
        public loaderService:LoaderService)
    {
     
    }
    ngOnInit(): void
    {
      this.setCurrentUser();
      this.getdepartments();
   
     
    }

    setCurrentUser()
    {
       const user : AuthenticatedResponse =JSON.parse(sessionStorage.getItem('user'));
      
       if (user)
       {
        console.log(user.username+" from set current user--app component init") 
          console.log("calling userservice.setCurrentUser from app component for user "+user.id)
          this.userservice.setCurrentUser(user);
       }
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
