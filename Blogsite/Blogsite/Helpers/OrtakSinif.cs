using Blogsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogsite.Helpers
{
    public class OrtakSinif
    {
        BlogDB db = new BlogDB();

        public static bool EditIzinYetkiVarmi(int id,Kullanici user)
        {
            if (user.id == id)
            {

                return true;
            }
            else if (user.YetkiId > 2)
            {
                 return true;    
            }
            return false;
        }
        public static bool DeleteIzinYetkiVarmi(int id, Kullanici user)
        {
            if (user.id == id)
            {

                return true;
            }
            else if (user.YetkiId > 3)
            {
                return true;
            }
            return false;
        }

        internal static bool DeleteIzinYetkiVarmi(object kullanici)
        {
            throw new NotImplementedException();
        }
    }
}