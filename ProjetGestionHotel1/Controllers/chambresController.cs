using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjetGestionHotel1.Models;

namespace ProjetGestionHotel1.Controllers
{
    public class chambresController : Controller
    {
        private gestion_hotelEntities db = new gestion_hotelEntities();

        // GET: chambres
        public ActionResult Index()
        {
            var chambre = db.chambre.Include(c => c.categorie);
            return View(chambre.ToList());
        }

        public ActionResult AvailableRooms()
        {
            var pquery = db.chambre.Where(ch => ch.disponibilite == "available");
            return View(pquery.ToList());
        }

        public ActionResult ChambresParCat(String cat)
        {
            if(cat == null)
            {
                return RedirectToAction("Index", "chambres");
            }
            else
            { var pquery = db.chambre.Where(ch => ch.categorie.nom_categorie == cat);
                return View(pquery.ToList());
            }
        }
        // GET: chambres/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chambre chambre = db.chambre.Find(id);
            if (chambre == null)
            {
                return HttpNotFound();
            }
            return View(chambre);
        }

        // GET: chambres/Create
        public ActionResult Create()
        {
            ViewBag.nom_categorie = new SelectList(db.categorie, "nom_categorie", "nom_categorie");
            return View();
        }

        // POST: chambres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "num_chambre,disponibilite,nom_categorie")] chambre chambre)
        {

            Session["chambre"] = chambre;
            if (ModelState.IsValid)
            {
                chambre.disponibilite = "available";
                db.chambre.Add(chambre);
                db.SaveChanges();
                return RedirectToAction("Create", "images");
            }

            ViewBag.nom_categorie = new SelectList(db.categorie, "nom_categorie", "nom_categorie", chambre.nom_categorie);
            return View(chambre);
        }

        // GET: chambres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chambre chambre = db.chambre.Find(id);
            if (chambre == null)
            {
                return HttpNotFound();
            }
            ViewBag.nom_categorie = new SelectList(db.categorie, "nom_categorie", "nom_categorie", chambre.nom_categorie);
            return View(chambre);
        }

        // POST: chambres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_chambre,num_chambre,disponibilite,nom_categorie")] chambre chambre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chambre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.nom_categorie = new SelectList(db.categorie, "nom_categorie", "nom_categorie", chambre.nom_categorie);
            return View(chambre);
        }

        // GET: chambres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chambre chambre = db.chambre.Find(id);
            if (chambre == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.chambre.Remove(chambre);
                db.SaveChanges();
                return RedirectToAction("Index", "administrateurs");
            }

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
