import { Injectable } from '@angular/core';
 
import { BehaviorSubject, Observable, Subject, delay } from 'rxjs';

@Injectable({
  providedIn: 'root'
})



export class LoaderService
 {

  public isLoading =new Subject<boolean>();
  isLoading$ = this.isLoading.asObservable();

  constructor() { }
}
