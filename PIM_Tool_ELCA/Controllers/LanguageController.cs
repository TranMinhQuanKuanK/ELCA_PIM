using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace PIM_Tool_ELCA.Controllers
{
    public class LanguageController : CustomController
    {
        public ActionResult Index(string culture)
        {
            culture = CultureHelper.GetImplementedCulture(culture);
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
            {
                cookie.Value = culture;  
            }
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(Request.UrlReferrer.OriginalString);
        }
    }
}