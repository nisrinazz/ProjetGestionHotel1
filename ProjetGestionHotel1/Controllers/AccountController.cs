
using ProjetGestionHotel1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetGestionHotel.Controllers
{
    public class AccountController : Controller
    {
        private gestion_hotelEntities db = new gestion_hotelEntities();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        public client getClient(String login, String mdp)
        {
            return db.client.Where(p => p.login_client == login && p.mdp_client == mdp).FirstOrDefault();
        }
        public administrateur getAdministrateur(String login, String mdp)
        {
            return db.administrateur.Where(a => a.login_admin == login && a.mdp_admin == mdp).FirstOrDefault();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(String login, String mdp)
        {
            login = Request.Form["login"];
            mdp = Request.Form["password"];
            client client = getClient(login, mdp);

            if (client == null)
            {
                administrateur admin = getAdministrateur(login, mdp);
                if (admin == null)
                {
                    TempData["message"] = "Login or password incorrect";
                    return View();
                }
                else
                {
                    Session["user"] = admin;
                    TempData["message"] = "Authentifié autant que admin";


                }

            }
            else
            {
                Session["user"] = client;
                client a = (client)Session["user"];
                TempData["message"] = "Authentifié autant que client";

            }

            return RedirectToAction("Res", "Account");


        }

        public ActionResult Res()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Login", "Account");
        }

    }
}