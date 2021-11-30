export class MemberResponseModel {
  public Id: number;
  public Visa: string;
  public FirstName: string;
  public LastName: string;
  constructor(Id: number,
    Visa: string,
    FirstName: string,
    LastName: string,
  ) {
    this.Id = Id;
    this.Visa = Visa;
    this.FirstName = FirstName;
    this.LastName = LastName;
  }

}
