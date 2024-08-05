using Microsoft.Ajax.Utilities;
using RMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMS.Controllers
{
    public class QrMenuController : Controller
    {

        RMSEntities db=new RMSEntities();
        // GET: QrMenu
        public IEnumerable<string> KategoriGetir()
        {

            var kategori = db.Menu.Select(x => x.YemekKategori).Distinct().ToList();

           
           

            return kategori;
        }
        public ActionResult Menu()
        {

            List<Menu> menu =db.Menu.ToList();
          


            return View(menu);
        }
    }
}