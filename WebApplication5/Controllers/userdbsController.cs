using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication5;

namespace WebApplication5.Controllers
{
    public class userdbsController : Controller
    {
        private telrehberiEntities db = new telrehberiEntities();

        // GET: userdbs
        public ActionResult Index()
        {
            if (Session["id"] != null)
            {
                var userdbs = db.userdbs.Include(u => u.department);
                return View(userdbs.ToList());
            }
            return RedirectToAction("Index", "Public");
        }

        // GET: userdbs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userdb userdb = db.userdbs.Find(id);
            if (userdb == null)
            {
                return HttpNotFound();
            }
            return View(userdb);
        }

        // GET: userdbs/Create
        public ActionResult Create()
        {
            ViewBag.depid = new SelectList(db.departments, "depID", "depName");
            return View();
        }

        // POST: userdbs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ad,soyad,telno,depid,id,isAdmin,password")] userdb userdb)
        {
            if (ModelState.IsValid)
            {
                db.userdbs.Add(userdb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.depid = new SelectList(db.departments, "depID", "depName", userdb.depid);
            return View(userdb);
        }

        // GET: userdbs/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            { 
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

            if (Session["depid"].ToString() == userdb.depid.ToString())
            {
                return View(userdb);
            }
            else
            {
                ModelState.AddModelError("", "Invalid Credentials");
            }
            return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        // POST: userdbs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ad,soyad,telno,depid,id,isAdmin,password")] userdb userdb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userdb).State = EntityState.Modified;
                db.SaveChanges();
                Session["depid"] = userdb.depid.ToString();
                return RedirectToAction("Index");
            }
            ViewBag.depid = new SelectList(db.departments, "depID", "depName", userdb.depid);
            if (Session["depid"].ToString() == userdb.depid.ToString())
            {
                return View(userdb);
            }
            else
            {
                ModelState.AddModelError("", "Invalid Credentials");
            }
            return RedirectToAction("Index");
        }

        // GET: userdbs/Delete/5
        public ActionResult Delete(int? id)
        {
            try { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userdb userdb = db.userdbs.Find(id);
            if (userdb == null)
            {
                return HttpNotFound();
            }
            if (Session["depid"].ToString() == userdb.depid.ToString())
            {
                return View(userdb);
            }
            else
            {
                ModelState.AddModelError("", "Invalid Credentials");
            }
            return RedirectToAction("Index");
        }
            catch(Exception e)
            {
                return RedirectToAction("Login","Login");
    }
}

        // POST: userdbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try {
                userdb userdb = db.userdbs.Find(id);
                db.userdbs.Remove(userdb);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index");
            }
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
