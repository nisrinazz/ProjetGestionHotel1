using ProjetGestionHotel1.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Generator;

namespace ProjetGestionHotel1.Controllers
{
    public class HomeController : Controller
    {
        private gestion_hotelEntities db = new gestion_hotelEntities();
        public ActionResult Index()
        {
            return View(db.categorie.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}