import { Component, OnInit } from '@angular/core';
import { PresenceService } from '../_services/presence.service';
import { DialogconfirmService } from '../_services/dialogconfirm.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  

   
  constructor(private dialogservice:DialogconfirmService)
  {

  }
  ngOnInit(): void
  {
       
  }

  test()
  {

    
  }

}
