using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RMS.Models;


namespace RMS.Controllers
{
    public class MasalarController : Controller
    {


        private RMSEntities db = new RMSEntities();


        public ActionResult Index()
        {
            return View(db.Masalar.Where(x => x.DurumM == true).ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Masalar masalar = db.Masalar.Find(id);
            if (masalar == null)
            {
                return HttpNotFound();
            }
            return View(masalar);
        }

        public IEnumerable<SelectListItem> KonumGetir()
        {

            var konum = db.Masalar.Select(x => x.Konum).Distinct().ToList();

            List<SelectListItem> list = new List<SelectListItem>();
            konum.ForEach(x => { list.Add(new SelectListItem { Value = x, Text = x }); });
            list.Add(new SelectListItem { Value = "Yeni Konum Ekle", Text = "Yeni Konum Ekle" });

            return list;
        }




        public ActionResult Create()
        {
            Masalar model = new Masalar();
            model.Konumlar = KonumGetir();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MasaId,MasaAd,Kapasite,Konum,Durum")] Masalar masalar)
        {
            if (ModelState.IsValid)
            {
                masalar.DurumM = true;
                db.Masalar.Add(masalar);
                db.SaveChanges();
                return RedirectToAction("Yonetim", "admin");
            }
            else
            {
                masalar.Konumlar = KonumGetir();
                return View(masalar);
            }


        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Masalar masalar = db.Masalar.Find(id);
            masalar.Konumlar = KonumGetir();
            if (masalar == null)
            {
                return HttpNotFound();
            }
            return View(masalar);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MasaId,MasaAd,Kapasite,Konum,Durum")] Masalar masalar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(masalar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Yonetim", "admin");
            }
            return View(masalar);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Masalar masalar = db.Masalar.Find(id);
            if (masalar == null)
            {
                return HttpNotFound();
            }
            return View(masalar);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Masalar masalar = db.Masalar.Find(id);
            db.Masalar.Remove(masalar);
            db.SaveChanges();
            return RedirectToAction("Yonetim", "admin");
        }



        [Authorize]
        public ActionResult MasaDuzenle()
        {


            var Model = db.Masalar.Where(x => x.DurumM == true).ToList();
            Model[0].Konumlar = KonumGetir();

            return View();
        }




        public ActionResult MasaDuzenlePartial()
        {
            var Model = db.Masalar.Where(x => x.DurumM == true).ToList();

            Model[0].Konumlar = KonumGetir();
            return PartialView("_MasaDuzenlePartial", Model);
        }

        [HttpPost]

        public ActionResult MasaEkle([Bind(Include = "MasaAd,Kapasite,Konum,Durum")] Masalar masa)
        {


            if (ModelState.IsValid)
            {
                masa.DurumM = true;
                masa.Durum = "Boş";
                db.Masalar.Add(masa);
                db.SaveChanges();
            }

            var Model = db.Masalar.Where(x => x.DurumM == true).ToList();
            Model[0].Konumlar = KonumGetir();

            return PartialView("_MasaDuzenlePartial", Model);
        }


        public ActionResult MasaSil(int MasaId)
        {
            var masa = db.Masalar.Find(MasaId);
            masa.DurumM = false;
            db.SaveChanges();
            var Model = db.Masalar.Where(x => x.DurumM == true).ToList();
            Model[0].Konumlar = KonumGetir();
            return PartialView("_MasaDuzenlePartial", Model);
        }


        public ActionResult MasaAdDegis(int Masaid, String YeniAd)
        {
            db.Masalar.Find(Masaid).MasaAd = YeniAd;
            db.SaveChanges();
            var Model = db.Masalar.Where(x => x.DurumM == true).ToList();
            Model[0].Konumlar = KonumGetir();
            return PartialView("_MasaDuzenlePartial", Model);
        }


        public ActionResult MasaKapasiteDegis(int Masaid, int Kapasite)
        {
            db.Masalar.Find(Masaid).Kapasite = Kapasite;
            db.SaveChanges();
            var Model = db.Masalar.Where(x => x.DurumM == true).ToList();
            Model[0].Konumlar = KonumGetir();
            return PartialView("_MasaDuzenlePartial", Model);
        }

        public ActionResult MasaKonumDegis(int Masaid, String Konum)
        {
            db.Masalar.Find(Masaid).Konum = Konum;
            db.SaveChanges();
            var Model = db.Masalar.Where(x => x.DurumM == true).ToList();
            Model[0].Konumlar = KonumGetir();
            return PartialView("_MasaDuzenlePartial", Model);
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
