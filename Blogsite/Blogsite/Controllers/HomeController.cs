using Blogsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogsite.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        BlogDB db = new BlogDB();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Kullanici model)
        {
            try
            {
                var varmi = db.Kullanicis.Where(i => i.KullaniciAdi == model.KullaniciAdi).SingleOrDefault();
                if (varmi == null)
                {
                    return View();
                }
                if (varmi.Sifre == model.Sifre) //SİFRE DOGRUYSA
                {
                    Session["username"] = varmi.KullaniciAdi; //büyük küçük harf duyarlılığı için
                    return RedirectToAction("Index","Kullanici");

                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }
        // GET: Kullanici/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kullanici/Create
        [HttpPost]
        public ActionResult Create(Kullanici model) //kULLANİCİDA CTRL NOKTA İLE TANIT
        {
            try
            {   //SATIR YANINI İSARETLEYEREK GELEN BİLGİ KONTROL EDİLEBİLİR ADIM ADIM
                // TODO: Add insert logic here

                var varmi = db.Kullanicis.Where(i => i.KullaniciAdi == model.KullaniciAdi).SingleOrDefault();


                if (varmi != null)
                {
                    return View();
                }
                if (string.IsNullOrEmpty(model.Sifre))
                {
                    return View();
                }
                //eklediği yer kullaniciyi
                model.KayitTarihi = DateTime.Now;
                model.YetkiId = 1;
                db.Kullanicis.Add(model);
                db.SaveChanges();
                //oturumu açık tutan bir daha girmesin diye + birden fazla bilgi tutulabilir
                Session["username"] = model.KullaniciAdi;

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}