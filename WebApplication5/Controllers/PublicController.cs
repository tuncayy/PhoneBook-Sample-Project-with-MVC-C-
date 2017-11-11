using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebApplication5.Controllers
{
    public class PublicController : Controller
    {
        telrehberiEntities db = new telrehberiEntities();
        // GET: Public
        public ActionResult Index()
        {
            var userdbs = db.userdbs.Include(u => u.department);

            return View(userdbs.ToList());
        }
    }
}