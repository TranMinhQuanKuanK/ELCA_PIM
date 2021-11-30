import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { Observable, Subject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ProjecListResponseModel } from './project-table/model/project-list-response-model';
import { Project } from './project-table/model/project-model';

@Injectable({
  providedIn: 'root'
})
export class GridDataService {
  public ProjectListGridData: Project[] = [];
  public pageSize: number = 0;
  public skip: number = 0;
  public data: GridDataResult = {
    data: [],
    total: 0
  };
  gridDataChanged = new Subject();
  constructor(private http: HttpClient) { }

  getGridData() { return this.data }

  getPageSize() { return this.pageSize }

  getSkip() { return this.skip }

  searchAndUpdateData(searchTerm: string, searchStatus: string, pageIndex: number, pageSize: number) {
    let searchParams = new HttpParams();
    searchParams = searchParams.append('SearchStatus', searchStatus);
    searchParams = searchParams.append('SearchTerm', searchTerm);
    searchParams = searchParams.append('PageIndex', pageIndex);
    searchParams = searchParams.append('PageSize', pageSize);
    this.http
      .get<ProjecListResponseModel>(
        'https://localhost:44393/Project/SearchProjectAPI',
        { params: searchParams, responseType: "json" }
      ).subscribe(responseObject => {
        this.ProjectListGridData = responseObject.ProjectList.map(responseModel => {
          return new Project(responseModel.Id,
            responseModel.ProjectNumber,
            responseModel.Name,
            responseModel.Customer,
            responseModel.Status,
            responseModel.StartDate,
            responseModel.Version);
        });

        this.data.data = this.ProjectListGridData;
        this.data.total = responseObject.ResultCount;
        this.pageSize = responseObject.PageSize;
        this.skip = (responseObject.PageIndex - 1) * responseObject.PageSize;
        this.gridDataChanged.next();
      });

  }

  initDataGrid() {
    let searchTerm = String(localStorage.getItem("SearchTerm"));
    let searchStatus = String(localStorage.getItem("SearchStatus"));
    let pageIndex = 1;
    let pageSize = 5;
    this.searchAndUpdateData(searchTerm, searchStatus, pageIndex, pageSize);
  }

  onPageChange(event: PageChangeEvent) {
    let searchTerm = String(localStorage.getItem("SearchTerm"));
    let searchStatus = String(localStorage.getItem("SearchStatus"));
    let pageIndex = Math.floor(event.skip / event.take) + 1;
    let pageSize = event.take;
    this.searchAndUpdateData(searchTerm, searchStatus, pageIndex, pageSize);
  }
}

