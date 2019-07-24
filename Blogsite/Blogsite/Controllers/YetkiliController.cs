using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogsite.Controllers
{
    public class YetkiliController : Controller
    {
        // GET: Yetkili
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if(Session["username"]==null)//yetkisiz eleman içeriye giremez  
            {
                filterContext.Result = new HttpNotFoundResult();
                return;
            }
            base.OnActionExecuted(filterContext);
        }


        public ActionResult Hata(string yazilacak)
        {

            ViewBag.Yaz = yazilacak;
            return View();

        }
    }

   
}