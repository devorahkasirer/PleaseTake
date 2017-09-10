using AdsWithLogin.Data;
using AdsWithLogin.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AdsWithLogin.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Manager manager = new Manager(Properties.Settings.Default.ConStr);
            AdsViewModel vm = new AdsViewModel();
            vm.Ads = manager.GetAds();
            if(User.Identity.IsAuthenticated)
            {
                string email = User.Identity.Name;
                vm.User = manager.GetByEmail(email);
            }
            return View(vm);
        }
        public ActionResult Details(int Id)
        {
            Manager manager = new Manager(Properties.Settings.Default.ConStr);
            AdDetailsViewModel vm = new AdDetailsViewModel();
            var ad = manager.GetAdById(Id);
            vm.Ad = ad;
            string email = User.Identity.Name;
            User user = manager.GetByEmail(email);
            vm.Posted = User.Identity.IsAuthenticated && (ad.User.Id == user.Id);
            return View(vm);
        }
        public ActionResult NewUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(User user, string password)
        {
            Manager manager = new Manager(Properties.Settings.Default.ConStr);
            manager.AddUser(user, password);
            return Redirect("/home/index");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            Manager manager = new Manager(Properties.Settings.Default.ConStr);
            User user = manager.Login(email, password);
            if (user == null)
            {
                return Redirect("/home/login");
            }
            FormsAuthentication.SetAuthCookie(email, true);
            return Redirect("/home/Index");
        }
        [Authorize]
        public ActionResult AddAd()
        {
            string email = User.Identity.Name;
            Manager manager = new Manager(Properties.Settings.Default.ConStr);
            UserViewModel vm = new UserViewModel();
            vm.User = manager.GetByEmail(email);
            return View(vm);
        }
        [HttpPost]
        public ActionResult Add(Ad ad, HttpPostedFileBase image, int userId)
        {
            Manager manager = new Manager(Properties.Settings.Default.ConStr);
            string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            image.SaveAs(Server.MapPath("~/Images") + "/" + fileName);
            ad.FileName = fileName;
            ad.DateListed = DateTime.Now;
            int Id = manager.AddAd(ad, userId);
            return Redirect("/home/index");
        }
        [HttpPost]
        public ActionResult Remove(int Id)
        {
            Manager manager = new Manager(Properties.Settings.Default.ConStr);
            manager.RemoveAd(Id);
            return Redirect("/home/index");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/home/index");
        }
    }
}