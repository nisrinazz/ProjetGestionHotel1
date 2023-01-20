
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
            if (Session["user"] == null)
            { return View(); }
            else if (Session["role"] == "ADMIN")
            { return RedirectToAction("Index", "administrateur"); }
            
             return RedirectToAction("Index", "Home"); 

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
                        TempData["erreur"] = "Login or Password incorrect";
                        return View();
                    }
                    else
                    {
                        Session["user"] = admin;
                        Session["role"] = "ADMIN";
                        return RedirectToAction("Index", "administrateurs");
                    }

                }
                else
                {
                    Session["user"] = client;
                    Session["role"] = "CLIENT";
                    return RedirectToAction("Index", "Home");
                }
          
        }
        public ActionResult Logout()
        {
            if (Session["User"] != null)
            {
                Session["user"] = null;
                Session["role"] = null;
               
            }
            return RedirectToAction("Index", "Home");
        }

    }
}