using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using tabla66.Models;

namespace tabla66.Controllers
{
    public class UsersController : Controller
    {
        private tablanEntities8 db = new tablanEntities8();

        // GET: Users
        public ActionResult Index()
        {
            string btnClick = Request["loginBtn"];
            if (btnClick == "Login")
            {
                string userName = Request["userName"];
                string password = Request["password"];
                var userLogin = (from data in db.User where data.Name == userName && data.Password == password select data).FirstOrDefault();

                if(userLogin != null)
                {
                    Session["userName"] = userLogin.Name;
                    Session["userId"] = userLogin.Id;
                    return RedirectToAction("MyPage", "Users");
                }

                else if(userLogin==null)
                {
                    return RedirectToAction("Login", "Users");
                }
            }

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult MyPage()
        {
            if (ValidateUser.IsUserValid())
            {                
                int userId = Convert.ToInt32(Session["userId"]);
                User user = (from data in db.User where data.Id == userId select data).FirstOrDefault();                
                ViewBag.AmountOfChannels = db.Channel.Count();
                return View(user);
            }

            else
            {
                return RedirectToAction("Login", "Users");
            }
        }

        public ActionResult AddToMyChannels()
        {
            int userId = Convert.ToInt32(Session["userId"]);
            User user = (from data in db.User.Include(s => s.Channel) where data.Id == userId select data).FirstOrDefault();

            string btnClick = Request["AddKanal"];
            int chanId = Convert.ToInt32(btnClick);

            var cnl = (from data in db.Channel where data.Id == chanId select data).FirstOrDefault(); //hittar igen nya favoritkanalen och sparar till en var
            if(!db.User.FirstOrDefault(u => u.Id == userId).Channel.Contains(cnl)) 
                {
                    db.User.FirstOrDefault(u => u.Id == userId).Channel.Add(cnl); // Så länge kanalen inte finns i användarens favoritlista så sparas den              
                db.SaveChanges();
                }
            return RedirectToAction("MyPage","Users");            
        }

        public ActionResult RemoveFromMyChannels()
        {
            int userId = Convert.ToInt32(Session["userId"]);
            User user = (from data in db.User.Include(s => s.Channel) where data.Id == userId select data).FirstOrDefault();

            string btnClick = Request["RemoveKanal"];
            int chanId = Convert.ToInt32(btnClick);

            var cnl = (from data in db.Channel where data.Id == chanId select data).FirstOrDefault(); //hittar igen kanalobjektet och sparar till en var
            if (db.User.FirstOrDefault(u => u.Id == userId).Channel.Contains(cnl))
            {
                db.User.FirstOrDefault(u => u.Id == userId).Channel.Remove(cnl); // Så länge kanalen finns i användarens favoritlista så tas den bort              
                db.SaveChanges();
            }
            return RedirectToAction("MyPage", "Users");
        }

        public ActionResult UpdateMyChannels()
        {
            int userId = Convert.ToInt32(Session["userId"]);
            User user = (from data in db.User.Include(s => s.Channel) where data.Id == userId select data).FirstOrDefault();

            string btnClick = Request["UpdateKanal"];
            int chanId = Convert.ToInt32(btnClick);

            var cnl = (from data in db.Channel where data.Id == chanId select data).FirstOrDefault(); //hittar igen nya favoritkanalen och sparar till en var
            if (!db.User.FirstOrDefault(u => u.Id == userId).Channel.Contains(cnl))            
                db.User.FirstOrDefault(u => u.Id == userId).Channel.Add(cnl); // Så länge kanalen inte finns i användarens favoritlista så sparas den   
            else
                db.User.FirstOrDefault(u => u.Id == userId).Channel.Remove(cnl);
            db.SaveChanges();
            return RedirectToAction("MyPage", "Users");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index","Shows");
        }


        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Password,Email")] User user)
        {
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = (from data in db.User.Include(s => s.Channel) where data.Id == id select data).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Password,Email")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
