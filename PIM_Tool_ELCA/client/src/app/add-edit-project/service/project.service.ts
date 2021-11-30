import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProjectResponseModel } from '../model/project-response-model';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor(private http: HttpClient) { }
  getEditForm(id: number): Observable<ProjectResponseModel> {
    let searchParams = new HttpParams();
    searchParams = searchParams.append('id', id);
    return this.http
      .get<ProjectResponseModel>(
        'https://localhost:44393/Project/EditProjectAPI',
        { params: searchParams, responseType: "json" }
      );
  }

}
