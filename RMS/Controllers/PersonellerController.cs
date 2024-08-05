using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RMS.Models;

namespace RMS.Controllers
{
    public class PersonellerController : Controller
    {
        private RMSEntities db = new RMSEntities();


        public ActionResult Index()
        {
            return View(db.Personeller.Where(x => x.Durum == true).ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personeller personeller = db.Personeller.Find(id);
            if (personeller == null)
            {
                return HttpNotFound();
            }
            return View(personeller);
        }


        public ActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonelId,Ad,Soyad,Maaş,Cinsiyet,KullaniciAdi,Sifre")] Personeller personeller)
        {
            if (ModelState.IsValid)
            {
                personeller.Rol = "Personel";
                personeller.Durum = true;
                db.Personeller.Add(personeller);
                db.SaveChanges();
                return RedirectToAction("Index", "Personeller");
            }

            return View(personeller);
        }

        public JsonResult KullaniciAdiKontrol(String KullaniciAdi)
        {
            return Json(!db.Personeller.Any(a => a.KullaniciAdi == KullaniciAdi), JsonRequestBehavior.AllowGet);

        }



        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personeller personeller = db.Personeller.Find(id);
            if (personeller == null)
            {
                return HttpNotFound();
            }
            return View(personeller);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonelId,Ad,Soyad,Maaş,Cinsiyet,Rol,KullaniciAdi,Sifre,Rol")] Personeller personeller)
        {
            if (ModelState.IsValid)
            {

                db.Entry(personeller).State = EntityState.Modified;
                personeller.Durum = true;
                db.SaveChanges();
                return RedirectToAction("Index", "Personeller");
            }
            return View(personeller);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personeller personeller = db.Personeller.Find(id);
            if (personeller == null)
            {
                return HttpNotFound();
            }
            return View(personeller);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Personeller personeller = db.Personeller.Find(id);
            //db.Personeller.Remove(personeller);
            personeller.Durum = false;
            db.SaveChanges();
            return RedirectToAction("Index", "Personeller");
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
