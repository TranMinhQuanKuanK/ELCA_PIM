export class ProjectModel {
  constructor(
    public Id: number,
    public GroupId: number,
    public ProjectNumber: number,
    public Name: string,
    public Customer: string,
    public Status: string,
    public StartDate: string,
    public EndDate: string,
    public MemberString: string,
    public MembersList: string[],
    public Version: number
  ) { }
}
