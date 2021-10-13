using ContractLayer;
using ServiceLayer;
using ServiceLayer.CustomException.ProjectException;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace PIM_Tool_ELCA.Controllers
{
    public enum EditMode
    {
        Edit,
        New
    }
    public class ProjectController : CustomController
    {

        private readonly IProjectService _projectService;
        private readonly IEmployeeService _employeeService;
        private readonly IGroupService _groupService;
        public ProjectController(IProjectService projectService, IEmployeeService employeeService, IGroupService groupService)
        {
            _projectService = projectService;
            _employeeService = employeeService;
            _groupService = groupService;
        }

        [HandleError]
        public ActionResult Index()
        {
            return RedirectToAction("ProjectList");
        }

        [HandleError]
        public ActionResult ProjectList()
        {
            const int defaultPageIndex = 1;
            const int defaultPageSize = 5;
            SearchProjectRequestModel request = new SearchProjectRequestModel()
            {
                PageIndex = defaultPageIndex,
                PageSize = defaultPageSize,
                SearchTerm = Session["SearchTerm"] != null ? Session["SearchTerm"].ToString() : string.Empty,
                SearchStatus = Session["SearchStatus"] != null ? Session["SearchStatus"].ToString() : string.Empty,
            };
            var result = _projectService.GetProjectList(request);
            ViewBag.ProjectList = result.ProjectList.ToList<ProjectListModel>();

            ViewBag.ResultCount = result.ResultCount;
            ViewBag.CurrentPage = 1;
            ViewBag.CurrentPageSize = 5;
            return View("ProjectList");
        }

        [HttpGet]
        public ActionResult SearchProject([Bind(Include = "SearchTerm, SearchStatus, PageIndex, PageSize")] SearchProjectModel searchProjectModel)
        {
            if (searchProjectModel.PageIndex == 0 || searchProjectModel.PageSize == 0) return Redirect("/Home/NotFound");

            Session["SearchTerm"] = searchProjectModel.SearchTerm;
            Session["SearchStatus"] = searchProjectModel.SearchStatus;

            SearchProjectRequestModel request = new SearchProjectRequestModel()
            {
                PageIndex = searchProjectModel.PageIndex,
                PageSize = searchProjectModel.PageSize,
                SearchStatus = searchProjectModel.SearchStatus,
                SearchTerm = searchProjectModel.SearchTerm,
            };

            var result = _projectService.GetProjectList(request);

            ViewBag.ProjectList = result.ProjectList
                .OrderBy(proj => proj.ProjectNumber).ToList<ProjectListModel>();
            ViewBag.ResultCount = result.ResultCount;
            ViewBag.CurrentPage = searchProjectModel.PageIndex;
            ViewBag.CurrentPageSize = searchProjectModel.PageSize;
            return View("ProjectList");
        }
        private List<SelectListItem> BuildGroupList(IList<long> groupIdList, long id)
        {
            List<SelectListItem> groupList = new List<SelectListItem>();
            foreach (var item in groupIdList)
            {
                groupList.Add(new SelectListItem
                {
                    Text = Resource.AddEditProject.AddEditProjectRe.Group_WordLabel + " " + item.ToString(),
                    Value = item.ToString(),
                    Selected = id == item
                });
            }
            return groupList;
        }
            [HandleError]
        public ActionResult EditProject(long id)
        {
            ViewBag.EditMode = EditMode.Edit;
            ViewBag.VisaList = _employeeService.GetAllMembers();
            ViewBag.GroupList = BuildGroupList(_groupService.GetGroupIdList(), id);
            return View("AddEditProject", _projectService.GetProjectById(id));
        }

        [HandleError]
        [HttpPost]
        public ActionResult EditProject(
            [Bind(Include = "ID, GroupID,ProjectNumber,Name,Customer,Status,StartDate,EndDate,MemberString,Version")] AddEditProjectModel projectModel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    List<string> memberList = new List<string>(VisaHelper.SplitVisa(projectModel.MemberString));
                    projectModel.MembersList = memberList;
                    _projectService.Update(projectModel);
                }


            }
            catch (ProjectHaveBeenEditedByAnotherUserException)
            {
                ViewBag.HasVersionWarning = true;
                ModelState.Clear();

                ViewBag.EditMode = EditMode.Edit;
                ViewBag.VisaList = _employeeService.GetAllMembers();
                ViewBag.GroupList = BuildGroupList(_groupService.GetGroupIdList(), projectModel.Id.Value);
                var targetProject = _projectService.GetProjectById((long)projectModel.Id);
                return View("AddEditProject", targetProject);
            }
            catch (GroupIDDoesntExistException)
            {
                ModelState.AddModelError(nameof(projectModel.GroupId), "GroupID doesn't exist");
            }
            catch (CantChangeProjectNumberException)
            {
                ModelState.AddModelError(nameof(projectModel.ProjectNumber), "Can't change project number");
            }
            catch (InvalidVisaException)
            {
                ModelState.AddModelError(nameof(projectModel.MemberString), "Invalid member list");
            }
            catch (InvalidStatusException)
            {
                ModelState.AddModelError(nameof(projectModel.Status), "Invalid status");
            }
            catch (EndDateSoonerThanStartDateException)
            {
                ModelState.AddModelError(nameof(projectModel.EndDate), "End date can't be sooner than start date.");
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.EditMode = EditMode.Edit;
                ViewBag.VisaList = _employeeService.GetAllMembers();
                ViewBag.GroupList = BuildGroupList(_groupService.GetGroupIdList(), projectModel.Id.Value);

                return View("AddEditProject", projectModel);
            }
        }

        [HttpGet]
        [HandleError]
        public ActionResult NewProject()
        {
            ViewBag.EditMode = EditMode.New;
            ViewBag.VisaList = _employeeService.GetAllMembers();
            ViewBag.GroupList = BuildGroupList(_groupService.GetGroupIdList(), 0);
            return View("AddEditProject", new AddEditProjectModel());
        }

        [HttpPost]
        [HandleError]
        public ActionResult NewProject([Bind(Include = "GroupId,ProjectNumber,Name,Customer,Status,StartDate,EndDate,MemberString,Version")] AddEditProjectModel projectModel)
        {
            try
            {
                if (ModelState.ContainsKey("Id"))
                    ModelState["Id"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    List<string> memberList = new List<string>(VisaHelper.SplitVisa(projectModel.MemberString));
                    projectModel.MembersList = memberList;
                    _projectService.Create(projectModel);
                }

            }
            catch (GroupIDDoesntExistException)
            {
                ModelState.AddModelError(nameof(projectModel.GroupId), "GroupID doesn't exist");
            }
            catch (ProjectNumberDuplicateException)
            {
                ModelState.AddModelError(nameof(projectModel.ProjectNumber), Resource.AddEditProject.AddEditProjectRe.DuplicateProjectNumber_ModelError);
            }
            catch (InvalidVisaException)
            {
                ModelState.AddModelError(nameof(projectModel.MemberString), "Invalid member list");
            }
            catch (InvalidStatusException)
            {
                ModelState.AddModelError(nameof(projectModel.Status), "Invalid status");
            }
            catch (EndDateSoonerThanStartDateException)
            {
                ModelState.AddModelError(nameof(projectModel.EndDate), Resource.AddEditProject.AddEditProjectRe.EndDate_ModelError);
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.EditMode = EditMode.New;
                ViewBag.VisaList = _employeeService.GetAllMembers();
                ViewBag.GroupList = BuildGroupList(_groupService.GetGroupIdList(),0);

                return View("AddEditProject", projectModel);
            }
        }

        [HttpPost]
        public ActionResult DeleteProject(List<DeleteProjectRequestModel> projectList)
        {
            try
            {
                _projectService.DeleteProject(projectList);
            }
            catch (ProjectNotExistedException e)
            {
                return Json(new DeleteProjectResponseModel()
                {
                    HasError = true,
                    ErrMessage = projectList.Count == 1
                    ? Resource.ProjectList.ProjectListRe.ProjectDoesntExist_DeleteError
                    : Resource.ProjectList.ProjectListRe.ProjectDoesntExistMultiple_DeleteError
                });
            }
            catch (CantDeleteProjectBecauseProjectHasBeenChangedException e)
            {
                return Json(new DeleteProjectResponseModel()
                {
                    HasError = true,
                    ErrMessage = projectList.Count == 1
                    ? Resource.ProjectList.ProjectListRe.ProjectHasLowerVersion_DeleteError
                    : Resource.ProjectList.ProjectListRe.ProjectHasLowerVersionMultiple_DeleteError
                });
            }
            catch (ProjectStatusNotNewException e)
            {
                return Json(new DeleteProjectResponseModel()
                {
                    HasError = true,
                    ErrMessage = "Project status invalid"
                });
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return Json(new DeleteProjectResponseModel()
            {
                HasError = false,
                ErrMessage = "No error "
            });
        }
    }
}