using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMS.Controllers
{
    public class LayoutController : Controller
    {
        // GET: Layout
        public ActionResult _Layout()
        {
            var veri = Session["ad"];
            return View();
        }
    }
}