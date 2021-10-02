﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIM_Tool_ELCA.Controllers
{
    public class HomeController : CustomController
    {
        public ActionResult Index()
        {
            return Redirect("Project/ProjectList");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult NotFound()
        {
            return View("NotFound");
        }
    }
}