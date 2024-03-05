import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { User } from 'src/app/_models/user';
import { UsersService } from 'src/app/_services/users.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})


export class UserListComponent implements OnInit

{

     displayedColumns = ['username', 'email', 'department','actions']; 
     users! : User[];
     MatdataSource :any;
     @ViewChild(MatPaginator) paginator: MatPaginator;
     
     constructor(private userservice:UsersService)
     {
        
     }
     ngOnInit(): void 
     {
     this.loadusers();
     }

     loadusers()
     {
        this.userservice.getUsers().subscribe(users=>
        {
          this.users=users;
          this.MatdataSource = new MatTableDataSource<any>(this.users);
          this.MatdataSource.paginator = this.paginator;
        }
        )
     }
  

}
