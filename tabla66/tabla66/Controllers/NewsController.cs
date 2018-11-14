using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using tabla66.Models;
using System.IO;

namespace tabla66.Controllers
{
    public class NewsController : Controller
    {
        private tablanEntities8 db = new tablanEntities8();

        // GET: News
        [HttpGet]
        public ActionResult Index()
        {
            if (ValidateUser.IsAdmin())
            {
                var news = db.News.Include(n => n.Show);
                return View(news.ToList());
            }
            else
                return RedirectToAction("Index", "Shows");
        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: News/Create
        
        public ActionResult Create()
        {
            if (ValidateUser.IsAdmin())
            {
                ViewBag.Show_id = new SelectList(db.Show, "Id", "Title");
                return View();
            }
            else
                return RedirectToAction("Index", "Shows");
    }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Text,Imagefile,Show_id")] News news, HttpPostedFileBase Imagefile)
        {
            if (ValidateUser.IsAdmin())
            {
                string fileName = Path.GetFileNameWithoutExtension(Imagefile.FileName);
                string extension = Path.GetExtension(Imagefile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                news.Image = "~/Img/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Img/"), fileName);
                Imagefile.SaveAs(fileName);

                if (ModelState.IsValid)
                {
               
                    db.News.Add(news);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.Show_id = new SelectList(db.Show, "Id", "Title", news.Show_id);
                return View(news);
            }
            else
                return RedirectToAction("Index", "Shows");
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (ValidateUser.IsAdmin())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                News news = db.News.Find(id);
                if (news == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Show_id = new SelectList(db.Show, "Id", "Title", news.Show_id);
                return View(news);
            }
            else
                return RedirectToAction("Index", "Shows");
    }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Image,Text,Show_id")] News news, HttpPostedFileBase Imagefile)
        {
            if (ValidateUser.IsAdmin())
            {

                if (ModelState.IsValid)
                {
                    if ( Imagefile!=null && Imagefile.ContentLength>0)
                    {
                        string filepath = Path.Combine(Server.MapPath(news.Image));
                        System.IO.File.Delete(filepath);

                        string file = Path.GetFileNameWithoutExtension(Imagefile.FileName);
                        string extension = Path.GetExtension(Imagefile.FileName);
                        file = file + DateTime.Now.ToString("yymmssfff") + extension;
                        news.Image = "~/Img/" + file;

                        file = Path.Combine(Server.MapPath("~/Img/"), file);
                        Imagefile.SaveAs(file);
                                      
                    }
               
                    db.Entry(news).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.Show_id = new SelectList(db.Show, "Id", "Title", news.Show_id);
                return View(news);
            }
            else
                return RedirectToAction("Index", "Shows");
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (ValidateUser.IsAdmin())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                News news = db.News.Find(id);
                if (news == null)
                {
                    return HttpNotFound();
                }
                return View(news);
            }
            else
                return RedirectToAction("Index", "Shows");
    }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            string filepath = Path.Combine(Server.MapPath(news.Image));
            System.IO.File.Delete(filepath);
            db.News.Remove(news);
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
        public PartialViewResult _Carousel ()
        {
            var news = db.News.Include(n => n.Show);
            return PartialView(news.ToList());
        }
    }
}
