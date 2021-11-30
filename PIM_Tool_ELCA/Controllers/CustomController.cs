using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace PIM_Tool_ELCA.Controllers
{
    public class CustomController : Controller
    {
        const string CookieName = "_culture";
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.Result = RedirectToAction("Home", "NotFound");
        }
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "*");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Credentials", "true");

            ViewBag.CurrentLanguage = Thread.CurrentThread.CurrentCulture;
            base.OnResultExecuting(filterContext);
            
        }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName;

            HttpCookie cultureCookie = Request.Cookies[CookieName];
            if (cultureCookie != null)
            {
                cultureName = cultureCookie.Value;

            }
            else
            {
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0
                ? Request.UserLanguages[0]
                : null;
            }
            cultureName = CultureHelper.GetImplementedCulture(cultureName);

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
           
        }
    }
}