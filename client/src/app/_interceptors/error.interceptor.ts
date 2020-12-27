import { ToastrService } from 'ngx-toastr';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private injRouter: Router, private injToastr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if(error) {
          switch(error.status) {
            case 400:
              if (error.error.errors){
                const modalStateErrors = [];
                for(let theErrorProperty in error.error.errors) {
                  if(error.error.errors[theErrorProperty]) {
                    modalStateErrors.push(error.error.errors[theErrorProperty])
                  }
                }
                throw modalStateErrors;
              } else {
                this.injToastr.error("Bad Request Error", error.status)
              }
              break;
            case 401:
              this.injToastr.error("Unauthorized", error.status)
              break;
            case 404: 
              this.injRouter.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = {state: {error: error.error}}
              this.injRouter.navigateByUrl('/server-error', navigationExtras)
              break;
            default:
              this.injToastr.error("This is the default message of the interceptor switch method in error.interceptor.ts. Something went wrong!")
              console.log(error)
              break;
             
          }
        }
        return throwError(error);
      })
    )
  }
}
