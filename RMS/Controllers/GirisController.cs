using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using RMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using System.Web.Security;
using System.Web.Configuration;
using Microsoft.Ajax.Utilities;

namespace RMS.Controllers
{



    [AllowAnonymous]
    public class GirisController : Controller
    {

        private RMSEntities db = new RMSEntities();
        public static adminpersonelViewModel model = new adminpersonelViewModel();

        public ActionResult Giris()
        {

            model.Personeller = db.Personeller.ToList();
            model.Adminler = db.Adminler.ToList();


            return View(model);
        }

        [HttpPost]
        public ActionResult PersonelGiris([Bind(Include = "KullaniciAdi,Sifre")] Personeller personel)

        {



            if (personel.KullaniciAdi == null || personel.Sifre == null)
            {
                ViewBag.durum = "alert-danger";
                ViewBag.message = "Kullanıcı adı ve şifre boş geçilemez";
                return View("~/Views/Giris/Giris.cshtml");
            }
            Personeller personel1 = db.Personeller.FirstOrDefault(p => p.KullaniciAdi == personel.KullaniciAdi && p.Durum == true);
            if (personel1 == null)
            {
                ViewBag.durum = "alert-danger";
                ViewBag.message = "Böyle bir kullanıcı yok.";
                return View("~/Views/Giris/Giris.cshtml");
            }
            else
            {
                if (personel.Sifre != personel1.Sifre)
                {

                    ViewBag.temp = personel.KullaniciAdi;
                    ViewBag.durum = "alert-danger";
                    ViewBag.message = "Yanlış Şifre";
                    return View("~/Views/Giris/Giris.cshtml");
                }
                else
                {



                    FormsAuthentication.SetAuthCookie(personel1.Rol, false);
                    Session["kullanici"] = personel1;
                    _Layout2(null, personel1);
                    ViewBag.durum = "alert-success";
                    ViewBag.message = "Giriş Başarılı";
                    Session["ad"] = personel1.Ad + " " + personel1.Soyad;
                    // AJAX isteği ile yapıldıysa JavaScript ile yönlendirme yap
                    if (Request.IsAjaxRequest())
                    {
                        var redirectUrl = Url.Action("Home", "Home");
                        return Json(new { redirectUrl = redirectUrl });
                    }
                    // AJAX isteği dışında (normal form postu) yapıldıysa doğrudan yönlendirme yap
                    else
                    {
                        return RedirectToAction("Home", "Home");
                    }

                }
            }
        }
        [HttpPost]
        public ActionResult AdminGiris([Bind(Include = "aKullaniciAdi,aSifre")] Adminler Adminler)
        {
            if (Adminler.aKullaniciAdi == null || Adminler.aSifre == null)
            {
                ViewBag.adurum = "alert-danger";
                ViewBag.amessage = "Kullanıcı adı ve şifre boş geçilemez";
                return View("~/Views/Giris/Giris.cshtml");
            }

            Adminler admin1 = db.Adminler.FirstOrDefault(a => a.aKullaniciAdi == Adminler.aKullaniciAdi && a.Durum == true);

            if (admin1 == null)
            {
                ViewBag.adurum = "alert-danger";
                ViewBag.amessage = "Böyle bir kullanıcı yok.";
                return View("~/Views/Giris/Giris.cshtml");
            }
            else
            {
                if (Adminler.aSifre != admin1.aSifre)
                {
                    ViewBag.atemp = Adminler.aKullaniciAdi;
                    ViewBag.adurum = "alert-danger";
                    ViewBag.amessage = "Yanlış Şifre";
                    return View("~/Views/Giris/Giris.cshtml");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(admin1.Rol, false);
                    Session["kullanici"] = admin1;
                    _Layout2(admin1, null);
                    ViewBag.adurum = "alert-success";
                    ViewBag.amessage = "Giriş Başarılı";
                    Session["ad"] = admin1.Ad + " " + admin1.Soyad;

                    // AJAX isteği ile yapıldıysa JavaScript ile yönlendirme yap
                    if (Request.IsAjaxRequest())
                    {
                        var redirectUrl = Url.Action("onlyAdmin", "admin");
                        return Json(new { redirectUrl = redirectUrl });
                    }
                    // AJAX isteği dışında (normal form postu) yapıldıysa doğrudan yönlendirme yap
                    else
                    {
                        return RedirectToAction("onlyAdmin", "admin");
                    }
                }
            }
        }


        public ActionResult _Layout2(Adminler admin, Personeller personel)
        {




            if (!Object.Equals(admin, null))
            {

                String resim = admin.AdminResim;

                if (resim != null)
                {
                    Session["resim"] = resim;

                }
                else
                {
                    if (admin.Cinsiyet == "Erkek")
                    {
                        Session["Resim"] = "/startbootstrap-sb-admin-2-gh-pages/img/DefaultMale.png";
                    }
                    else
                    {
                        Session["Resim"] = "/startbootstrap-sb-admin-2-gh-pages/img/DefaultFemale.png";
                    }

                }




                return PartialView("_kullaniciadi");
            }
            else
            {
                String ad = personel.Ad + " " + personel.Soyad;
                String resim = personel.PersonelResim;
                Session["ad"] = ad;

                if (resim != null)
                {
                    Session["resim"] = resim;

                }
                else
                {
                    if (personel.Cinsiyet == "Erkek")
                    {
                        Session["Resim"] = "/startbootstrap-sb-admin-2-gh-pages/img/DefaultMale.png";
                    }
                    else
                    {
                        Session["Resim"] = "/startbootstrap-sb-admin-2-gh-pages/img/DefaultFemale.png";
                    }

                }
                return PartialView("_kullaniciadi", ad);
            }





        }
        public ActionResult GetirAd()
        {



            var kisi = Session["kullanici"];


            String adminType = db.Adminler.GetType().GenericTypeArguments[0].Name;
            String personelType = db.Personeller.GetType().GenericTypeArguments[0].Name;

            if (kisi == null)
            {
                return RedirectToAction("ErisimYok");
            }
            else
            {
                String kisiTypef = kisi.GetType().Name;
                String[] k = kisiTypef.Split('_');
                String kisiType = k[0];
                if (kisiType == adminType)
                {
                    var admin = Session["kullanici"] as Adminler;
                    return Content(admin.Ad + " " + admin.Soyad);
                }
                else if (kisiType == personelType)
                {
                    var personel = Session["kullanici"] as Personeller;
                    return Content(personel.Ad + " " + personel.Soyad);
                }
            }


            return null;


        }




        public String GetirResim()
        {

            String resim;
            var kisi = Session["kullanici"];


            String adminType = db.Adminler.GetType().GenericTypeArguments[0].Name;
            String personelType = db.Personeller.GetType().GenericTypeArguments[0].Name;
            String kisiTypef = kisi.GetType().Name;
            String[] k = kisiTypef.Split('_');
            String kisiType = k[0];


            if (kisiType == adminType)
            {
                var admin = Session["kullanici"] as Adminler;

                resim = admin.AdminResim;
                if (resim != null)
                {
                    Session["resim"] = resim;

                }
                else
                {
                    if (admin.Cinsiyet == "Erkek")
                    {
                        Session["Resim"] = "/startbootstrap-sb-admin-2-gh-pages/img/DefaultMale.png";
                    }
                    else
                    {
                        Session["Resim"] = "/startbootstrap-sb-admin-2-gh-pages/img/DefaultFemale.png";
                    }

                }
            }
            else if (kisiType == personelType)
            {
                var personel = Session["kullanici"] as Personeller;
                resim = personel.PersonelResim;
                if (resim != null)
                {
                    Session["resim"] = resim;

                }
                else
                {
                    if (personel.Cinsiyet == "Erkek")
                    {
                        Session["Resim"] = "/startbootstrap-sb-admin-2-gh-pages/img/DefaultMale.png";
                    }
                    else
                    {
                        Session["Resim"] = "/startbootstrap-sb-admin-2-gh-pages/img/DefaultFemale.png";
                    }

                }

            }

            return Session["resim"].ToString();

        }

        public String GetirKisiResim(Adminler admin, Personeller personel)
        {
            String resim;
            if (admin.aKullaniciAdi != null)
            {
                resim = admin.AdminResim;
                if (resim != null)
                {


                }
                else
                {
                    if (admin.Cinsiyet == "Erkek")
                    {
                        resim = "/startbootstrap-sb-admin-2-gh-pages/img/DefaultMale.png";
                    }
                    else
                    {
                        resim = "/startbootstrap-sb-admin-2-gh-pages/img/DefaultFemale.png";
                    }

                }
                return resim;
            }
            else
            {
                resim = personel.PersonelResim;
                if (resim != null)
                {


                }
                else
                {
                    if (personel.Cinsiyet == "Erkek")
                    {
                        resim = "/startbootstrap-sb-admin-2-gh-pages/img/DefaultMale.png";
                    }
                    else
                    {
                        resim = "/startbootstrap-sb-admin-2-gh-pages/img/DefaultFemale.png";
                    }

                }
                return resim;
            }



        }


        public ActionResult ErisimYok()
        {
            return View();
        }


        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            Session["kullanici"] = null;

            return RedirectToAction("Giris", "Giris");
        }





    }


}