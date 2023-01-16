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
    public class commentairesController : Controller
    {
        private gestion_hotelEntities db = new gestion_hotelEntities();

        // GET: commentaires
        public ActionResult Index()
        {
            var commentaire = db.commentaire.Include(c => c.client);
            return View(commentaire.ToList());
        }

        // GET: commentaires/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            commentaire commentaire = db.commentaire.Find(id);
            if (commentaire == null)
            {
                return HttpNotFound();
            }
            return View(commentaire);
        }

        // GET: commentaires/Create
        public ActionResult Create()
        {
            ViewBag.id_client = new SelectList(db.client, "id_client", "nom_client");
            return View();
        }

        // POST: commentaires/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_commentaire,id_client,text_commentaire,score,prediction")] commentaire commentaire)
        {
            if (ModelState.IsValid)
            {
                db.commentaire.Add(commentaire);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_client = new SelectList(db.client, "id_client", "nom_client", commentaire.id_client);
            return View(commentaire);
        }

        // GET: commentaires/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            commentaire commentaire = db.commentaire.Find(id);
            if (commentaire == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_client = new SelectList(db.client, "id_client", "nom_client", commentaire.id_client);
            return View(commentaire);
        }

        // POST: commentaires/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_commentaire,id_client,text_commentaire,score,prediction")] commentaire commentaire)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commentaire).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_client = new SelectList(db.client, "id_client", "nom_client", commentaire.id_client);
            return View(commentaire);
        }

        // GET: commentaires/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            commentaire commentaire = db.commentaire.Find(id);
            if (commentaire == null)
            {
                return HttpNotFound();
            }
            return View(commentaire);
        }

        // POST: commentaires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            commentaire commentaire = db.commentaire.Find(id);
            db.commentaire.Remove(commentaire);
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
