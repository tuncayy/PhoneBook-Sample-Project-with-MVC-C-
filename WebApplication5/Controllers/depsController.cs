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
    public class depsController : Controller
    {
        private telrehberiEntities db = new telrehberiEntities();

        // GET: deps
        public ActionResult Index()
        {
            var departments = db.departments.Include(d => d.userdb);
            return View(departments.ToList());
        }

        // GET: deps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            department department = db.departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: deps/Create
        public ActionResult Create()
        {
            ViewBag.yonetici = new SelectList(db.userdbs, "id", "ad");
            return View();
        }

        // POST: deps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "depID,depName,yonetici")] department department)
        {
            if (ModelState.IsValid)
            {
                db.departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.yonetici = new SelectList(db.userdbs, "id", "ad", department.yonetici);
            return View(department);
        }

        // GET: deps/Edit/5
        public ActionResult Edit(int? id)
        {
            try {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                department department = db.departments.Find(id);
                if (department == null)
                {
                    return HttpNotFound();
                }
                ViewBag.yonetici = new SelectList(db.userdbs, "id", "ad", department.yonetici);
                if (Session["depyonetici"].ToString() != null)
                {
                    return View(department);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        // POST: deps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "depID,depName,yonetici")] department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.yonetici = new SelectList(db.userdbs, "id", "ad", department.yonetici);
            return View(department);
        }

        // GET: deps/Delete/5
        public ActionResult Delete(int? id)
        {
            try { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            department department = db.departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            if (Session["depyonetici"].ToString() != null)
            {
                return View(department);
            }
            else
            {
                ModelState.AddModelError("", "Invalid Credentials");
            }
            return RedirectToAction("Index");
        }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
}

        // POST: deps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            department department = db.departments.Find(id);
           userdb user = db.userdbs.Find(id);
          

          
            var query = (from u in db.userdbs
                                        join d in db.departments
                                        on  u.depid 
                                        equals d.depID
                                        select new {u.ad }).ToList();

            if (query.FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "bu departmanda kayıtlı kullanıcı olduğu için silemezsiniz");
            }
            else
            {
                    db.departments.Remove(department);
                  db.SaveChanges();
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
