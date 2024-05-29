import { Component, OnInit } from '@angular/core';
import { PresenceService } from '../_services/presence.service';
import { DialogconfirmService } from '../_services/dialogconfirm.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  

   
  constructor(private dialogservice:DialogconfirmService,private router:Router)
  {

  }
  ngOnInit(): void
  {
       
  }

  test()
  {

    
  }
  testprint()
  {
    this.router.navigate(['stickers']);
  }

}
