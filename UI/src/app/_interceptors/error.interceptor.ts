import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router :Router,private snackbar:MatSnackBar,private toastr:ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe
    (catchError(
      error=>
      {
        if (error)
        {
          switch( error.status)
          {
           case 400:
            if (error.error.errors)
            {
              const modalStateErrors=[];
              for (const key in error.error.errors)
              {
                if (error.error.errors[key])
                {
                  modalStateErrors.push(error.error.errors[key])
                }
              }
              throw modalStateErrors;
            }
            else 
            {
              //toaster...
              // this.toastr.error(error.statusText,error.status);

              /*
              this.snackbar.open(error.status, 'close', {
                duration: 3000
              });
              */
              this.toastr.error(error.status,'');
            }
           
            break;


            case 401:
              /*
              this.snackbar.open(error.status, 'close', {
                duration: 3000
              });
              */
                this.toastr.error(error.status,'');
               break;
               case 404:
               // this.router.navigateByUrl('/not-found');
                this.toastr.error('Not Found Error','');
                break;

                case 500:
                  const navigationExctras:NavigationExtras={state:{error:error.error}};
                  this.router.navigateByUrl('/server-error',navigationExctras);
                  break;

            default :
                /*
            this.snackbar.open('An Error Ooccured', 'close', {
              duration: 3000
            });
                */  
            this.toastr.error('An Error Occured','');
             console.log(error);
            break;  
          }
        }
        return throwError(error);
      }
    ))
  }
}
