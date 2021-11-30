export class Project {
  public id: number;
  public projectNumber: number;
  public name: string;
  public customer: string;
  public status: string;
  public startDate: string;
  public version: number;

  constructor(id: number,
    projectNumber: number,
    name: string,
    customer: string,
    status: string,
    startDate: string,
    version: number
  ) {
    this.id = id;
    this.projectNumber = projectNumber;
    this.name = name;
    this.customer = customer;
    this.status = status;
    this.startDate = startDate;
    this.version = version;
  }
}
