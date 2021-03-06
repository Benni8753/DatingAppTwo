import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Member } from '../_models/member';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];

  constructor(private injectedHttp: HttpClient) { }

  getMembers() {
    if(this.members.length > 0) return of(this.members);
    return this.injectedHttp.get<Member[]>(this.baseUrl + 'users').pipe(
      map(members => {
        this.members = members;
        return members;
      })
    )
  }

  getMember(username: string) {
    const member = this.members.find(x => x.userName === username);
    if(member !== undefined) return of(member);
    return this.injectedHttp.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member) {
    return this.injectedHttp.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    )
  }

  setMainPhoto(photoId: number) {
    return this.injectedHttp.put(this.baseUrl + 'users/set-main-photo/' + photoId, {})
  }

  deletePhoto(photoId: number) {
    return this.injectedHttp.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }
}
