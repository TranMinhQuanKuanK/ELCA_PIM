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
        IList<Project> GetProjectList(string searchTerm, string searchStatus);
        Project GetProjectByID(long id);
        Project GetProjectByProjectNumber(short projNumber);
        void UpdateProject(Project project);
        void CreateNewProject(Project project);
        void DeleteProject(IDictionary<long, int> projectIDDictionary);
    }
}
