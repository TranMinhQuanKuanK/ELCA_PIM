import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MemberResponseModel } from '../model/member-response-model';

@Injectable({
  providedIn: 'root'
})
export class MemberService {

  constructor(private http: HttpClient) { }
  getMemberList(): Observable<MemberResponseModel[]> {
    let response: MemberResponseModel[] = [];
    return this.http
      .get<MemberResponseModel[]>(
        'https://localhost:44393/Project/VisaListAPI',
        { responseType: "json" }
      );

  }

}
