export class ProjecListResponseModel {
  public ProjectList: any[];
  public ResultCount: number;
  public PageIndex: number;
  public PageSize: number;
  constructor(ProjectList: any[],
    ResultCount: number,
    PageSize: number,
    PageIndex: number,
  ) {
    this.ProjectList = ProjectList;
    this.ResultCount = ResultCount;
    this.PageSize = PageSize;
    this.PageIndex = PageIndex;
  }
}
