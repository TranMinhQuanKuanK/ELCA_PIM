import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, tap } from 'rxjs/operators';

import { Project } from './model/project-model';
import { ProjecListResponseModel } from './model/project-list-response-model';
import { DataSourceRequestState } from '@progress/kendo-data-query';
import { GridDataResult, PageChangeEvent, SelectableSettings } from '@progress/kendo-angular-grid';
import { Observable, Subscription } from 'rxjs';
import { GridDataService } from '../grid-data.service';

@Component({
  selector: 'proj-list-project-table',
  templateUrl: './project-table.component.html',
  styleUrls: ['./project-table.component.css']
})
export class ProjectTableComponent implements OnInit {

  public pageSize: number = 0;
  public skip: number = 0;
  public data: GridDataResult = {
    data: [],
    total: 0
  };
  public selectedId: number[];
  public selectableSettings: SelectableSettings = {
    checkboxOnly: true,
    mode: "multiple",
    drag: true,
  };;

  private subscription: Subscription;

  constructor(private http: HttpClient, private dataService: GridDataService) {

  }

  ngOnInit(): void {
    this.subscription = this.dataService.gridDataChanged.subscribe(
      () => {
        this.pageSize = this.dataService.getPageSize();
        this.skip = this.dataService.getSkip();
        this.data = this.dataService.getGridData();
      }
    );
    this.dataService.initDataGrid();


    let searchParams = new HttpParams();
    searchParams = searchParams.append('SearchStatus', "");
    searchParams = searchParams.append('SearchTerm', "");
    searchParams = searchParams.append('PageIndex', 1);
    searchParams = searchParams.append('PageSize', 5);

  }
  onPageChange(event: PageChangeEvent) {
    this.dataService.onPageChange(event);
  }
  ngOnDestroy() {

  }
}
