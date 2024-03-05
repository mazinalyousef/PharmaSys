import { Component, Input, OnInit } from '@angular/core';
import { message } from '../_models/message';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit

{
  @Input() msg :message;
  ngOnInit(): void 
  {
     
  }

}
