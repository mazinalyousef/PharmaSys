import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { UsersService } from '../_services/users.service';
import { AuthenticatedResponse } from '../_models/AuthenticatedResponse';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private userservice:UsersService) 
  {

  }


  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> 
  {
    let  currentuser : AuthenticatedResponse;
    this.userservice.loggedUser$.pipe(take(1)).subscribe(res=>currentuser=res);
    if (currentuser)
    {
       request  = request.clone(
        {
          setHeaders:{
            Authorization: `Bearer ${currentuser.token}`
          }
        }
       ) 
    }
    else
    {

    }
    return next.handle(request);
  }
}
