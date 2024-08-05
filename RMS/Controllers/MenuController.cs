using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.IdentityModel.Tokens;
using RMS.Models;
using RMS.Models.Managers;

namespace RMS.Controllers
{
    public class MenuController : Controller
    {
        private RMSEntities db = new RMSEntities();

        // GET: Menu
        public ActionResult Index()
        {
            return View(db.Menu.ToList());
        }

        // GET: Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }


        public IEnumerable<SelectListItem> KategoriGetir()
        {

            var kategori = db.Menu.Where(x => x.Durum == true).Select(x => x.YemekKategori).Distinct().ToList();

            List<SelectListItem> list = new List<SelectListItem>();
            kategori.ForEach(x => { list.Add(new SelectListItem { Value = x, Text = x }); });
            list.Add(new SelectListItem { Value = "Yeni Kategori Ekle", Text = "Yeni Kategori Ekle" });

            return list;
        }

        public PartialViewResult KatalogGetir()
        {

            var kataloglar = db.YemekKatalog.ToList();



            return PartialView("_KataloglarPartial", kataloglar);
        }


        public ActionResult Create()
        {
            Menu model = new Menu();
            //model.Kategoriler=KategoriGetir();

            return View(model);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "YemekId,YemekAd,YemekKategori,YemekResim, YemekFiyat")] Menu menu)
        {


            if (ModelState.IsValid)
            {
                menu.Durum = true;
                db.Menu.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Yonetim", "admin");
            }
            else
            {
                Menu m = new Menu();
                //m.Kategoriler = KategoriGetir();    
                return View(m);
            }



        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Find(id);
            menu.Kategoriler = KategoriGetir();
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        [HttpPost]
        public ActionResult Edit(int YemekId, String YemekAd, int YemekFiyat, String YemekKategori, String Kataloglar)
        {

            Menu menu = new Menu()
            {
                YemekId = YemekId,
                YemekAd = YemekAd,
                YemekKategori = YemekKategori,
                YemekFiyat = YemekFiyat,
                KatalogIdler = Kataloglar
            };



            db.Entry(menu).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Yonetim", "admin");

        }



        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        [HttpPost]
        public ActionResult YemekSil(int YemekId)
        {
            if (ModelState.IsValid)
            {
                var yemek = db.Menu.Find(YemekId);
                //var AktifSiparisler = db.Siparisler.Where(x => x.YemekId == YemekId && x.Durum == "Aktif");
                yemek.Durum = false;


                db.SaveChanges();


            }
            KatalogViewModel model = new KatalogViewModel();
            // YemekKatalog ve YemekOzellik listelerini oluştur
            model.YemekKataloglar = db.YemekKatalog.ToList();
            model.YemekOzellikler = db.YemekOzellik.ToList();
            model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();
            model.Menuler = db.Menu.Where(x => x.Durum == true).ToList();
            model.Kategoriler = KategoriGetir();
            return PartialView("_MenuPartial", model);
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
            model.Menuler = db.Menu.Where(x => x.Durum == true).ToList();
            model.Kategoriler = KategoriGetir();
            return PartialView("_MenuPartial", model);
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
            model.Menuler = db.Menu.Where(x => x.Durum == true).ToList();
            model.Kategoriler = KategoriGetir();
            return PartialView("_MenuPartial", model);
        }




        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = db.Menu.Find(id);
            menu.Durum = false;
            //db.Menu.Remove(menu);
            db.SaveChanges();
            return RedirectToAction("onlyAdmin", "admin");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public PartialViewResult Menu()
        {




            KatalogViewModel model = new KatalogViewModel();



            // YemekKatalog ve YemekOzellik listelerini oluştur
            model.YemekKataloglar = db.YemekKatalog.ToList();
            model.YemekOzellikler = db.YemekOzellik.ToList();
            model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();
            model.Menuler = db.Menu.Where(x => x.Durum == true).ToList();
            model.Kategoriler = KategoriGetir();

            return PartialView("_MenuPartial", model);

        }
        [HttpPost]
        public ActionResult KatalogDuzenle(int KatalogId, String KatalogAd)
        {


            if (ModelState.IsValid)
            {
                var katalog = db.YemekKatalog.Find(KatalogId);
                katalog.KatalogAd = KatalogAd;
                var katalogOzellikler = db.Katalog_Ozellik.Where(k => k.KatalogId == KatalogId).ToList();
                foreach (var ozellikk in katalogOzellikler)
                {
                    ozellikk.YemekKatalog.KatalogAd = KatalogAd;
                }

                db.SaveChanges();
            }
            KatalogViewModel model = new KatalogViewModel();
            // YemekKatalog ve YemekOzellik listelerini oluştur
            model.YemekKataloglar = db.YemekKatalog.ToList();
            model.YemekOzellikler = db.YemekOzellik.ToList();
            model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();
            model.Menuler = db.Menu.Where(x => x.Durum == true).ToList();
            model.Kategoriler = KategoriGetir();
            return PartialView("_MenuPartial", model);
        }
        [HttpPost]
        public ActionResult KatalogMultiChange(int KatalogId, Boolean Multi)
        {


            if (ModelState.IsValid)
            {
                var katalog = db.YemekKatalog.Find(KatalogId);
                katalog.CokluSecim = Multi;
                var katalogOzellikler = db.Katalog_Ozellik.Where(k => k.KatalogId == KatalogId).ToList();
                foreach (var ozellikk in katalogOzellikler)
                {
                    ozellikk.YemekKatalog.CokluSecim = Multi;
                }

                db.SaveChanges();
            }
            KatalogViewModel model = new KatalogViewModel();
            // YemekKatalog ve YemekOzellik listelerini oluştur
            model.YemekKataloglar = db.YemekKatalog.ToList();
            model.YemekOzellikler = db.YemekOzellik.ToList();
            model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();
            model.Menuler = db.Menu.Where(x => x.Durum == true).ToList();
            model.Kategoriler = KategoriGetir();
            return PartialView("_MenuPartial", model);
        }


        public ActionResult MenuDuzenle()
        {






            return View();

        }

        [HttpPost]
        public ActionResult YemekCreate(String YemekAd, string YemekFiyat, String YemekKategori, HttpPostedFileBase YemekResim)
        {
            string YemekResimm = null;
            if (YemekResim != null && YemekResim.ContentLength > 0)
            {



                // Resmi bir klasöre kaydetme
                var fileName = Path.GetFileName(YemekResim.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/YemekResimler/" + YemekAd + "/"));

                if (!Directory.Exists(path)) // Klasör daha önce oluşturulmamışsa
                {
                    Directory.CreateDirectory(path); // Klasörü oluştur
                    ViewBag.Message = "Yeni klasör başarıyla oluşturuldu.";
                }
                else
                {

                    ViewBag.Message = "Klasör zaten var.";
                }




                YemekResim.SaveAs(Path.Combine(Server.MapPath("~/Content/YemekResimler/" + YemekAd + "/"), fileName));
                YemekResimm = "/Content/YemekResimler/" + YemekAd + "/" + fileName;


            }
            else
            {
                YemekResimm = null;
            }



            db.SaveChanges();

            if (ModelState.IsValid)
            {
                Menu yemek = new Menu()
                {
                    YemekAd = YemekAd,
                    YemekFiyat = double.Parse(YemekFiyat.Replace(".", ",")),
                    YemekKategori = YemekKategori,
                    YemekResim = YemekResimm,
                    Durum = true
                };
                db.Menu.Add(yemek);
                db.SaveChanges();

            }




            KatalogViewModel model = new KatalogViewModel();



            // YemekKatalog ve YemekOzellik listelerini oluştur
            model.YemekKataloglar = db.YemekKatalog.ToList();
            model.YemekOzellikler = db.YemekOzellik.ToList();
            model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();
            model.Menuler = db.Menu.Where(x => x.Durum == true).ToList();
            model.Kategoriler = KategoriGetir();
            return PartialView("_MenuPartial", model);
        }






        [HttpPost]
        public ActionResult Filtrele(int YemekId)
        {
            Dictionary<string, int> list = new Dictionary<string, int>();

            var yemek = db.Menu.Find(YemekId);

            if (yemek != null && !string.IsNullOrEmpty(yemek.KatalogIdler))
            {
                var Kataloglar = db.YemekKatalog.ToList();
                var yemekKataloglar = yemek.KatalogIdler.Split(',');

                foreach (var item in yemekKataloglar)
                {
                    var katalogId = int.Parse(item);
                    var katalog = Kataloglar.FirstOrDefault(x => x.KatalogId == katalogId);

                    if (katalog != null)
                    {
                        list.Add(katalog.KatalogAd, katalog.KatalogId);
                    }
                }
            }

            return Json(list);
        }


        [HttpPost]
        public PartialViewResult KatalogKaldir(int YemekId, int KatalogId)
        {




            try
            {
                var yemek = db.Menu.Find(YemekId);






                if (!string.IsNullOrEmpty(yemek.KatalogIdler))
                {

                    var kataloglar = yemek.KatalogIdler.Split(',');


                    var yeniParcalar = kataloglar.Where(x => x != KatalogId.ToString()).ToArray();

                    string sonuc = string.Join(",", yeniParcalar);
                    yemek.KatalogIdler = sonuc;
                    db.SaveChanges();
                    Console.WriteLine(sonuc);
                }
                else
                {
                    Console.WriteLine("Girdi boş olduğu için işlem yapılmadı.");
                }




                KatalogViewModel model = new KatalogViewModel();



                // YemekKatalog ve YemekOzellik listelerini oluştur
                model.YemekKataloglar = db.YemekKatalog.ToList();
                model.YemekOzellikler = db.YemekOzellik.ToList();
                model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();
                model.Menuler = db.Menu.Where(x => x.Durum == true).ToList();
                model.Kategoriler = KategoriGetir();
                return PartialView("_MenuPartial", model);
            }
            catch (Exception ex)
            {
                KatalogViewModel model = new KatalogViewModel();



                // YemekKatalog ve YemekOzellik listelerini oluştur
                model.YemekKataloglar = db.YemekKatalog.ToList();
                model.YemekOzellikler = db.YemekOzellik.ToList();
                model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();
                model.Menuler = db.Menu.Where(x => x.Durum == true).ToList();
                model.Kategoriler = KategoriGetir();
                return PartialView("_MenuPartial", model);

            }




        }








        [HttpPost]
        public PartialViewResult KatalogEkle(int YemekId, int KatalogId)
        {




            try
            {
                if (YemekId != -1)
                {
                    var yemek = db.Menu.Find(YemekId);

                    if (string.IsNullOrEmpty(yemek.KatalogIdler))
                    {

                        yemek.KatalogIdler = KatalogId.ToString();
                        ViewBag.Durum = "Başarılı";
                        db.SaveChanges();
                    }
                    else
                    {
                        var kataloglar = yemek.KatalogIdler.Split(',');


                        if (kataloglar.Where(x => x == KatalogId.ToString()).ToArray().IsNullOrEmpty())
                        {
                            yemek.KatalogIdler = string.Join(",", kataloglar) + "," + KatalogId;
                            ViewBag.Durum = "Başarılı";
                            db.SaveChanges();

                        }
                        else
                        {
                            ViewBag.Durum = "Böyle bir kategori zaten mevcut";
                        }


                    }

                }


                KatalogViewModel model = new KatalogViewModel();



                // YemekKatalog ve YemekOzellik listelerini oluştur
                model.YemekKataloglar = db.YemekKatalog.ToList();
                model.YemekOzellikler = db.YemekOzellik.ToList();
                model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();
                model.Menuler = db.Menu.Where(x => x.Durum == true).ToList();
                model.Kategoriler = KategoriGetir();
                return PartialView("_MenuPartial", model);
            }
            catch (Exception ex)
            {
                KatalogViewModel model = new KatalogViewModel();



                // YemekKatalog ve YemekOzellik listelerini oluştur
                model.YemekKataloglar = db.YemekKatalog.ToList();
                model.YemekOzellikler = db.YemekOzellik.ToList();
                model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();
                model.Menuler = db.Menu.Where(x => x.Durum == true).ToList();
                model.Kategoriler = KategoriGetir();

                return PartialView("_MenuPartial", model);
            }




        }




        public PartialViewResult ResimKaldır(int YemekId)
        {
            var yemek = db.Menu.Find(YemekId);
            var path = Path.Combine(Server.MapPath("~" + yemek.YemekResim));
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                yemek.YemekResim = null;
                db.SaveChanges();
                ViewBag.Message = "Dosya başarıyla silindi.";
            }
            else
            {
                ViewBag.Message = "Dosya bulunamadı.";
            }
            KatalogViewModel model = new KatalogViewModel();



            // YemekKatalog ve YemekOzellik listelerini oluştur
            model.YemekKataloglar = db.YemekKatalog.ToList();
            model.YemekOzellikler = db.YemekOzellik.ToList();
            model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();
            model.Menuler = db.Menu.Where(x => x.Durum == true).ToList();
            model.Kategoriler = KategoriGetir();
            return PartialView("_MenuPartial", model);

        }


        [HttpPost]
        public PartialViewResult YemekDuzenle(int YemekId, String YemekAd, string YemekFiyat, String YemekKategori, HttpPostedFileBase YemekResim)
        {

            var yemek = db.Menu.Find(YemekId);
            yemek.YemekAd = YemekAd;
            yemek.YemekFiyat = double.Parse(YemekFiyat.Replace(".", ","));
            yemek.YemekKategori = YemekKategori;

            if (YemekResim != null && YemekResim.ContentLength > 0)
            {



                // Resmi bir klasöre kaydetme
                var fileName = Path.GetFileName(YemekResim.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/YemekResimler/" + yemek.YemekAd + "/"));

                if (!Directory.Exists(path)) // Klasör daha önce oluşturulmamışsa
                {
                    Directory.CreateDirectory(path); // Klasörü oluştur
                    ViewBag.Message = "Yeni klasör başarıyla oluşturuldu.";
                }
                else
                {

                    try
                    {
                        var dosyaYolu = Server.MapPath("~" + yemek.YemekResim);

                        if (System.IO.File.Exists(dosyaYolu))
                        {
                            System.IO.File.Delete(dosyaYolu);
                            ViewBag.Message = "Dosya başarıyla silindi.";
                        }
                        else
                        {
                            ViewBag.Message = "Dosya bulunamadı.";
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "Dosya silinirken bir hata oluştu: " + ex.Message;
                    }

                    ViewBag.Message = "Klasör zaten var.";
                }




                YemekResim.SaveAs(Path.Combine(Server.MapPath("~/Content/YemekResimler/" + yemek.YemekAd + "/"), fileName));
                yemek.YemekResim = "/Content/YemekResimler/" + yemek.YemekAd + "/" + fileName;


            }
            //else
            //{
            //    var dosyaYolu = Server.MapPath("~" + yemek.YemekResim);
            //    if (System.IO.File.Exists(dosyaYolu))
            //    {
            //        System.IO.File.Delete(dosyaYolu);
            //        yemek.YemekResim = null;
            //        ViewBag.Message = "Dosya başarıyla silindi.";
            //    }
            //    else
            //    {
            //        ViewBag.Message = "Dosya bulunamadı.";
            //    }
            //}



            db.SaveChanges();



            KatalogViewModel model = new KatalogViewModel();



            // YemekKatalog ve YemekOzellik listelerini oluştur
            model.YemekKataloglar = db.YemekKatalog.ToList();
            model.YemekOzellikler = db.YemekOzellik.ToList();
            model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();
            model.Menuler = db.Menu.Where(x => x.Durum == true).ToList();
            model.Kategoriler = KategoriGetir();
            return PartialView("_MenuPartial", model);

        }







    }



}
