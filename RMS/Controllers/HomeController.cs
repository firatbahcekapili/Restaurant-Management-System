using RMS.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.AccessControl;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web.WebPages;
using System.Web.Helpers;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Data.Entity.Validation;

namespace RMS.Controllers
{


    public class HomeController : Controller
    {

        RMSEntities db = new RMSEntities();


        public ActionResult Home()
        {



            var Model = db.Masalar.Where(x => x.DurumM == true).ToList();

            foreach (var item in Model)
            {
                var liste = item.Siparisler.Where(x => x.Durum == "Aktif");
                if (liste.Any())
                {
                    item.Durum = "Dolu";

                }


            }
            try
            {
                // Kaydetme işlemi
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }


            return View(Model);





        }
        public IEnumerable<SelectListItem> KonumGetir()
        {

            var konum = db.Masalar.Where(x => x.DurumM == true).Select(x => x.Konum).Distinct().ToList();


            List<SelectListItem> list = new List<SelectListItem>();
            konum.ForEach(x => { list.Add(new SelectListItem { Value = x, Text = x }); });


            return list;
        }
        public ActionResult Masa(int? id)
        {
            MasaViewModel model = new MasaViewModel();
            model.Masalar = db.Masalar.Where(x => x.MasaId == id).ToList();
            model.Menu = db.Menu.Where(x => x.Durum == true).ToList();
            model.Masalar[0].Konumlar = KonumGetir();
            model.Siparisler = db.Siparisler.ToList();
            model.YemekKatalog = db.YemekKatalog.ToList();
            model.YemekOzellik = db.YemekOzellik.ToList();
            model.Katalog_Ozellikler = db.Katalog_Ozellik.ToList();


            if (Session["kullanici"] != null)
            {


                if (Session["kullanici"] is Personeller)
                {
                    List<Personeller> personel = new List<Personeller>();
                    personel.Add((Personeller)Session["kullanici"]);
                    model.Personeller = personel;
                }
                else if (Session["kullanici"] is Adminler)
                {
                    List<Adminler> admin = new List<Adminler>();
                    admin.Add((Adminler)Session["kullanici"]);
                    model.Adminler = admin;
                }
                else
                {

                }



            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Masa()
        {
            var Model = db.Masalar.Where(x => x.DurumM == true).ToList();




            return View(Model);
        }

        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();

            return RedirectToAction("Giris", "Giris");
        }


        public PartialViewResult AdisyonGuncelle(List<string> Veri, List<string> adet, int id)
        {
            var sipariş = db.Siparisler.Where(x => x.MasaId == id && x.Durum == "Aktif").ToList();


            if (Veri != null)
            {
                for (int i = 0; i < Veri.Count(); i++)
                {

                    if (int.Parse(adet[i]) != sipariş.Where(x => x.YemekAd.Equals(Veri[i])).Sum(x => x.Adet))
                    {

                        //Yemek ekleme işlemi
                        if (int.Parse(adet[i]) > sipariş.Where(x => x.YemekAd.Equals(Veri[i])).Sum(x => x.Adet))
                        {

                            var c = Veri[i];
                            Siparisler s = new Siparisler();
                            var a = db.Menu.FirstOrDefault(x => x.YemekAd.Equals(c) && x.Durum == true);
                            s.MasaId = id;
                            s.YemekId = a.YemekId;
                            s.YemekAd = Veri[i];
                            s.YemekFiyat = (decimal?)a.YemekFiyat;
                            s.Tarih = DateTime.Now;
                            s.Durum = "Aktif";
                            s.Adet = int.Parse(adet[i]) - sipariş.Where(x => x.YemekAd.Equals(Veri[i])).Sum(x => x.Adet);
                            var kisi = Session["kullanici"];

                            String kisiTypef = kisi.GetType().Name;
                            String[] k = kisiTypef.Split('_');
                            String kisiType = k[0];
                            if (kisiType == "Adminler")
                            {
                                var admin = Session["kullanici"] as Adminler;
                                s.AdminId = admin.AdminId;
                            }
                            else if (kisiType == "Personeller")
                            {
                                var personel = Session["kullanici"] as Personeller;
                                s.PersonelId = personel.PersonelId;

                            }
                            db.Siparisler.Add(s);
                            sipariş.Add(s);
                            db.SaveChanges();
                        }
                        //Yemek eksiltme / silme işlemi
                        else
                        {
                            int eksilt = sipariş.Where(x => x.YemekAd.Equals(Veri[i])).Sum(x => x.Adet) - int.Parse(adet[i]);

                            foreach (var item in sipariş.Where(x => x.YemekAd.Equals(Veri[i])))
                            {
                                if (eksilt == 0)
                                {

                                    break;
                                }
                                else
                                {

                                    if (item.Adet <= eksilt)
                                    {
                                        item.Durum = "Kaldırıldı";
                                        eksilt = eksilt - item.Adet;
                                    }
                                    else if (item.Adet > eksilt)
                                    {
                                        item.Adet = item.Adet - eksilt;
                                        eksilt = 0;
                                        break;
                                    }
                                }

                            }

                        }



                    }
                }

            }


            db.SaveChanges();
            var siparişGuncel = db.Siparisler.Where(x => x.MasaId == id && x.Durum == "Aktif").ToList();

            return PartialView("_AdisyonPartial", siparişGuncel);




        }

        public PartialViewResult Adisyony(int id)
        {
            var sipariş = db.Siparisler.Where(x => x.MasaId == id && x.Durum == "Aktif").ToList();
            return PartialView("_AdisyonPartial", sipariş);
        }




        public PartialViewResult Adisyon(int YemekId, string adet, int id, String YemekOzellikJson, List<String> Ozellikler)
        {
            var sipariş = db.Siparisler.Where(x => x.MasaId == id && x.Durum == "Aktif").ToList();

            JObject cleanedJson;
            string cleanedJsonString = null;

            if (!string.IsNullOrEmpty(YemekOzellikJson))
            {
                // JSON stringini JObject'a dönüştür
                JObject json = JObject.Parse(YemekOzellikJson);

                // Boş özellikleri temizle
                cleanedJson = new JObject(json.Properties()
                    .Where(p => p.Value.Type != JTokenType.Array || ((JArray)p.Value).Any(v => !string.IsNullOrEmpty(v.ToString()))));

                // Temizlenmiş JSON stringini al
                cleanedJsonString = cleanedJson.ToString();
                if (cleanedJson.ToString().Equals("{}"))
                {
                    cleanedJsonString = null;
                }
            }
            else if (Ozellikler != null)
            {


                JObject obj = null;
                // Özellikleri saklayacak bir liste oluştur
                List<string> mevcutOzellikler = new List<string>();


                foreach (var item in sipariş.Where(y => y.YemekId == YemekId))
                {
                    if (item.YemekOzellikJson != null)
                    {
                        obj = JObject.Parse(item.YemekOzellikJson);

                        foreach (var pair in obj)
                        {
                            // Grubun altında bulunan öğeleri döngüye al ve listeye ekle
                            foreach (var i in pair.Value)
                            {
                                mevcutOzellikler.Add(i.ToString());
                            }
                        }

                        if (Enumerable.SequenceEqual(mevcutOzellikler, Ozellikler))
                        {
                            cleanedJsonString = item.YemekOzellikJson;
                            break;
                        }
                    }

                    mevcutOzellikler.Clear();


                }



            }


            var yemek = db.Menu.FirstOrDefault(x => x.YemekId == YemekId && x.Durum == true);

            if (int.TryParse(adet, out int adety) && adety != 0)
            {
                if (adety > 0)
                {

                    Siparisler s = new Siparisler();

                    s.MasaId = id;
                    s.YemekId = yemek.YemekId;
                    s.YemekAd = yemek.YemekAd;
                    s.YemekFiyat = (decimal?)yemek.YemekFiyat;
                    s.Adet = adety;
                    s.YemekOzellikJson = cleanedJsonString;


                    s.Tarih = DateTime.Now;
                    s.Durum = "Aktif";

                    var kisi = Session["kullanici"];
                    if (!sipariş.Any())
                    {
                        s.AdisyonId = DateTime.Now.ToString("yyMMddHHmmss") + id;
                    }
                    else
                    {
                        s.AdisyonId = sipariş.FirstOrDefault().AdisyonId;
                    }
                    String kisiTypef = kisi.GetType().Name;
                    String[] k = kisiTypef.Split('_');
                    String kisiType = k[0];
                    if (kisiType == "Adminler")
                    {
                        var admin = Session["kullanici"] as Adminler;
                        s.AdminId = admin.AdminId;
                    }
                    else if (kisiType == "Personeller")
                    {
                        var personel = Session["kullanici"] as Personeller;
                        s.PersonelId = personel.PersonelId;

                    }

                    db.Siparisler.Add(s);
                    sipariş.Add(s);

                    db.SaveChanges();



                }
                else
                {
                    int eksilt = Math.Abs(adety);

                    foreach (var item in sipariş.Where(x => x.YemekId.Equals(YemekId)))
                    {

                        JObject obj = null;
                        // Özellikleri saklayacak bir liste oluştur
                        List<string> mevcutOzellikler = new List<string>();





                        if (!item.YemekOzellikJson.IsEmpty() && Ozellikler != null)
                        {
                            obj = JObject.Parse(item.YemekOzellikJson);
                            // JSON içindeki her özellik grubunu döngüye al
                            foreach (var pair in obj)
                            {
                                // Grubun altında bulunan öğeleri döngüye al ve listeye ekle
                                foreach (var i in pair.Value)
                                {
                                    mevcutOzellikler.Add(i.ToString());
                                }
                            }

                            var esitmi = mevcutOzellikler.OrderBy(x => x.Trim().Replace(",", "")).SequenceEqual(Ozellikler.OrderBy(x => x.Trim().Replace(",", "")));
                            if (Enumerable.SequenceEqual(mevcutOzellikler, Ozellikler))
                            {
                                if (eksilt == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    if (item.Adet <= eksilt)
                                    {
                                        item.Durum = "Kaldırıldı";
                                        eksilt = eksilt - item.Adet;

                                        db.SaveChanges();
                                    }
                                    else if (item.Adet > eksilt)
                                    {
                                        item.Adet = item.Adet - eksilt;

                                        Siparisler kaldırıldı = new Siparisler
                                        {
                                            Durum = "Kaldırıldı",
                                            Adet = eksilt,
                                            AdminId = item.AdminId,
                                            PersonelId = item.PersonelId,
                                            YemekAd = item.YemekAd,
                                            YemekFiyat = item.YemekFiyat,
                                            Tarih = DateTime.Now,
                                            YemekOzellikJson = item.YemekOzellikJson,
                                            AdisyonId = item.AdisyonId,
                                            YemekId = item.YemekId,
                                            MasaId = item.MasaId

                                        };


                                        db.Siparisler.Add(kaldırıldı);
                                        db.SaveChanges();
                                        eksilt = 0;

                                        break;
                                    }
                                }
                                db.SaveChanges();
                            }



                        }
                        else if (item.YemekOzellikJson == null && Ozellikler == null)
                        {

                            if (eksilt == 0)
                            {
                                break;
                            }
                            else
                            {
                                if (item.Adet <= eksilt)
                                {
                                    item.Durum = "Kaldırıldı";
                                    eksilt = eksilt - item.Adet;
                                    if (eksilt == 0)
                                    {
                                        break;
                                    }
                                    db.SaveChanges();
                                }
                                else if (item.Adet > eksilt)
                                {
                                    item.Adet = item.Adet - eksilt;

                                    Siparisler kaldırıldı = new Siparisler
                                    {
                                        Durum = "Kaldırıldı",
                                        Adet = eksilt,
                                        AdminId = item.AdminId,
                                        PersonelId = item.PersonelId,
                                        YemekAd = item.YemekAd,
                                        YemekFiyat = item.YemekFiyat,
                                        Tarih = DateTime.Now,
                                        YemekOzellikJson = item.YemekOzellikJson,
                                        AdisyonId = item.AdisyonId,
                                        YemekId = item.YemekId,
                                        MasaId = item.MasaId

                                    };


                                    db.Siparisler.Add(kaldırıldı);
                                    db.SaveChanges();

                                    eksilt = 0;
                                    break;
                                }
                            }
                            db.SaveChanges();

                        }




                    }


                }
            }
            else
            {

                Console.WriteLine("Geçersiz adet değeri veya adet sıfır.");
            }

            var siparişSon = sipariş.FindAll(x => x.Durum == "Aktif");

            db.SaveChanges();
            return PartialView("_AdisyonPartial", siparişSon);
        }


        public ActionResult Ode(int id)
        {

            var siparisler = db.Siparisler.Where(x => x.MasaId == id && x.Durum == "Aktif");

            var total = siparisler.Sum(x => x.YemekFiyat * x.Adet);


            siparisler.ForEach(x => { x.Durum = "Ödendi"; });
            var m = db.Masalar.FirstOrDefault(x => x.MasaId == id);
            m.Durum = "Boş";

            db.SaveChanges();


            //return RedirectToAction("Home", "HomeController");
            return RedirectToAction("Masa", "Home", new { id = id });

        }

        [HttpPost]
        public JsonResult MasaDurumDegistir(int id, String durum)
        {


            var m = db.Masalar.Find(id);


            var liste = m.Siparisler.Where(x => x.Durum == "Aktif");
            if (!liste.Any())//Aktif sipariş yoksa bu bloğa girer
            {

                if (durum == "Dolu")
                {
                    m.Durum = "Dolu";
                    db.SaveChanges();
                }
                else
                {
                    m.Durum = "Boş";
                    db.SaveChanges();
                }

            }
            else//Aktif sipariş varsa bu bloğa girer
            {
                if (durum == "Boş")
                {
                    m.Durum = "Dolu";
                    db.SaveChanges();
                }
                else
                {
                    m.Durum = "Dolu";
                    db.SaveChanges();
                }

            }

            db.SaveChanges();


            return Json(m.Durum);


        }

        [HttpPost]
        public ActionResult MasaTasi(int Masaid, int Yeniid)
        {

            var s = db.Siparisler.Where(x => x.MasaId == Masaid && x.Durum == "Aktif");



            s.ForEach(x => { x.MasaId = Yeniid; x.AdisyonId = x.AdisyonId + "->" + Yeniid; });
            var m = db.Masalar.Find(Masaid);
            m.Durum = "Boş";

            db.SaveChanges();
            return RedirectToAction("Masa", "Home", new { id = Yeniid });







        }
        [HttpPost]
        public ActionResult MasaBirlestir(int Masaid, int Yeniid)
        {

            var s = db.Siparisler.Where(x => x.MasaId == Masaid && x.Durum == "Aktif");
            var sy = db.Siparisler.Where(x => x.MasaId == Yeniid && x.Durum == "Aktif");

            s.ForEach(x => { x.MasaId = Yeniid; x.AdisyonId = x.AdisyonId + "+" + Yeniid; });
            sy.ForEach(x => { x.AdisyonId = s.FirstOrDefault().AdisyonId; });
            var m = db.Masalar.Find(Masaid);
            m.Durum = "Boş";

            db.SaveChanges();

            return RedirectToAction("Masa", "Home", new { id = Yeniid });







        }

        public PartialViewResult MasaListesi()
        {

            var liste = db.Masalar.Where(x => x.DurumM == true).ToList();
            return PartialView("_MasalarPartial", liste);
        }


    }
}