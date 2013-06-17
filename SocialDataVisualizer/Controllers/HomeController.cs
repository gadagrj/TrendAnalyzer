using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialDataVisualizer.Controllers
{
    public class HomeController : Controller
    {
        [AllowCrossSiteJson]
        public ActionResult Index()
        {
           //TwitterRestApiManager objMan = new TwitterRestApiManager();
           //string str = objMan.getdata();
            return View();
        }
    }
}
