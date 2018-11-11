using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tabla66.Models;
using tabla66.ViewModel;

namespace tabla66.Controllers
{
    public class AdminPanelController : Controller
    {
        // GET: AdminPanel
        public ActionResult Dashboard()
        {
            if(ValidateUser.IsUserValid())
            {
                return View();
            }

            else
            {
                return RedirectToAction("Index", "Shows");
            }
        }
    }
}