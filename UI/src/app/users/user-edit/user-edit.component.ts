import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subject, Subscription, takeUntil } from 'rxjs';
import { Department } from 'src/app/_models/department';
import { User } from 'src/app/_models/user';
import { DataService } from 'src/app/_services/data.service';
import { DepartmentsService } from 'src/app/_services/departments.service';
 
import { UsersService } from 'src/app/_services/users.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
 
})
export class UserEditComponent implements OnInit
{


   departments?:Department[];
 //departments!:Observable<Department[]>;
  private unsubscribe = new Subject<void>();

  title!:string;

  form :FormGroup;

  user:User;

  iseditmode:boolean;
  
  // fetched userName for editing....
  //private userName='';
  username?:string;
  constructor( private activatedRoute:ActivatedRoute,private departmentservice:DepartmentsService,
    public dataservice: DataService,
  private router:Router,private userservice:UsersService)
  {
   
  }
  

  

  ngOnInit(): void
  {
    // this.getDepartments();
      this.dataservice.departments$.pipe(takeUntil(this.unsubscribe)).subscribe(data=>
        {
          this.departments=data;
        })
      {

      };

     this.form =new FormGroup
     (
      {
         userName:new FormControl(''),
         email:new FormControl(''),
         password:new FormControl(''),
         departmentId:new FormControl('')
      }
     );

     // load user....
      this.loaduser();
  }


    loaduser()
    {
      this.username = this.activatedRoute.snapshot.params['id'];

      if (this.username)
      {
          this.iseditmode=true;
          // edit...
           this.userservice.getUser(this.username).subscribe
           (
            result=>
            {
              this.user = result;
              this.title="Editing.."+this.user.userName;

              // update the form from the result....
              this.form.patchValue(this.user);
            }
            ,
            error=>
            {console.log(error);}
            )
      }
      else
      {
          this.iseditmode=false;
          // add...
          this.title="Add a New User...";
         // this.form.patchValue(this.user);
        //  this.user=Object.assign(<User>{}, this.form.value);

        this.user={} as User;
        this.form.patchValue(this.user);
          
      }
    }


    onSubmit()
    {
     
      // var _user = (this.username)? this.user:<User>{ };
      // var _user =this.user;
      
       this.user.email  = this.form.get("email")?.value;
       this.user.password = this.form.get("password")?.value;
       this.user.departmentId= +this.form.get("departmentId")?.value;
       this.user.userName = this.form.get("userName")?.value;

       if (this.username)
       {
          // update...
           this.userservice.updateuser(this.username,this.user).subscribe
           (
            result=>
            {
              console.log("User " + this.user.userName + " has been updated.");
              this.router.navigate(['/users']);
            }
            ,
            error=>
            {
              console.log(error);
            }
            
           )
       }
       else
       {
          // add...
          this.userservice.adduser(this.user).subscribe
           (
            result=>
            {
              console.log("User " + this.user.userName + " has been Added.");
              this.router.navigate(['/users']);
            }
            ,
            error=>
            {
              console.log(error);
            }
            
           )
       }

    }
    
/*
   getDepartments()
   {
      this.departmentservice.getDepartments().subscribe(res=>
      {
          this.departments=res;
      });
     
   }
   */
  

     ngOnDestroy() {
    this.unsubscribe.next();
    this.unsubscribe.complete();
}


}
