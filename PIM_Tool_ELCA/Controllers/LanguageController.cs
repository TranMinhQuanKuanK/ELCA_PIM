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
            HttpCookie cultureCookie = Request.Cookies[Constant.CookieConstant.cultureCookieName];
            if (cultureCookie != null)
            {
                cultureCookie.Value = culture;  
            }
            else
            {
                cultureCookie = new HttpCookie(Constant.CookieConstant.cultureCookieName);
                cultureCookie.Value = culture;
                cultureCookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cultureCookie);
            return Redirect(Request.UrlReferrer.OriginalString);
        }
    }
}