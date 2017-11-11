using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebApplication5.Controllers
{
    public class LoginController : Controller
    {
        telrehberiEntities db = new telrehberiEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(userdb user, department dep)
        {
            
            if (ModelState.IsValid)
            {
                var details = (from userdb in db.userdbs
                               where userdb.ad == user.ad && userdb.password == user.password
                               select new
                               {
                                   userdb.ad,
                                   userdb.soyad,
                                   userdb.depid,
                                   userdb.isAdmin,
                                   userdb.id
                               }
                               ).ToList();

                string id = "";
                if (details.FirstOrDefault() != null)
                {
                    Session["name"] = details.FirstOrDefault().ad;
                    Session["soyad"] = details.FirstOrDefault().soyad;
                    Session["depid"] = details.FirstOrDefault().depid;
                    Session["isadmin"] = "False";
                    Session["id"] = details.FirstOrDefault().id;
                    id = details.FirstOrDefault().id.ToString();
                }
                else if(details.FirstOrDefault() == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                var yonetici = (from department in db.departments
                                where department.yonetici.ToString() == id
                                select new
                                {
                                    department.yonetici
                                }
                           ).ToList();

                if (yonetici.FirstOrDefault() != null)
                {
                    Session["depyonetici"] = yonetici.FirstOrDefault().yonetici;
                if ((Session["isadmin"].ToString() == "True") || (Session["depyonetici"].ToString() == Session["id"].ToString()))
                {
                    if(Session["name"].ToString() == user.ad.ToString())
                        return RedirectToAction("Index", "userdbs");
                    else
                        return RedirectToAction("Login","Login");

                    }
                }
                
                else 
                {
                    Session["depyonetici"] = null;
                    return RedirectToAction("Index", "Public");
                }
                
               
            
            }

            else
            {
                ModelState.AddModelError("", "Invalid Credentials");
            }
            
           
            



            return View(user);
        }


        public ActionResult ChangePass(int? id)
        {
            id = Convert.ToInt32(Session["id"].ToString());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userdb userdb = db.userdbs.Find(id);
            if (userdb == null)
            {
                return HttpNotFound();
            }
            ViewBag.depid = new SelectList(db.departments, "depID", "depName", userdb.depid);

            return View(userdb);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePass([Bind(Include = "ad,soyad,telno,depid,id,password")] userdb userdb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userdb).State = EntityState.Modified;
                db.SaveChanges();
                try {
                    if ((Session["isadmin"].ToString() == "True") || (Session["depyonetici"].ToString() != null))
                    {

                        return RedirectToAction("Index", "userdbs");

                    }
                    else
                        return RedirectToAction("Index", "Public");
                }
                catch(Exception ex)
                {
                    return RedirectToAction("Index", "Public");
                }
            }
            return View(userdb);
            }
            
        

    }
}