using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjetGestionHotel1.Models;

namespace ProjetGestionHotel.Controllers
{
    public class administrateursController : Controller
    {
        private gestion_hotelEntities db = new gestion_hotelEntities();

        // GET: administrateurs
        public ActionResult Index()
        {
            /*  var administrateurModel = db.administrateur.ToList();
              var reservationModel = db.reservation.ToList();
              var clientModel = db.client.ToList();
              var commentaireModel = db.commentaire.ToList();

              var allModels = new indexViewModel { administrateurModelObject = administrateurModel,
                  clientModelObject= clientModel,
                  reservationModelObject= reservationModel,
                  commentaireModelObject= commentaireModel
              };*/
            return View();
        }

        // GET: administrateurs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            administrateur administrateur = db.administrateur.Find(id);
            if (administrateur == null)
            {
                return HttpNotFound();
            }
            return View(administrateur);
        }

        // GET: administrateurs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: administrateurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_admin,nom_admin,prenom_admin,email_admin,tel_admin,login_admin,mdp_admin,is_superadmin")] administrateur administrateur)
        {
            if (ModelState.IsValid)
            {
                db.administrateur.Add(administrateur);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(administrateur);
        }

        // GET: administrateurs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            administrateur administrateur = db.administrateur.Find(id);
            if (administrateur == null)
            {
                return HttpNotFound();
            }
            return View(administrateur);
        }

        // POST: administrateurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_admin,nom_admin,prenom_admin,email_admin,tel_admin,login_admin,mdp_admin,is_superadmin")] administrateur administrateur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(administrateur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(administrateur);
        }

        // GET: administrateurs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            administrateur administrateur = db.administrateur.Find(id);
            if (administrateur == null)
            {
                return HttpNotFound();
            }
            return View(administrateur);
        }

        // POST: administrateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            administrateur administrateur = db.administrateur.Find(id);
            db.administrateur.Remove(administrateur);
            db.SaveChanges();
            return RedirectToAction("Index");
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
