using OES.Data;
using OES.Model.Users;
using OnlineExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineExaminationSystem.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View(new LoginModel());
        }
        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            // Lets first check if the Model is valid or not
            if (ModelState.IsValid)
            {
                using (OESData db = new OESData())
                {
                    string username = model.UserName;
                    string password = model.Password;

                    // Now if our password was enctypted or hashed we would have done the
                    // same operation on the user entered password here, But for now
                    // since the password is in plain text lets just authenticate directly
                    var user = db.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
                    bool userValid = user != null;

                    // User found in the database
                    if (userValid)
                    {

                        FormsAuthentication.SetAuthCookie(username, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            switch ((UserRole)user.Role)
                            {
                                case UserRole.Admin:
                                    return Redirect("~/Admin");

                                case UserRole.Instructor:
                                    return Redirect("~/Instructor");
                            }
                            return Redirect("~/");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
           
            return Redirect("~/");
        }

        public ActionResult _PartialLogin()
        {
            OESData db = new OESData();
            var user = db.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name, StringComparison.OrdinalIgnoreCase));
            PartialLoginModel model = new PartialLoginModel();
            model.IsLogedIn = false;
            if (user != null)
            {
                model.IsLogedIn = true;
                model.UserName = user.Name;
                model.UserId = user.UserId;
                model.LastVisit = user.LastVisit;
               
            }
            return View(model);
        }
    }


}