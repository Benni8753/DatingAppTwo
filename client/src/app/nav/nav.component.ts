import { User } from './../_models/user';
import { AccountService } from './../_services/account.service';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}

  constructor(public injectedAccountService : AccountService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  login() {
    this.injectedAccountService.login(this.model).subscribe(response => {
      console.log(this.injectedAccountService.currentUser$)
      this.router.navigateByUrl('/members');
    })
  }

  logout() {
    this.injectedAccountService.logout();
    this.router.navigateByUrl('/')
  }

}
