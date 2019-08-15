using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    using Portal.Models;
    public class UserController : Controller
    {
        // GET: User
        public ActionResult CreateOrEditUser(int id=0)
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        public ActionResult CreateOrEditUser(User user)
        {
            using(DBmodels db=new DBmodels())
            {
                if (db.Users.Any(x => user.UserName == x.UserName))
                {
                    ViewBag.DuplicateMessage = "User Name '" + user.UserName + "' already exists";
                    return View("CreateOrEditUser", user);
                }
                else
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                
            }
            ViewBag.successmessage = "Successfully '" + user.UserName + "' added to the portal";
            ModelState.Clear();
            return View("CreateOrEditUser",new User());
        }

        public ActionResult Login(int id = 0)
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            using (DBmodels db = new DBmodels())
            {
                var userDetails = db.Users.Where(x => x.UserName == user.UserName && x.Password == user.Password).FirstOrDefault();
                if (userDetails == null)
                {
                    user.LoginError = "Wrong User Name or Password";
                    return View("Login",user);
                }
                else
                {
                    Session["UserID"] = user.UserId;
                    Session["UserName"] = user.UserName;
                    return RedirectToAction("Welcome", "User");
                }
            }
                return View(user);
        }

        public ActionResult Welcome(User user)
        {
            return View(user);
        }

        public ActionResult Logout()
        {
            int userID =(int) Session["UserID"];
            Session.Abandon();
            return RedirectToAction("Login", "User");
        }
    }
}