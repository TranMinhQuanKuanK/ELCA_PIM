import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class ProjectListComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
