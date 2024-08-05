using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using RMS.Models;

namespace RMS.Controllers
{
    public class AdminlerController : Controller
    {
        private RMSEntities db = new RMSEntities();

        // GET: Adminler
        public ActionResult Index()
        {
            return View(db.Adminler.Where(x => x.Durum == true).ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adminler adminler = db.Adminler.Find(id);
            if (adminler == null)
            {
                return HttpNotFound();
            }
            return View(adminler);
        }

        // GET: Adminler/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdminId,Ad,Soyad,Cinsiyet,aKullaniciAdi,aSifre")] Adminler adminler)
        {
            if (ModelState.IsValid)
            {


                adminler.Rol = "Admin";
                adminler.Durum = true;
                db.Adminler.Add(adminler);

                db.SaveChanges();


                return RedirectToAction("Index", "Adminler");
            }

            return View(adminler);
        }

        public JsonResult KullaniciAdiKontrol(String aKullaniciAdi)
        {
            return Json(!db.Adminler.Any(a => a.aKullaniciAdi == aKullaniciAdi), JsonRequestBehavior.AllowGet);

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adminler adminler = db.Adminler.Find(id);
            if (adminler == null)
            {
                return HttpNotFound();
            }
            return View(adminler);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdminId,Ad,Soyad,Cinsiyet,Rol,aKullaniciAdi,aSifre")] Adminler adminler)
        {
            if (ModelState.IsValid)
            {

                db.Entry(adminler).State = EntityState.Modified;
                adminler.Durum = true;
                db.SaveChanges();
                return RedirectToAction("Index", "Adminler");
            }
            return View(adminler);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adminler adminler = db.Adminler.Find(id);
            if (adminler == null)
            {
                return HttpNotFound();
            }

            return View(adminler);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Adminler adminler = db.Adminler.Find(id);
            //db.Adminler.Remove(adminler);
            adminler.Durum = false;
            db.SaveChanges();
            return RedirectToAction("Index", "Adminler");
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
