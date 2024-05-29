import { Component, OnInit } from '@angular/core';
import { UsersService } from '../_services/users.service';
import { changepassword } from '../_models/changepassword';
import { take } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/user';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit

{

   changePasswordModel :changepassword={userId:'',userName:'',currentPassword:'',newPassword:''};
   UserId : string;
   user:User;
   username?:string;

  constructor(
    private activatedRoute:ActivatedRoute,private router:Router,
    private userservice:UsersService,private toastr :ToastrService)
   {

   }
  ngOnInit(): void
  
  {

    this.loaduser();

    /*
    this.userservice.loggedUser$.pipe(
      take(1) 
     ).subscribe(
       res=> {
       this.loggedUserId=res.id;
       }
     )
     */
  }


  loaduser()
  {
    this.username = this.activatedRoute.snapshot.params['id'];

    if (this.username)
    {
        
         
         this.userservice.getUser(this.username).subscribe
         (
          result=>
          {
            this.user = result; 
            this.UserId = result.id;  
          }
          ,
          error=>
          {console.log(error);}
          )
    }
  
  }

  changePassword()
  {

    this.changePasswordModel.userId = this.UserId;
this.userservice.changePassword(this.changePasswordModel).subscribe(
  res=>
  {
     this.toastr.info("Password Changed Successfully","");
  },
  error=>
  {
    this.toastr.error(error,"")
  }
)
  }

}
