import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, map } from 'rxjs';
import { UsersService } from '../_services/users.service';
import { UserRoles } from '../_enums/userRoles';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate
 {
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
            if (user.roles.includes(UserRoles.Manager)||user.roles.includes(UserRoles.Developer))
            {
              return true;
            }
            
          }
          console.log('UnAuthorized....');
           this.tostr.error('UnAuthorized','');
          return false;
        }
      )
    )
  }
  
}
