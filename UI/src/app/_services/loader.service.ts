import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
 
import { BehaviorSubject, Observable, Subject, delay } from 'rxjs';

@Injectable({
  providedIn: 'root'
})



export class LoaderService
 {


  busyRequestCount=0;

  public isLoading =new Subject<boolean>();
  isLoading$ = this.isLoading.asObservable();

  constructor(private spinnerService:NgxSpinnerService) 
  {
   // this.isLoading.next(false);
   
   }

   busy()
   {
    this.busyRequestCount++;
    this.spinnerService.show(undefined,{
      type:'ball-spin',
      bdColor:'rgba(0,51,102,0)',
      color:'#003366'
    });
   }
   idle()
   {
    this.busyRequestCount--;
    if (this.busyRequestCount<=0)
    {
      this.busyRequestCount=0;
      this.spinnerService.hide();
    }
   }
}
