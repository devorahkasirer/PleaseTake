using AdsWithLogin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdsWithLogin.Web
{
    public class LayoutDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var db = new Manager(Properties.Settings.Default.ConStr);
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var user = db.GetByEmail(filterContext.HttpContext.User.Identity.Name);
                filterContext.Controller.ViewBag.User = user;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}