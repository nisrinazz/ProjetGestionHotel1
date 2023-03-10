using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ProjetGestionHotel1.Models;
using System.Web.UI.WebControls;


namespace ProjetGestionHotel.Controllers
{
    public class clientsController : Controller
    {
        private gestion_hotelEntities db = new gestion_hotelEntities();

        // GET: clients
        public ActionResult Index()
        {
            return View(db.client.ToList());
        }

        // GET: clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                client cl = (client)@Session["user"];
                id = cl.id_client;
            }
            client client = db.client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }




        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Exclude = "photo_profil")] client client)
        {

            if (ModelState.IsValid)
            {
                var checkEmailClient = db.client.FirstOrDefault(c => c.email_client.Equals(client.email_client));
                var checkLoginClient = db.client.FirstOrDefault(c => c.login_client.Equals(client.login_client));
                var checkEmailAdmin = db.administrateur.FirstOrDefault(a => a.email_admin.Equals(client.email_client));
                var checkLoginAdmin = db.administrateur.FirstOrDefault(a => a.login_admin.Equals(client.login_client));


                if (checkLoginClient == null && checkEmailClient == null && checkEmailAdmin == null && checkLoginAdmin == null)
                {
                    byte[] imageData = null;
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

                    client.photo_profil = imageData;
                    db.client.Add(client);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }
                else if (checkLoginClient != null || checkLoginAdmin != null)
                {
                    TempData["erreur-login"] = "Login already exist !";
                    return View();
                }
                else if (checkEmailClient != null || checkEmailAdmin != null)
                {
                    TempData["erreur-email"] = "Email already exist !";
                    return View();
                }
            }

            return View(client);
        }


        // GET: clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                client cl = (client)@Session["user"];
                id = cl.id_client;
            }
            client client = db.client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }


        // POST: clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_client,nom_client,prenom_client,email_client,tel_client,login_client,mdp_client,photo_profil")] client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            client client = db.client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            client client = db.client.Find(id);
            db.client.Remove(client);
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
