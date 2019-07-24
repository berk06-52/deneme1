using Blogsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogsite.ViewModels
{
    public class SonAtilanMakaleViewModel
    {
        public Makaleler Makalem { get; set; } // prob çift tab??? otomatik tamamlama

        public List<Makaleler> SonMakaleler { get; set; }

    }
}