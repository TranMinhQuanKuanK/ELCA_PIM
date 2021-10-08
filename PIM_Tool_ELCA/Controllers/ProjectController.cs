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
            var result = _projectService.
            GetProjectList(
            Session["SearchTerm"] != null ? Session["SearchTerm"].ToString() : "",
            Session["SearchStatus"] != null ? Session["SearchStatus"].ToString() : ""
            , 1
            , 5
            );
            ViewBag.ProjectList = result.projectList.OrderBy(proj => proj.ProjectNumber).ToList<ProjectListModel>();
            ViewBag.ResultCount = result.resultCount;
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

            var result = _projectService.GetProjectList(
                searchProjectModel.SearchTerm
                , searchProjectModel.SearchStatus
                , searchProjectModel.PageIndex
                , searchProjectModel.PageSize);

            ViewBag.ProjectList = result.projectList
                .OrderBy(proj => proj.ProjectNumber).ToList<ProjectListModel>();
            ViewBag.ResultCount = result.resultCount;
            //ViewBag.SearchTerm = searchProjectModel.SearchTerm;
            // ViewBag.SearchStatus = searchProjectModel.SearchStatus;
            ViewBag.CurrentPage = searchProjectModel.PageIndex;
            ViewBag.CurrentPageSize = searchProjectModel.PageSize;
            return View("ProjectList");
        }

        [HandleError]
        public ActionResult EditProject(int id)
        {
            ViewBag.NewOREdit = "Edit";
            ViewBag.VisaList = _employeeService.GetAllMembers();
            ViewBag.GroupList = _groupService.GetGroupIDList();
            return View("AddEditProject", _projectService.GetProjectByID(id));
        }

        [HandleError]
        [HttpPost]
        public ActionResult EditProject(int id,
            [Bind(Include = "ID, GroupID,ProjectNumber,Name,Customer,Status,StartDate,EndDate,Members,Version")] AddEditProjectModel projectModel)
        {

            //GroupID exist
            try
            {
                if (ModelState.IsValid)
                {
                    List<string> memberList = new List<string>(VisaHelper.SplitVisa(projectModel.Members));
                    projectModel.MembersList = memberList;
                    _projectService.ValidateProjectModelAndUpdate(projectModel);
                }

            }
            catch (VersionLowerThanCurrentVersionException)
            {
                ViewBag.VersionWarning = "Trueeeeee";
                ModelState.Clear();

                ViewBag.NewOREdit = "Edit";
                ViewBag.VisaList = _employeeService.GetAllMembers();
                ViewBag.GroupList = _groupService.GetGroupIDList();
                return View("AddEditProject", _projectService.GetProjectByID((long)projectModel.ID));
            }
            catch (GroupIDDoesntExistException)
            {
                ModelState.AddModelError("GroupID", "GroupID doesn't exist");
            }
            catch (CantChangeProjectNumberException)
            {
                ModelState.AddModelError("ProjectNumber", "Can't change project number");
            }
            catch (InvalidVisaException)
            {
                ModelState.AddModelError("Members", "Invalid member list");
            }
            catch (InvalidStatusException)
            {
                ModelState.AddModelError("Status", "Invalid status");
            }
            catch (EndDateSoonerThanStartDateException)
            {
                ModelState.AddModelError("EndDate", "End date can't be sooner than start date.");
            }


            //-----------------------------------------------------------------------------
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.NewOREdit = "Edit";
                ViewBag.VisaList = _employeeService.GetAllMembers();
                ViewBag.GroupList = _groupService.GetGroupIDList();

                return View("AddEditProject", projectModel);
            }
        }

        [HttpGet]
        [HandleError]
        public ActionResult NewProject()
        {
            ViewBag.NewOREdit = "New";
            ViewBag.VisaList = _employeeService.GetAllMembers();
            ViewBag.GroupList = _groupService.GetGroupIDList();
            return View("AddEditProject", new AddEditProjectModel());
        }

        [HttpPost]
        [HandleError]
        public ActionResult NewProject([Bind(Include = "GroupID,ProjectNumber,Name,Customer,Status,StartDate,EndDate,Members,Version")] AddEditProjectModel projectModel)
        {
            try
            {
                if (ModelState.ContainsKey("ID"))
                    ModelState["ID"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    List<string> memberList = new List<string>(VisaHelper.SplitVisa(projectModel.Members));
                    projectModel.MembersList = memberList;
                    _projectService.ValidateAndCreateNewProject(projectModel);
                }

            }
            catch (GroupIDDoesntExistException)
            {
                ModelState.AddModelError("GroupID", "GroupID doesn't exist");
            }
            catch (ProjectNumberDuplicateException)
            {
                ModelState.AddModelError("ProjectNumber", Resource.AddEditProject.AddEditProjectRe.DuplicateProjectNumber_ModelError);
            }
            catch (InvalidVisaException)
            {
                ModelState.AddModelError("Members", "Invalid member list");
            }
            catch (InvalidStatusException)
            {
                ModelState.AddModelError("Status", "Invalid status");
            }
            catch (EndDateSoonerThanStartDateException)
            {
                ModelState.AddModelError("EndDate", Resource.AddEditProject.AddEditProjectRe.EndDate_ModelError);
            }


            //-----------------------------------------------------------------------------
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.NewOREdit = "New";
                ViewBag.VisaList = _employeeService.GetAllMembers();
                ViewBag.GroupList = _groupService.GetGroupIDList();

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
                    hasError = true,
                    errMessage = projectList.Count == 1
                    ? Resource.ProjectList.ProjectListRe.ProjectDoesntExist_DeleteError
                    : Resource.ProjectList.ProjectListRe.ProjectDoesntExistMultiple_DeleteError
                });
            }
            catch (CantDeleteProjectDueToLowerVersionException e)
            {
                return Json(new DeleteProjectResponseModel()
                {
                    hasError = true,
                    errMessage = projectList.Count == 1
                    ? Resource.ProjectList.ProjectListRe.ProjectHasLowerVersion_DeleteError
                    : Resource.ProjectList.ProjectListRe.ProjectHasLowerVersionMultiple_DeleteError
                });
            }
            catch (ProjectStatusNotNewException e)
            {
                return Json(new DeleteProjectResponseModel()
                {
                    hasError = true,
                    errMessage = "Project status invalid"
                });
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return Json(new DeleteProjectResponseModel()
            {
                hasError = false,
                errMessage = "No error "
            });
        }
    }
}