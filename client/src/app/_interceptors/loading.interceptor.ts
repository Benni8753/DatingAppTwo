import { BusyService } from './../_services/busy.service';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, pipe } from 'rxjs';
import { delay, finalize } from 'rxjs/operators';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private injBusyService: BusyService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.injBusyService.busy();
    return next.handle(request).pipe(
      delay(1000),
      finalize(() => {
        this.injBusyService.idle();
      })
    );
  }
}
