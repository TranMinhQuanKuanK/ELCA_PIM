using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIM_Tool_ELCA.Controllers
{
    public class ProjectController : CustomController
    {
        private readonly ProjectService _projectService;

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: Project
        public ActionResult Index()
        {
            return RedirectToAction("ProjectList");
        }
        public ActionResult ProjectList()
        {
            ViewBag.ProjectList = _projectService.GetProjectList();
            return View("ProjectList");
        }
    }
}