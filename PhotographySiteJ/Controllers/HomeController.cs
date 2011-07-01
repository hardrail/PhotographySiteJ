using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PhotographySiteJ.Models;

namespace PhotographySiteJ.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Jen's photography page!";

            List<ImageInformation> imageList = ImageHelper.GetGallery("FrontGallery");

            return View(imageList);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
