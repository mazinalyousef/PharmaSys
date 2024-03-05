import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { confirmDialogData } from '../_models/confirmDialogData';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DialogconfirmService {

  constructor(private dialog:MatDialog)
   {


    }

  confirmDialog(data:confirmDialogData) :Observable<boolean>
  {
  return  this.dialog.open(ConfirmDialogComponent,{data,width:'400px',disableClose:true}).afterClosed();
  }
}
