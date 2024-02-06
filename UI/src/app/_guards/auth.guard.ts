import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, map } from 'rxjs';
import { UsersService } from '../_services/users.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private userService : UsersService)
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
          return false;
        }
      )
    )
  }
  
}
