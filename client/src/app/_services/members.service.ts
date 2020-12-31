import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Member } from '../_models/member';


@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;

  constructor(private injectedHttp: HttpClient) { }

  getMembers() {
    return this.injectedHttp.get<Member[]>(this.baseUrl + 'users')
  }

  getMember(username: string) {
    return this.injectedHttp.get<Member>(this.baseUrl + 'users/' + username);
  }
}
