import { ProjectModel } from "./project-model";

export class ProjectResponseModel {
  public projectModel: ProjectModel;
  public hasError: boolean;
  public errorList: string[];
  constructor(projectModel: ProjectModel,
    hasError: boolean,
    errorList: string[],
  ) {
    this.projectModel = projectModel;
    this.hasError = hasError;
    this.errorList = errorList;
  }
}
