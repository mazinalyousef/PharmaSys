import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, delay, finalize } from 'rxjs';
import { LoaderService } from '../_services/loader.service';

@Injectable()
export class BusyInterceptor implements HttpInterceptor {

  constructor(private loaderService:LoaderService)
   {

   }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>>
   {

    this.loaderService.isloading.next(true);
    return next.handle(request).pipe(
      
      finalize(
        ()=>{
          this.loaderService.isloading.next(false);
        }
      )
    );
  }
}
