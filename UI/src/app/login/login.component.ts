import { Component, OnInit } from '@angular/core';
import { LoginModel } from '../_models/login';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { AuthenticatedResponse } from '../_models/AuthenticatedResponse';
import { environment } from 'src/environments/environment';
import { UsersService } from '../_services/users.service';

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
      console.log(res +" from login component");
      this.router.navigate(['home']);
    },
    err=>
    {
      console.log(err);
    }
   )
  }
/*
    login = ( form: NgForm) =>
     {
    if (form.valid) {
      this.http.post<AuthenticatedResponse>(this.baseUrl+'Users/login', this.loginuser, {
        headers: new HttpHeaders({ "Content-Type": "application/json"})
      })
      .subscribe({
        next: (response: AuthenticatedResponse) => {
          
          const token = response.token;
          localStorage.setItem("jwt", token); 
           console.log(token);
          this.invalidLogin = false; 

          // load application .. navigate nav component...
          this.router.navigate(['home'])
        },
        error: (err: HttpErrorResponse) => this.invalidLogin = true
      })
    }
  }
  */

}
