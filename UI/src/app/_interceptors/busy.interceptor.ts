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


    const exurl='Tasks/WaitForTaskTimer';
    if (request.url.search(exurl)===-1)
    {
        // this.loaderService.isLoading.next(true);
        this.loaderService.busy();
    }
   
    return next.handle(request).pipe(
      delay(10),
      finalize(
        ()=>{
          //this.loaderService.isLoading.next(false);
          this.loaderService.idle();
        }
      )
    );
  }
}
