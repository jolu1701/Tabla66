using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tabla66.Models
{
    public class ValidateUser
    {
        public static bool IsUserValid()
        {
            HttpContext context = HttpContext.Current;
            if (context.Session["userId"] != null)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}