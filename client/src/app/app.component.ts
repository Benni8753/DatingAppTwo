import { AccountService } from './_services/account.service';
import { TitleCasePipe } from '@angular/common';
import { HttpClient, JsonpClientBackend } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  allMyUsers: any;

  // the ctor injects the http behaviour which is needed to request data from the api
  constructor(private injectedAccountService: AccountService) {}
  //this method is called when angular starts. It requests the users from the api and sets allMyUsers
  //equal to the response. Http get is an observable you need to subscribe to do something with it, otherwise
  //, its lazy
  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user'));
    this.injectedAccountService.setCurrentUser(user);
  }

}
