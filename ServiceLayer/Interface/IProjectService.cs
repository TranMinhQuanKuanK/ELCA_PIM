using ContractLayer;
using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IProjectService
    {
        ProjectListPageContractResult GetProjectList(SearchProjectRequestModel request);
        AddEditProjectModel GetProjectByID(long id);
        bool CheckProjectNumberExist(short projectNumber);
        bool ValidateProjectModelAndUpdate(AddEditProjectModel project);
        bool ValidateAndCreateNewProject(AddEditProjectModel project);
        bool DeleteProject(IList<DeleteProjectRequestModel> projectList);
    }
}
