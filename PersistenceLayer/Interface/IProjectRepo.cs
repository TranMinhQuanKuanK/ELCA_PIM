using DomainLayer;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.Interface
{
    public interface IProjectRepo
    {
        ProjectListPageDomainResult GetProjectList(SearchProjectRequest request, ISession session);
        Project GetProjectById(long id, ISession session);
        Project GetProjectByProjectNumber(short projNumber, ISession session);
        void UpdateProject(Project project, ISession session);
        void CreateNewProject(Project project, ISession session);
        void DeleteProject(IDictionary<long, int> projectIDDictionary, ISession session);
    }
}
