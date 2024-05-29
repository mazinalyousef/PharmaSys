import { Component, OnInit, Inject, inject  } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { confirmDialogData } from '../_models/confirmDialogData';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent implements OnInit

{

  title: string;
  message: string;


  constructor(@Inject(MAT_DIALOG_DATA) public data :confirmDialogData)
  
  {

  }
  ngOnInit(): void {
    
  }


   
}
