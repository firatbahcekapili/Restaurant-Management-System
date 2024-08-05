using Microsoft.Ajax.Utilities;
using RMS.Models;
using RMS.Models.Managers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RMS.Controllers
{
    public class adminController : Controller
    {
        // GET: admin
        private RMSEntities db = new RMSEntities();



        public ActionResult onlyAdmin()
        {


            AdminViewModel Viewmodel = new AdminViewModel()
            {
                //Personeller = db.Personeller.ToList(),
                //Adminler = db.Adminler.ToList(),
                //Masalar=db.Masalar.ToList(),
                //Menu=db.Menu.ToList()



            };
            Viewmodel.Personeller = db.Personeller.Where(x => x.Durum == true).ToList();
            Viewmodel.Adminler = db.Adminler.Where(x => x.Durum == true).ToList();
            Viewmodel.Masalar = db.Masalar.Where(x => x.DurumM == true).ToList();
            Viewmodel.Menu = db.Menu.Where(x => x.Durum == true).ToList();
            Viewmodel.Siparisler = db.Siparisler.ToList();


            return View(Viewmodel);
        }


        public ActionResult yeni()
        {
            MasaViewModel model = new MasaViewModel();
            model.Masalar = db.Masalar.Where(x => x.DurumM == true).ToList();
            model.Menu = db.Menu.Where(x => x.Durum == true).ToList();


            return View(model);
        }


        public ActionResult Yonetim()
        {
            AdminViewModel Viewmodel = new AdminViewModel()
            {
                //Personeller = db.Personeller.ToList(),
                //Adminler = db.Adminler.ToList(),
                //Masalar=db.Masalar.ToList(),
                //Menu=db.Menu.ToList()



            };
            Viewmodel.Personeller = db.Personeller.Where(x => x.Durum == true).ToList();
            Viewmodel.Adminler = db.Adminler.Where(x => x.Durum == true).ToList();
            Viewmodel.Masalar = db.Masalar.Where(x => x.DurumM == true).ToList();
            Viewmodel.Menu = db.Menu.Where(x => x.Durum == true).ToList();
            Viewmodel.Kataloglar = db.YemekKatalog.ToList();




            return View(Viewmodel);
        }




        public ActionResult adminYonet()
        {
            AdminViewModel Viewmodel = new AdminViewModel();



            return View(Viewmodel);
        }




    }
}