using Blogsite.Helpers;
using Blogsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogsite.Controllers
{
    public class KullaniciController : YetkiliController
    {
        BlogDB db = new BlogDB();

        // GET: Kullanici
        public ActionResult Index()
        { //bilgi taşıma için viewBag kullanımı
            string kullaniciadi = Session["username"].ToString();
            var kullanici = db.Kullanicis.Where(i => i.KullaniciAdi == kullaniciadi).SingleOrDefault();
         //deneme amaçlı   ViewBag.deneme = kullanici.isim; 
            return View(kullanici);
        }

        // GET: Kullanici/Details/5
        public ActionResult Details(int id)
        {   //kisi profilinlerine yollama id bulunanı viweda çıkarma
            var kisi = db.Kullanicis.Where(i => i.id ==id).SingleOrDefault();
            return View(kisi);
        }
        //Profil sağ tıkla view ekleyerek hazır eklenebiliyor 
        public ActionResult Profil()
        {   //kisi profilinlerine yollama
            string kullaniciadi = Session["username"].ToString(); //direk kullaniciadi yerine session yazmak hata veriyor
            var kisi = db.Kullanicis.Where(i => i.KullaniciAdi == kullaniciadi ).SingleOrDefault();
            return View(kisi);
        }

       
        public ActionResult Logout()
        {   //KULLANICI ÇIKIŞ YAPDIKDAN SONRA
            Session["username"] = null;
            return RedirectToAction("Index", "Home");
        }

        // GET: Kullanici/Edit/5
        public ActionResult Edit(int id)
        { string kullaniciadi = Session["username"].ToString();
            var user = db.Kullanicis.Where(i => i.KullaniciAdi == kullaniciadi).SingleOrDefault();

         if(OrtakSinif.EditIzinYetkiVarmi(id,user))
            {
                var kisi = db.Kullanicis.Where(i => i.id == id).SingleOrDefault();
                return View(kisi);
            }
           
             
            return HttpNotFound();
        }

        // POST: Kullanici/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Kullanici model)
        {
            try
            {
                var kisi = db.Kullanicis.Where(i => i.id == id).SingleOrDefault();
                //null kayıt tarihi ve yetki için
                kisi.isim = model.isim;
                kisi.soyisim = model.soyisim;
                kisi.Sifre = model.Sifre;
                db.SaveChanges();

                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Kullanici/Delete/5
        public ActionResult Delete(int id)
        {   

            return View();
        }

        // POST: Kullanici/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
