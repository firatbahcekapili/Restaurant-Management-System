using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RMS.Models;

namespace RMS.Controllers
{
    public class YemekKatalogController : Controller
    {
        private RMSEntities db = new RMSEntities();

        // GET: YemekKatalog
        public ActionResult Index()
        {


            return View(db.YemekKatalog.ToList());
        }


        public ActionResult KatalogDuzenle()
        {
            KatalogViewModel model = new KatalogViewModel();



            // YemekKatalog ve YemekOzellik listelerini oluştur
            model.YemekKataloglar = db.YemekKatalog.ToList();
            model.YemekOzellikler = db.YemekOzellik.ToList();
            model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();


            return View(model);
        }

        public PartialViewResult Ozellik()
        {


            KatalogViewModel model = new KatalogViewModel();

            // YemekKatalog ve YemekOzellik listelerini oluştur
            model.YemekKataloglar = db.YemekKatalog.ToList();
            model.YemekOzellikler = db.YemekOzellik.ToList();
            model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();

            return PartialView("_YemekOzellikPartial", model);

        }


        [HttpPost]
        public ActionResult Filtrele(int katalogId)
        {
            Dictionary<string, int> list = new Dictionary<string, int>();


            var yemekOzellikListesi = db.Katalog_Ozellik
      .Where(ko => ko.KatalogId == katalogId)
      .Select(ko => ko.YemekOzellik) // YemekOzellik tablosuna geçiş
      .ToList();
            foreach (var item in yemekOzellikListesi)
            {
                list.Add(item.Yemek_Ozellik, item.OzellikId);

            }

            return Json(list);
        }


        [HttpPost]
        public PartialViewResult OzellikKaldir(int OzellikId, int KatalogId)
        {




            try
            {
                var x = db.Katalog_Ozellik.FirstOrDefault(z => z.OzellikId == OzellikId && z.KatalogId == KatalogId);
                db.Katalog_Ozellik.Remove(x);
                db.SaveChanges();
                KatalogViewModel model = new KatalogViewModel();



                // YemekKatalog ve YemekOzellik listelerini oluştur
                model.YemekKataloglar = db.YemekKatalog.ToList();
                model.YemekOzellikler = db.YemekOzellik.ToList();
                model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();

                return PartialView("_YemekOzellikPartial", model);
            }
            catch (Exception ex)
            {
                KatalogViewModel model = new KatalogViewModel();



                // YemekKatalog ve YemekOzellik listelerini oluştur
                model.YemekKataloglar = db.YemekKatalog.ToList();
                model.YemekOzellikler = db.YemekOzellik.ToList();
                model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();

                return PartialView("_YemekOzellikPartial", model);
            }




        }


        [HttpPost]
        public PartialViewResult OzellikEkle(int OzellikId, int KatalogId)
        {




            try
            {
                var x = db.YemekOzellik.FirstOrDefault(z => z.OzellikId == OzellikId);

                if (db.Katalog_Ozellik.FirstOrDefault(z => z.KatalogId == KatalogId && z.OzellikId == OzellikId) == null)
                {
                    Katalog_Ozellik ko = new Katalog_Ozellik()
                    {
                        KatalogId = KatalogId,
                        OzellikId = OzellikId
                    };
                    db.Katalog_Ozellik.Add(ko);
                    db.SaveChanges();
                    ViewBag.EklemeDurumu = "Başarılı";



                }
                else
                {
                    ViewBag.EklemeDurumu = "Bu özellik zaten ekli!";

                }

                KatalogViewModel model = new KatalogViewModel();



                // YemekKatalog ve YemekOzellik listelerini oluştur
                model.YemekKataloglar = db.YemekKatalog.ToList();
                model.YemekOzellikler = db.YemekOzellik.ToList();
                model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();

                return PartialView("_YemekOzellikPartial", model);
            }
            catch (Exception ex)
            {
                KatalogViewModel model = new KatalogViewModel();



                // YemekKatalog ve YemekOzellik listelerini oluştur
                model.YemekKataloglar = db.YemekKatalog.ToList();
                model.YemekOzellikler = db.YemekOzellik.ToList();
                model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();

                return PartialView("_YemekOzellikPartial", model);
            }




        }



        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YemekKatalog yemekKatalog = db.YemekKatalog.Find(id);
            if (yemekKatalog == null)
            {
                return HttpNotFound();
            }
            return View(yemekKatalog);
        }


        public ActionResult Create()
        {
            return View();
        }

        // POST: YemekKatalog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KatalogId,KatalogAd")] YemekKatalog yemekKatalog)
        {
            if (ModelState.IsValid)
            {
                db.YemekKatalog.Add(yemekKatalog);
                db.SaveChanges();

            }

            return View(yemekKatalog);
        }

        [HttpPost]
        public ActionResult KatalogCreate(String KatalogAd)
        {
            if (ModelState.IsValid)
            {
                YemekKatalog katalog = new YemekKatalog()
                {
                    KatalogAd = KatalogAd
                };
                db.YemekKatalog.Add(katalog);
                db.SaveChanges();

            }
            KatalogViewModel model = new KatalogViewModel();



            // YemekKatalog ve YemekOzellik listelerini oluştur
            model.YemekKataloglar = db.YemekKatalog.ToList();
            model.YemekOzellikler = db.YemekOzellik.ToList();
            model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();

            return PartialView("_YemekOzellikPartial", model);
        }

        [HttpPost]
        public ActionResult KatalogDelete(int KatalogId)
        {
            if (ModelState.IsValid)
            {
                var katalog = db.YemekKatalog.Find(KatalogId);

                // Manuel olarak ilgili kayıtları siliyoruz
                var katalogOzellikler = db.Katalog_Ozellik.Where(k => k.KatalogId == KatalogId).ToList();
                foreach (var ozellik in katalogOzellikler)
                {
                    db.Katalog_Ozellik.Remove(ozellik);
                }

                db.YemekKatalog.Remove(katalog);
                db.SaveChanges();

            }
            KatalogViewModel model = new KatalogViewModel();
            // YemekKatalog ve YemekOzellik listelerini oluştur
            model.YemekKataloglar = db.YemekKatalog.ToList();
            model.YemekOzellikler = db.YemekOzellik.ToList();
            model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();

            return PartialView("_YemekOzellikPartial", model);
        }


        [HttpPost]
        public ActionResult OzellikCreate(String OzellikAd)
        {
            if (ModelState.IsValid)
            {
                YemekOzellik ozellik = new YemekOzellik()
                {
                    Yemek_Ozellik = OzellikAd
                };
                db.YemekOzellik.Add(ozellik);
                db.SaveChanges();

            }
            KatalogViewModel model = new KatalogViewModel();



            // YemekKatalog ve YemekOzellik listelerini oluştur
            model.YemekKataloglar = db.YemekKatalog.ToList();
            model.YemekOzellikler = db.YemekOzellik.ToList();
            model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();

            return PartialView("_YemekOzellikPartial", model);
        }

        [HttpPost]
        public ActionResult OzellikDelete(int OzellikId)
        {
            if (ModelState.IsValid)
            {
                var ozellik = db.YemekOzellik.Find(OzellikId);

                // Manuel olarak ilgili kayıtları siliyoruz
                var katalogOzellikler = db.Katalog_Ozellik.Where(k => k.OzellikId == OzellikId).ToList();
                foreach (var ozellikk in katalogOzellikler)
                {
                    db.Katalog_Ozellik.Remove(ozellikk);
                }

                db.YemekOzellik.Remove(ozellik);
                db.SaveChanges();

            }
            KatalogViewModel model = new KatalogViewModel();
            // YemekKatalog ve YemekOzellik listelerini oluştur
            model.YemekKataloglar = db.YemekKatalog.ToList();
            model.YemekOzellikler = db.YemekOzellik.ToList();
            model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();

            return PartialView("_YemekOzellikPartial", model);
        }


        [HttpPost]
        public ActionResult OzellikDuzenle(int OzellikId, String OzellikAd)
        {


            if (ModelState.IsValid)
            {
                var ozellik = db.YemekOzellik.Find(OzellikId);
                ozellik.Yemek_Ozellik = OzellikAd;
                var katalogOzellikler = db.Katalog_Ozellik.Where(k => k.OzellikId == OzellikId).ToList();
                foreach (var ozellikk in katalogOzellikler)
                {
                    ozellikk.YemekOzellik.Yemek_Ozellik = OzellikAd;
                }

                db.SaveChanges();
            }
            KatalogViewModel model = new KatalogViewModel();
            // YemekKatalog ve YemekOzellik listelerini oluştur
            model.YemekKataloglar = db.YemekKatalog.ToList();
            model.YemekOzellikler = db.YemekOzellik.ToList();
            model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();

            return PartialView("_YemekOzellikPartial", model);
        }

        [HttpPost]
        public ActionResult KatalogDuzenle(int KatalogId, String KatalogAd)
        {


            if (ModelState.IsValid)
            {
                var katalog = db.YemekKatalog.Find(KatalogId);
                katalog.KatalogAd = KatalogAd;
                var katalogOzellikler = db.Katalog_Ozellik.Where(k => k.KatalogId == KatalogId).ToList();
                foreach (var katalogg in katalogOzellikler)
                {
                    katalogg.YemekKatalog.KatalogAd = KatalogAd;
                }

                db.SaveChanges();
            }
            KatalogViewModel model = new KatalogViewModel();
            // YemekKatalog ve YemekOzellik listelerini oluştur
            model.YemekKataloglar = db.YemekKatalog.ToList();
            model.YemekOzellikler = db.YemekOzellik.ToList();
            model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();

            return PartialView("_YemekOzellikPartial", model);
        }


        public IEnumerable<SelectListItem> OzellikGetir()
        {

            var ozellikler = db.YemekOzellik.ToList();

            List<SelectListItem> list = new List<SelectListItem>();
            ozellikler.ForEach(x => { list.Add(new SelectListItem { Value = x.Yemek_Ozellik, Text = x.Yemek_Ozellik }); });
            list.Add(new SelectListItem { Value = "Yeni Özellik Ekle", Text = "Yeni Özellik Ekle" });

            return list;
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YemekKatalog yemekKatalog = db.YemekKatalog
     .Include(y => y.Katalog_Ozellik)  // Include ile ilişkili verileri sorguya dahil et
     .FirstOrDefault(y => y.KatalogId == id);

            if (yemekKatalog != null)
            {
                var katalogOzellikList = yemekKatalog.Katalog_Ozellik.ToList();

            }
            if (yemekKatalog == null)
            {
                return HttpNotFound();
            }
            return View(yemekKatalog);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KatalogId,KatalogAd")] YemekKatalog yemekKatalog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yemekKatalog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yemekKatalog);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YemekKatalog yemekKatalog = db.YemekKatalog.Find(id);
            if (yemekKatalog == null)
            {
                return HttpNotFound();
            }
            return View(yemekKatalog);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            YemekKatalog yemekKatalog = db.YemekKatalog.Find(id);
            db.YemekKatalog.Remove(yemekKatalog);
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
