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
    public class ShowsController : Controller
    {
        private tablanEntities8 db = new tablanEntities8();

        // GET: Shows of today or requested days ahead
        public ActionResult Index(int? daysAhead)
        {
            if (!daysAhead.HasValue)
                daysAhead = 0;

            var culture = new System.Globalization.CultureInfo("sv-SV");
            string dagnamn = culture.DateTimeFormat.GetDayName(DateTime.Now.AddDays(Convert.ToDouble(daysAhead)).DayOfWeek);
            ViewBag.day = dagnamn;

            if (Session["userId"] != null) //inloggad
            {
                int userId = Convert.ToInt32(Session["userId"]);
                User user = (from data in db.User where data.Id == userId select data).FirstOrDefault();
                List<int> kanaler = new List<int>();
                foreach (var channel in user.Channel)
                {
                    kanaler.Add(channel.Id);
                }
                var show = (from shw in db.Show where kanaler.Contains(shw.Channel.Id) select shw).ToList();
                show = show.Where(s => s.Start_time.Day == DateTime.Now.Day + daysAhead).ToList();
                show = show.OrderBy(s => s.Channel_id).ThenBy(s => s.Start_time).ToList();                
                return View(show);
            }

            else //anonym användare
            {
                var show = db.Show.Include(s => s.Channel).Include(s => s.Genre).OrderBy(s => s.Channel_id).ThenBy(s => s.Start_time).Where(s => s.Start_time.Day == DateTime.Now.Day + daysAhead);
                return View(show);
            }
            
        }

        public ActionResult ByGenres(int? reqGenreId, int? daysAhead)
        {
            if (!reqGenreId.HasValue)
                reqGenreId = 1;
            if (!daysAhead.HasValue)
                daysAhead = 0;
            var show = db.Show.Include(s => s.Channel).Include(s => s.Genre).OrderBy(s => s.Start_time).ThenBy(s => s.Channel_id).Where(s => s.Genre_id==reqGenreId && s.Start_time.Day == DateTime.Now.Day + daysAhead);
            return View(show);
        }

        public ActionResult ByChannel(int? reqChannelId)
        {
            if (!reqChannelId.HasValue)
                reqChannelId = 1;            
            var show = db.Show.Include(s => s.Channel).Include(s => s.Genre).OrderBy(s => s.Start_time).Where(s => s.Channel_id == reqChannelId && s.Start_time.Day >= DateTime.Now.Day);
            Channel c = (from channel in db.Channel where channel.Id == reqChannelId select channel).FirstOrDefault();
            ViewBag.Channel = c.Channel1;
            return View(show);
        }

        // GET: Shows/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Show show = db.Show.Find(id);
            if (show == null)
            {
                return HttpNotFound();
            }
            return View(show);
        }

        // GET: Shows/Create
        public ActionResult Create()
        {
            ViewBag.Channel_id = new SelectList(db.Channel, "Id", "Channel1");
            ViewBag.Genre_id = new SelectList(db.Genre, "Id", "Genre1");
            return View();
        }

        // POST: Shows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Channel_id,Genre_id,Start_time,Duration,Info")] Show show)
        {
            if (ModelState.IsValid)
            {
                db.Show.Add(show);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Channel_id = new SelectList(db.Channel, "Id", "Channel1", show.Channel_id);
            ViewBag.Genre_id = new SelectList(db.Genre, "Id", "Genre1", show.Genre_id);
            return View(show);
        }

        // GET: Shows/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Show show = db.Show.Find(id);
            if (show == null)
            {
                return HttpNotFound();
            }
            ViewBag.Channel_id = new SelectList(db.Channel, "Id", "Channel1", show.Channel_id);
            ViewBag.Genre_id = new SelectList(db.Genre, "Id", "Genre1", show.Genre_id);
            return View(show);
        }

        // POST: Shows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Channel_id,Genre_id,Start_time,Duration,Info")] Show show)
        {
            if (ModelState.IsValid)
            {
                db.Entry(show).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Channel_id = new SelectList(db.Channel, "Id", "Channel1", show.Channel_id);
            ViewBag.Genre_id = new SelectList(db.Genre, "Id", "Genre1", show.Genre_id);
            return View(show);
        }

        // GET: Shows/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Show show = db.Show.Find(id);
            if (show == null)
            {
                return HttpNotFound();
            }
            return View(show);
        }

        // POST: Shows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Show show = db.Show.Find(id);
            db.Show.Remove(show);
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
