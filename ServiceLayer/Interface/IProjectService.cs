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
        AddEditProjectModel GetProjectById(long id);
        bool CheckProjectNumberExist(short projectNumber);
        void Update(AddEditProjectModel project);
        void Create(AddEditProjectModel project);
        void DeleteProject(IList<DeleteProjectRequestModel> projectList);
    }
}
