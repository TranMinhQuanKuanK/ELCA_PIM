import { Component, OnInit } from '@angular/core';
import { GridDataService } from '../grid-data.service';
interface Status {
  text: string;
  value: string;
}
@Component({
  selector: 'proj-list-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})
export class SearchBarComponent implements OnInit {
  public searchTerm: string;
  public searchStatus: Status;
  public defaultStatus: Status = {
    text: "Select status...",
    value: ""
  }
  public listStatus: Array<Status> = [
    { text: "New", value: "NEW" },
    { text: "Planned", value: "PLA" },
    { text: "In Progress", value: "INP" },
    { text: "Finished", value: "FIN" },
  ];
  constructor(private dataService: GridDataService) { }

  ngOnInit(): void {
    this.searchTerm = String(localStorage.getItem("SearchTerm"));
    switch (String(localStorage.getItem("SearchStatus"))) {
      case "NEW":
        this.searchStatus = this.listStatus[0];
        break;
      case "PLA":
        this.searchStatus = this.listStatus[1];
        break;
      case "INP":
        this.searchStatus = this.listStatus[2];
        break;
      case "FIN":
        this.searchStatus = this.listStatus[3];
        break;
      default:
        this.searchStatus = this.defaultStatus;
    }
    // this.searchStatus = String(localStorage.getItem("SearchStatus"));
  }
  onSubmitSearch() {
    this.dataService.searchAndUpdateData(this.searchTerm, this.searchStatus.value, 1, 5);
    localStorage.setItem("SearchTerm", this.searchTerm);
    localStorage.setItem("SearchStatus", this.searchStatus.value);
  }
  onReset() {
    this.dataService.searchAndUpdateData("", "", 1, 5);
    this.searchStatus = this.defaultStatus;
    this.searchTerm = "";
    localStorage.setItem("SearchTerm", "");
    localStorage.setItem("SearchStatus", "");
  }
}
