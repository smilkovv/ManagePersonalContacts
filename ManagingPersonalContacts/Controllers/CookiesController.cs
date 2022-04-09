using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagingPersonalContacts.Models;

namespace ManagingPersonalContacts.Controllers
{
    public class CookiesController : Controller
    {
        //
        // GET: /Cookies/
        /*
        public ActionResult Index()
        {
            return View();
        }
        */

        public ActionResult ChangeTheme(bool? value)
        {

            HttpCookie PersonalContactsCookies = new HttpCookie(Cookies.ManagingPersonalContactsTheme);
            PersonalContactsCookies.Value = value.ToString();
            PersonalContactsCookies.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(PersonalContactsCookies);



            return RedirectToAction("Index", "Home");
        }



	}
}