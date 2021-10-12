using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.Interface
{
    public interface IProjectRepo
    {
        ProjectListPageDomainResult GetProjectList(SearchProjectRequest request);
        Project GetProjectById(long id);
        Project GetProjectByProjectNumber(short projNumber);
        void UpdateProject(Project project);
        void CreateNewProject(Project project);
        void DeleteProject(IDictionary<long, int> projectIDDictionary);
    }
}
