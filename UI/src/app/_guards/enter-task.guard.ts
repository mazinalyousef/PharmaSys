import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, map, take } from 'rxjs';
import { UsersService } from '../_services/users.service';
import { ToastrService } from 'ngx-toastr';
import { BatchtaskService } from '../_services/batchtask.service';

@Injectable({
  providedIn: 'root'
})
export class EnterTaskGuard implements CanActivate

{

  constructor(private userService : UsersService
    ,private tostr:ToastrService,private taskService:BatchtaskService,
    private router: Router
    )
  {
  }


  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean > {

      let loggedUserId:string='';
      let batchUserId:string='';
      this.userService.loggedUser$.pipe(
        take(1) 
          ).subscribe(
        res=> {
        loggedUserId=res.id;
        }
        );
        const TaskId = route.params['id'];

      return this.taskService.getBatchTask(TaskId).pipe
      (
        map
        (
          res=>
          {
            if (res)
            {
              const batchuserId = res.userId;
              if (batchuserId)
              {
                   
                  if (batchuserId===loggedUserId)
                  {
                    return true;
                  }
                  else
                  {
                    this.tostr.error('Not Allowed not  equal users....','');
                    return false;
                  }
              }
              else
              {
                this.tostr.error('Not Allowed no user....','');
                return false;
              }
            }
            console.log('Not Allowed....');
            this.tostr.error('Not Allowed last.......','');
           return false;
          }
        )
      )
  }
  
}
