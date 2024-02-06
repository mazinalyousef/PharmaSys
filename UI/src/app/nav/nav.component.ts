import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UsersService } from '../_services/users.service';
import { Observable, take, takeLast } from 'rxjs';
import { AuthenticatedResponse } from '../_models/AuthenticatedResponse';
import { PresenceService } from '../_services/presence.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})


export class NavComponent implements OnInit {


  notificationsCount$:Observable<number>;
 // notificationsIsHidden:boolean;
  notificationCounter :number;
  //loggedin : boolean;
  currentUser$:Observable<AuthenticatedResponse>;

  constructor(public userservice:UsersService,
    public presenceservice:PresenceService,
     private router: Router)
  {
    //this.notificationsIsHidden=false;
    this.notificationCounter=0;
  }


    ngOnInit(): void 
    
    {

     //this.router.navigate(['home'])
   
   //  this.getCurrentUser();

     this.currentUser$ = this.userservice.loggedUser$;
     if (this.currentUser$)
     {
      this.router.navigate(['login']);
      // this.router.navigate(['home']);
   
     }
     else
     {
       this.router.navigate(['login']);
     }


     this.notificationsCount$ = this.presenceservice.NewnotificationsCount$.pipe(takeLast(1));
     

    }

    getCurrentUser()
    {
      this.userservice.loggedUser$.subscribe(
        res=>
        {
         // this.loggedin = !!res;
         console.log(res);
        }
        ,err=>
        {
         console.log(err);
        }
      )
    }

    onlogout()
    {
      this.userservice.performlogout();
      this.router.navigate(['login']);
    }

    showNotifications()
    {
      this.router.navigate(['notifications']);
    }

}
