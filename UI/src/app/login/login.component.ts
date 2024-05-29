import { Component, OnInit } from '@angular/core';
import { LoginModel } from '../_models/login';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { AuthenticatedResponse } from '../_models/AuthenticatedResponse';
import { environment } from 'src/environments/environment';
import { UsersService } from '../_services/users.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit 
{

  baseUrl = environment.apiUrl;
  invalidLogin: boolean;
  loginuser : LoginModel={username:'',password:''}
  
   
  constructor(private router: Router,
     private userservice: UsersService,
     private toastr:ToastrService
    )
  {

  }
  ngOnInit(): void
  
  {
     
  }


  login()
  {
   this.userservice.performlogin(this.loginuser).subscribe(
    res=>
    {
       
      this.router.navigate(['home']);
    },
    err=>
    {
      console.log(err);
      this.toastr.error(err,"");
    }
   )
  }
 

}
