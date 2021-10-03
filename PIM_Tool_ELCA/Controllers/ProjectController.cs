using ContractLayer;
using ServiceLayer;
using ServiceLayer.CustomException.ProjectException;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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
        // GET: Project
        public ActionResult Index()
        {
            return RedirectToAction("ProjectList");
        }
        [HandleError]
        public ActionResult ProjectList()
        {
            ViewBag.ProjectList = _projectService.
                GetProjectList(
                Session["SearchTerm"] != null ? Session["SearchTerm"].ToString() : "",
                Session["SearchStatus"] != null ? Session["SearchStatus"].ToString() : ""
                );

            return View("ProjectList");
        }
        public ActionResult SearchProject([Bind(Include = "SearchTerm, SearchStatus")] SearchProjectModel searchProjectModel)
        {
            ViewBag.ProjectList = _projectService.GetProjectList(searchProjectModel.SearchTerm, searchProjectModel.SearchStatus);
            ViewBag.SearchTerm = searchProjectModel.SearchTerm;
            Session["SearchTerm"] = searchProjectModel.SearchTerm;
            Session["SearchStatus"] = searchProjectModel.SearchStatus;
            ViewBag.SearchStatus = searchProjectModel.SearchStatus;
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
            [Bind(Include = "ID, GroupID,ProjectNumber,Name,Customer,Status,StartDate,EndDate,Members")] AddEditProjectModel projectModel)
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
                //update
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
        public ActionResult NewProject([Bind(Include = "ID, GroupID,ProjectNumber,Name,Customer,Status,StartDate,EndDate,Members")] AddEditProjectModel projectModel)
        {
            ModelState["ID"].Errors.Clear();
            ViewBag.NewOREdit = "New";
            ViewBag.VisaList = _employeeService.GetAllMembers();
            ViewBag.GroupList = _groupService.GetGroupIDList();
            return View("AddEditProject", new AddEditProjectModel());
        }
        //public ActionResult DeleteMultiple([Bind(Include = "ProjectList")] DeleteMultipleProjectRequestModel deleteMultipleProjectModel)
        //{
        //    ViewBag.ProjectList = _projectService.GetProjectList("", "");//sẽ xóa sau và thay bằng logic đúng
        //    return Json();
        //}

    }
}