import { ToastrService } from 'ngx-toastr';
import { AccountService } from './../_services/account.service';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private injAccService: AccountService, private injToastrService: ToastrService) {}

  canActivate(): Observable<boolean> {
    return this.injAccService.currentUser$.pipe(
      map(user => {
        if(user) return true;
        this.injToastrService.error('You shall not pass!')
        return false;
      })
    )
  }
  
}
