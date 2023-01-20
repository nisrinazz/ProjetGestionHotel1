using ProjetGestionHotel1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetGestionHotel1.Controllers
{
    public class CategorieController : Controller
    {

        private gestion_hotelEntities db = new gestion_hotelEntities();
        // GET: Categorie

        public ActionResult CategorieListe()
        {
            return View(db.categorie.ToList());
        }

        public ActionResult CategorieListeHome()
        {
            return View(db.categorie.ToList());
        }
    }
}