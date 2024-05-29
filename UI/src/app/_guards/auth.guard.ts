import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, map } from 'rxjs';
import { UsersService } from '../_services/users.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private userService : UsersService
    ,private tostr:ToastrService
    )
  {

  }
  canActivate(): Observable<boolean> {
    return this.userService.loggedUser$.pipe
    (
      map
      (
        user=>
        {
          if (user)
          {
            return true
          }
          console.log('Unauthenticated....');
           this.tostr.error('UnAuthenticated','');
          return false;
        }
      )
    )
  }
  
}
