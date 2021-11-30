import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { GroupsService } from '@progress/kendo-angular-grid';
import { GroupedObservable, zip } from 'rxjs';
import { MemberResponseModel } from './model/member-response-model';
import { ProjectResponseModel } from './model/project-response-model';
import { GroupService } from './service/group.service';
import { MemberService } from './service/member.service';
import { ProjectService } from './service/project.service';
interface Group {
  text: string;
  value: number;
}
interface Member {
  text: string;
  value: string;
}
interface Status {
  text: string;
  value: string;
}
@Component({
  selector: 'app-add-edit-project',
  templateUrl: './add-edit-project.component.html',
  styleUrls: ['./add-edit-project.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class AddEditProjectComponent implements OnInit {
  @ViewChild('formTemplate', { static: false }) projectForm: NgForm;
  constructor(
    private activatedroute: ActivatedRoute,
    private http: HttpClient,
    private router: Router,
    private memberService: MemberService,
    private groupService: GroupService,
    private projectService: ProjectService,
  ) { }
  public IsEdit: boolean;
  public id: number;
  public MemberListData: Array<Member> = [];
  public GroupListData: Array<Group> = [];
  public listStatus: Array<Status> = [
    { text: "New", value: "NEW" },
    { text: "Planned", value: "PLA" },
    { text: "In Progress", value: "INP" },
    { text: "Finished", value: "FIN" },
  ];
  //---Model for form---
  public Id: number;
  public GroupId: Group;
  public ProjectNumber: number;
  public Name: string;
  public Customer: string;
  public Status: Status;
  public StartDate: Date;
  public EndDate: Date;
  public MemberString: string;
  public MembersList: Member[];
  public Version: number;
  //--------------------
  private initGroup(groupList: number[]) {
    groupList.forEach((el) => {
      this.GroupListData.push({ text: `Group ${el}`, value: el });
    });
  }
  private initMember(memberResponseList: MemberResponseModel[]) {
    memberResponseList.forEach((el) => {
      this.MemberListData.push({ text: `${el.Visa}: ${el.FirstName} ${el.LastName}`, value: el.Visa });
    });
  }
  private initFormData(projectResponse: ProjectResponseModel) {
    console.log(projectResponse);
    this.Id = projectResponse.projectModel.Id;

    let group = this.GroupListData.find(element => element.value === projectResponse.projectModel.GroupId);
    this.GroupId = <Group>group;
    console.log(this.GroupId);

    this.ProjectNumber = projectResponse.projectModel.ProjectNumber;
    this.Name = projectResponse.projectModel.Name;
    this.Customer = projectResponse.projectModel.Customer;

    let status = this.listStatus.find(el => el.value === projectResponse.projectModel.Status);
    this.Status = <Status>status;
    console.log(this.Status);

    this.StartDate = new Date(parseInt(projectResponse.projectModel.StartDate.replace("/Date(", "").replace(")/", ""), 10));

    if (projectResponse.projectModel.EndDate != null) {
      console.log(projectResponse.projectModel.EndDate);
      this.EndDate = new Date(parseInt(projectResponse.projectModel.EndDate.replace("/Date(", "").replace(")/", ""), 10));
    }
    // this.MemberString = projectResponse.projectModel.MemberString;
    let members = this.MemberListData.filter(el => projectResponse.projectModel.MembersList.includes(el.value));
    this.MembersList = members;
    console.log(this.StartDate);

    this.Version = projectResponse.projectModel.Version;
  }
  ngOnInit(): void {
    this.activatedroute.data.subscribe(data => {
      this.IsEdit = data.mode === "Edit";
      if (this.IsEdit) {
        this.activatedroute.params.subscribe(
          (params: Params) => {
            this.id = +params['id'];
            let dataZip = zip(
              this.memberService.getMemberList(),
              this.groupService.getGroupList(),
              this.projectService.getEditForm(this.id));
            dataZip.subscribe(values => {
              this.initMember(values[0]);
              this.initGroup(values[1]);
              this.initFormData(values[2]);
            });
          });
      }
    });
    //call project service
  }

}
