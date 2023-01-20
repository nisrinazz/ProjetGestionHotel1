using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using ProjetGestionHotel1.Models;

namespace ProjetGestionHotel.Controllers
{
    public class administrateursController : Controller
    {
        private gestion_hotelEntities db = new gestion_hotelEntities();

        // GET: administrateurs
        public ActionResult Index()
        {
            if (Session["role"] == "ADMIN")
            {
                Session["nbr_client"] = db.client.Count();
                Session["comments_negative"] = db.commentaire.Where(c => c.score == 0).Count();
                Session["comments_positive"] = db.commentaire.Where(c => c.score == 1).Count();
                return View(db.administrateur.ToList());
            }

                return RedirectToAction("Index","Home");       
        }

        // GET: administrateurs/Details/5
        public ActionResult Details(int? id)
        {
            if(Session["role"] == "ADMIN")
            {
                if (id == null)
                {
                    administrateur admin = (administrateur)@Session["user"];
                    id = admin.id_admin;

                }
                administrateur administrateur = db.administrateur.Find(id);
                if (administrateur == null)
                {
                    return HttpNotFound();
                }
                return View(administrateur);
            }
            else return RedirectToAction("Index", "Home");
        }

        // GET: administrateurs/Create
        public ActionResult Create()
        { administrateur ad = (administrateur)Session["user"];
            if (ad.is_superadmin == true)
            { return View(); }
            else if( ad.is_superadmin == false)
            {
                return RedirectToAction("Index", "administrateurs");
            }
                return RedirectToAction("Index", "Home");
        }

        // POST: administrateurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "photo_profil")] administrateur administrateur)
        {
            if (ModelState.IsValid)
            {
                var checkEmailAdmin = db.administrateur.FirstOrDefault(a => a.email_admin.Equals(administrateur.email_admin));
                var checkLoginAdmin = db.administrateur.FirstOrDefault(a => a.login_admin.Equals(administrateur.login_admin));
                byte[] imageData = null;
                if (checkEmailAdmin == null && checkLoginAdmin == null)
                {
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase poImgFile = Request.Files["photo_profil"];
                        using (var binary = new BinaryReader(poImgFile.InputStream))
                        {
                            if (poImgFile.ContentLength != 0)
                                imageData = binary.ReadBytes(poImgFile.ContentLength);
                            else
                            {
                                string path = "C:\\Users\\azzai\\source\\repos\\ProjetGestionHotel1\\ProjetGestionHotel1\\Content\\images\\thisavatar.png";
                                imageData = System.IO.File.ReadAllBytes(path);
                            }

                        }
                    }
                    administrateur.is_superadmin = false;
                    administrateur.photo_profil = imageData;
                    db.administrateur.Add(administrateur);
                    db.SaveChanges();
                    return RedirectToAction("Index", "administrateurs");
                }
                else if (checkLoginAdmin != null)
                {
                    ViewBag.error = "Login already exist !";
                    return View();
                }
                else if (checkEmailAdmin != null)
                {
                    ViewBag.error = "Email already exist !";
                    return View();
                }
            }
            return View(administrateur);
        }

        // GET: administrateurs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                administrateur admin = (administrateur)@Session["user"];
                id = admin.id_admin;
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
        public ActionResult Edit(administrateur administrateur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(administrateur).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Modified successfully";
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
            else
            {
                db.administrateur.Remove(administrateur);
                db.SaveChanges();
                return RedirectToAction("Index");
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
