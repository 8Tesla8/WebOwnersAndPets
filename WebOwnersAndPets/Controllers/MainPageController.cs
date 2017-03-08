using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebOwnersAndPets.Controllers
{
    public class MainPageController : Controller
    {
        // GET: Owner
        public ActionResult Index()
        {
            return View();
        }

        // GET: Owner Pets
        public ActionResult Pets()
        {
            return View();
        }
    }
}