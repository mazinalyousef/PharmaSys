import { Component, OnInit } from '@angular/core';
import { userTask } from '../_models/userTask';
import { BatchtaskService } from '../_services/batchtask.service';
import { UsersService } from '../_services/users.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-user-tasks',
  templateUrl: './user-tasks.component.html',
  styleUrls: ['./user-tasks.component.css']
})
export class UserTasksComponent implements OnInit
 {

  userTasks : userTask[];

  loggedUserId:string;

  constructor( private taskservice :BatchtaskService,
    private userservice :UsersService)
  {
      
  } 

  ngOnInit(): void
  {

       
    // get the current logged user Id
    this.userservice.loggedUser$.pipe(
      take(1) 
     ).subscribe
     (
       res=>
       {
         console.log(res.username +" from user tasks component");
         this.loggedUserId=res.id;


       }
     );

     if (this.loggedUserId)
     {
        // get the user current tasks
        this.taskservice.getallRunningForUser(this.loggedUserId).subscribe
        (
          res=>
          {
              this.userTasks=res;
              console.log(this.userTasks);
          },error=>
          {
            console.log(error);
          }
        )
     }
      
  }

}
