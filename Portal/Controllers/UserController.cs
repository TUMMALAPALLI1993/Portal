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
                }
                else
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                
            }
            ViewBag.successmessage = "Successfully " + user.UserName + " added to the portal";
            ModelState.Clear();
            return View("CreateOrEditUser",new User());
        }
    }
}