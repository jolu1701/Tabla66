using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tabla66.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Shows");
        }

        public ActionResult About()
        {
            return RedirectToAction("Index", "Shows");
        }

        public ActionResult Contact()
        {
            return RedirectToAction("Index", "Shows");
        }
    }
}