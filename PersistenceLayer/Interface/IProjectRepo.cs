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
        ProjectListPageDomainResult GetProjectList(string searchTerm, string searchStatus, int pageIndex, int pageSize);
        Project GetProjectByID(long id);
        Project GetProjectByProjectNumber(short projNumber);
        void UpdateProject(Project project);
        void CreateNewProject(Project project);
        void DeleteProject(IDictionary<long, int> projectIDDictionary);
    }
}
