using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tabla66.Models;
using tabla66.ViewModel;

namespace tabla66.Controllers
{
    public class ChannelVMController : Controller
    {
        // GET: ChannelVM
        public ActionResult Index()
        {
            tablanEntities1 tb = new tablanEntities1();
            List<ChannelShowsVM> CSList = new List<ChannelShowsVM>();
            var channelshowlist = (from s in tb.Show
                                   join c in tb.Channel on s.Channel_id equals c.Id
                                   select new { s.Title, c.Channel1, s.Info, s.Start_time, s.Duration }).ToList();

            foreach (var v in channelshowlist)
            {
                ChannelShowsVM item = new ChannelShowsVM();
                item.Title = v.Title;
                item.Channel1 = v.Channel1;
                item.Info = v.Info;
                item.Start_time = v.Start_time;
                item.Duration = v.Duration;
                CSList.Add(item);
            }
            
            return View(CSList);
        }
    }
}