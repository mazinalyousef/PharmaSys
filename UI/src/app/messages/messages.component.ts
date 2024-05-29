import { Component, OnInit } from '@angular/core';
import { UsersService } from '../_services/users.service';
import { PresenceService } from '../_services/presence.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit
{

  constructor(public presenseService : PresenceService, private userservice:UsersService )
  {
    
  }
  ngOnInit(): void
  {

  }
}
