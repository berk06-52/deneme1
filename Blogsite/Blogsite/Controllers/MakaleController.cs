using Blogsite.Helpers;
using Blogsite.Models;
using Blogsite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogsite.Controllers
{
    public class MakaleController : YetkiliController
    {
        BlogDB db = new BlogDB(); //BlogDB tıkla  alt+enter ilk seç

        public object SonAtilanMakaleViewModel { get; private set; }

        // GET: Makale
        public ActionResult Index(string Aramayap=null,int KategoriId=0)
        {
            ViewBag.KategoriId = new SelectList(db.Kategoris, "KategoriId", "KategoriAdi");
            var makaleler = from a in db.Makalelers
                            select a;

            if(KategoriId != 0)
            {
                makaleler = makaleler.Where(i => i.KategoriId == KategoriId);//Kategoriye aratma
            }

            if(!string.IsNullOrEmpty(Aramayap))//BOŞ DEİLSE
            {
                makaleler = makaleler.Where(i => i.baslik.Contains(Aramayap));//NEYİ NE GÖRE ARATMA ÖRNEK BASLİK
            }
            return View(makaleler);
        }

        // GET: Makale/Details/5
        public ActionResult Details(int id)
        {
            var makale = db.Makalelers.Where(i => i.id == id).SingleOrDefault();

            if(makale==null) //boş makale atımı önlemek
            {
                return HttpNotFound();
            }
            SonAtilanMakaleViewModel vm = new SonAtilanMakaleViewModel();
            vm.Makalem = makale;
            vm.SonMakaleler = db.Makalelers.OrderByDescending(i => i.Tarih).Take(5).ToList(); //son 5 makale alma


            return View(vm);
        }
        public ActionResult KisiMakalelerListele()
        {
            var kullaniciadi = Session["username"].ToString();
            var makaleler = db.Kullanicis.Where(a => a.KullaniciAdi == kullaniciadi).SingleOrDefault().Makalelers.ToList();
            //makaleleri bulup view yollama
            return View(makaleler);
        }

        // GET: Makale/Create
        public ActionResult Create()
        { //MAKALE YARATMA
            ViewBag.KategoriId = new SelectList(db.Kategoris, "KategoriId", "KategoriAdi");
            return View();
        }

        // POST: Makale/Create
        [HttpPost]
        public ActionResult Create(Makaleler model,string etiketler)
        {
            try
            {
                // TODO: Add insert logic here
                string kullaniciadi = Session["username"].ToString();
                var  kullanici = db.Kullanicis.Where(i => i.KullaniciAdi == kullaniciadi).SingleOrDefault();
                model.KullaniciId = kullanici.id; //kullanici atama
                model.Tarih = DateTime.Now;
                db.Makalelers.Add(model);//modele gönder
                if (!string.IsNullOrEmpty(etiketler))
                {
                    string[] etiketDizisi = etiketler.Split(',');// virgül ile ayırma
                    foreach(var i in etiketDizisi)
                    {
                        var yenietiket = new Etiket { EtiketAd = i };
                        db.Etikets.Add(yenietiket);
                        model.Etikets.Add(yenietiket);
                    }
                }

               
                db.SaveChanges();//kayıt
                return RedirectToAction("Index","Kullanici");
            }
            catch
            {
                return View();
            }
        }

        // GET: Makale/Edit/5
        public ActionResult Edit(int id)
        {
            string kullaniciadi = Session["username"].ToString(); //kullanıcı al
            var makale = db.Makalelers.Where(i => i.id == id).SingleOrDefault(); //makaleyi bul
            if(makale==null)
            {
                return HttpNotFound();
            }
            if(makale.Kullanici.KullaniciAdi==kullaniciadi)
            {
                ViewBag.KategoriId = new SelectList(db.Kategoris, "KategoriId", "KategoriAdi");
                return View(makale);

            }
            return HttpNotFound();
           
        }

        // POST: Makale/Edit/5
        [HttpPost]
        public ActionResult Edit(int id,Makaleler model)
        {
            try
            {
                var makale = db.Makalelers.Where(i => i.id == id).SingleOrDefault(); //eski makale
                makale.baslik = model.baslik;
                makale.icerik = model.icerik;
                makale.KategoriId = model.KategoriId;
                db.SaveChanges();
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var kullaniciadi = Session["username"].ToString();
                var kullanici = db.Kullanicis.Where(i => i.KullaniciAdi == kullaniciadi).SingleOrDefault();
                var makale = db.Makalelers.Where(i => i.id == id).SingleOrDefault(); //silincek makale alma
                if(kullanici.id==makale.KullaniciId)
                {

                    foreach(var i in makale.Yorums.ToList())
                    {
                        db.Yorums.Remove(i);
                    }
                    foreach (var i in makale.Etikets.ToList())
                    {
                        db.Etikets.Remove(i);
                    }
                    db.Makalelers.Remove(makale);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
                return RedirectToAction("Hata", "Yetkili", new { yazilacak = "Makale silinemedi" });
            }
                



            catch
            {

                return RedirectToAction("Hata", "Yetkili", new { yazilacak = "Makale silinemedi" });    
              
            }
        }
        public JsonResult YorumYap(string yorum ,int Makaleid)
        {

            var kullaniciadi = Session["username"].ToString();
            var kullanici = db.Kullanicis.Where(i => i.KullaniciAdi == kullaniciadi).SingleOrDefault();

            if(yorum=="")
            {
                Json(true, JsonRequestBehavior.AllowGet);
            }

            db.Yorums.Add(new Yorum { KullaniciId = kullanici.id, MakaleId = Makaleid, Tarih = DateTime.Now, YorumIcerik = yorum });
            db.SaveChanges();
            return Json(false,JsonRequestBehavior.AllowGet); //??

        }

        public ActionResult YorumDelete(int id)
        {

            try
            {

                var kullaniciadi = Session["username"].ToString();
                var kullanici = db.Kullanicis.Where(i => i.KullaniciAdi == kullaniciadi).SingleOrDefault();
                var yorum = db.Yorums.Where(i => i.id == id).SingleOrDefault(); //açık olmasın
                var makale = db.Makalelers.Where(i => i.id == yorum.MakaleId).SingleOrDefault();
                if (yorum == null)
                {

                    return RedirectToAction("Hata", "Yetkili", new { yazilacak = "Yorum silinemedi" });

                }
                if (OrtakSinif.DeleteIzinYetkiVarmi(id, kullanici) || makale.KullaniciId == kullanici.id)
                {

                    db.Yorums.Remove(yorum);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Makaleler", new { id = yorum.MakaleId });

                }

                return RedirectToAction("Hata", "Yetkili", new { yazilacak = "Yorum silinemedi" });
            }

            catch
            {
                return RedirectToAction("Hata", "Yetkili", new { yazilacak = "Yorum silinemedi" });
            }
           

        }
    }
}
