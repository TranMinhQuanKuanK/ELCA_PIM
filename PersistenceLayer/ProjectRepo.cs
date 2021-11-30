using DomainLayer;
using NHibernate;
using NHibernate.Criterion;
using PersistenceLayer.CustomException.Project;
using PersistenceLayer.Helper;
using PersistenceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer
{
    public class ProjectRepo : IProjectRepo
    {
        private void BuildRestrictionForCritera(ICriteria criteria, string searchTerm, string searchStatus)
        {
            if (searchTerm != string.Empty)
                criteria.Add(Expression.Disjunction()
                                      .Add(Expression.Like("Name", $"%{searchTerm}%"))
                                      .Add(Expression.Like(Projections.Cast(NHibernateUtil.String, Projections.Property("ProjectNumber")), $"%{searchTerm}%"))
                                      .Add(Expression.Like("Customer", $"%{searchTerm}%"))
                                    );
            if (searchStatus != string.Empty)
                criteria.Add(Expression.Like("Status", $"%{searchStatus}%"));

        }
        public ProjectListPageDomainResult GetProjectList(SearchProjectRequest request, ISession session)
        {

            IList<Project> result = new List<Project>();
            request.SearchTerm = request.SearchTerm == null ? string.Empty : request.SearchTerm.Trim().ToUpper();
            request.SearchStatus = request.SearchStatus == null ? string.Empty : request.SearchStatus.Trim().ToUpper();

            var criteria = session.CreateCriteria<Project>();
            BuildRestrictionForCritera(criteria, request.SearchTerm, request.SearchStatus);
            criteria.AddOrder(Order.Asc("ProjectNumber"));
            criteria.SetFirstResult((request.PageIndex - 1) * request.PageSize).SetMaxResults(request.PageSize);
            result = criteria.List<Project>();


            var countCriteria = session.CreateCriteria<Project>();
            BuildRestrictionForCritera(countCriteria, request.SearchTerm, request.SearchStatus);
            var count = countCriteria.SetProjection(Projections.Count(Projections.Id())).UniqueResult<int>();
            return new ProjectListPageDomainResult
            {
                projectList = result,
                resultCount = count
            };
        }
        public List<Project> GetProjectByIdList(List<long> idList, ISession session)
        {

            List<Project> result = null;

            result = (List<Project>)session.QueryOver<Project>().Where(item => idList.Contains(item.Id)).List<Project>();

            return result;
        }//saiiiii
        public Project GetProjectById(long id, ISession session)
        {


            Project result = null;

            result = session.QueryOver<Project>().Where(item => item.Id == id).SingleOrDefault<Project>();

            return result;
        }

        public Project GetProjectByProjectNumber(short projNumber, ISession session)
        {

            Project result = null;

            result = session.QueryOver<Project>()
                .Where(item => item.ProjectNumber == projNumber)
                .SingleOrDefault<Project>();

            return result;
        }

        public void UpdateProject(Project project, ISession session)
        {

            using (var tx = session.BeginTransaction())
            {
                try
                {
                    session.Update(project);
                    tx.Commit();
                }
                catch (NHibernate.StaleObjectStateException e)
                {
                    throw new VersionLowerThanCurrentVersionException("Version lower than current version", e);
                }
            }
        }
        public void CreateNewProject(Project project, ISession session)
        {

            using (var tx = session.BeginTransaction())
            {
                session.Save(project);
                tx.Commit();
            }
        }
        public void DeleteProject(IDictionary<long, int> projectIdDictionary, ISession session)
        {

            using (var tx = session.BeginTransaction())
            {
                try
                {
                    foreach (var item in projectIdDictionary)
                    {
                        Project _proj = new Project();
                        session.Load(_proj, item.Key);

                        if (item.Value != _proj.Version)
                        {
                            throw new CantDeleteProjectDueToLowerVersionException();
                        }
                        if (_proj.Status != "NEW")
                        {
                            throw new ProjectStatusNotNewException();
                        }
                        session.Delete(_proj);
                    }

                }
                catch (ObjectNotFoundException e)
                {
                    throw new ProjectNotExistedException("Some of the projects don't exist!", e);
                }
                tx.Commit();
            }
        }
    }
}
