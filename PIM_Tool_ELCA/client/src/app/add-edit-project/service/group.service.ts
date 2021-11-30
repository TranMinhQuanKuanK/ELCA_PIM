import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GroupService {

  constructor(private http: HttpClient) { }
  getGroupList(): Observable<number[]> {
    return this.http
      .get<number[]>(
        'https://localhost:44393/Project/GroupListAPI',
        { responseType: "json" }
      );
  }
}
