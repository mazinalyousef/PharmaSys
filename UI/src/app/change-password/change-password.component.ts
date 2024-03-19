import { Component, OnInit } from '@angular/core';
import { UsersService } from '../_services/users.service';
import { changepassword } from '../_models/changepassword';
import { take } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit

{



   changePasswordModel :changepassword={userId:'',currentPassword:'',newPassword:''};
   loggedUserId : string;

  constructor(private userservice:UsersService,private toastr :ToastrService)
   {

   }
  ngOnInit(): void
  
  {
    this.userservice.loggedUser$.pipe(
      take(1) 
     ).subscribe(
       res=> {
       this.loggedUserId=res.id;
       }
     )
  }

  changePassword()
  {

    this.changePasswordModel.userId = this.loggedUserId;
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
