using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotographySiteJ.Models;

namespace PhotographySiteJ.Controllers
{
    public class GalleryController : Controller
    {
        //
        // GET: /Gallery/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SeniorPictures()
        {
            List<ImageInformation> imageList = ImageHelper.GetGallery("SeniorPictures");

            return View(imageList);
        }
        public ActionResult MaternityPictures()
        {
            List<ImageInformation> imageList = ImageHelper.GetGallery("MaternityPictures");

            return View(imageList);
        }

        public ActionResult GetSlideshowImage(string image, string gallery)
        {
            ViewData["SlideshowImagePath"] = @"/Content/Images/" + gallery + "/" + image;

            return PartialView("_Slideshow");
        }
    }
}
