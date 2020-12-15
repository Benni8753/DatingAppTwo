import { TitleCasePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  allMyUsers: any;

  // the ctor injects the http behaviour which is needed to request data from the api
  constructor(private http: HttpClient) {
    
  }
  //this method is called when angular starts. It requests the users from the api and it sets allMyUsers
  //equal to the response. Http get is an observable you need to subscribe to do something with it, otherwise
  //, its lazy
  ngOnInit() {
    this.getUsers();
  }z

  getUsers() {
    this.http.get('https://localhost:5001/api/users').subscribe(response => {
      this.allMyUsers = response;
    }, error => {
      console.log(error)
    })
  }
}
