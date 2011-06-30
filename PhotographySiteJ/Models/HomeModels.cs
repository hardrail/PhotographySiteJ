using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using PhotographySiteJ.Models;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace PhotographySiteJ.Models
{
    public class ImageInformation
    {
        public string ThumbnailPath { get; set; }
        public string ActualPath { get; set; }
        public string FileName { get; set; }
    }

    public class GalleryModel
    {
        public List<ImageInformation> Images { get; set; }
    }
    public abstract class ImageHelper
    {
        private const int thumbnailheight = 60, thumbnailWidth = 60, reducedWidth = 600, reducedHeight = 480;

        public static bool ImageExists(string imagePath)
        {
            return File.Exists(imagePath);
        }

        public static string GetGalleryName(string imagePath)
        {
            return imagePath.Split('\\').GetValue(imagePath.Split('\\').Length - 2).ToString();
        }

        public static List<ImageInformation> GetGallery(string GalleryName)
        {
            List<ImageInformation> images = new List<ImageInformation>();

            if (GalleryName != null && GalleryName != "all")
            {
                images = Directory.GetFiles(HttpContext.Current.Server.MapPath(@"/Content/Images/" + GalleryName), "*", SearchOption.AllDirectories).Select(f =>
                    new ImageInformation
                    {
                        FileName = Path.GetFileName(f),
                        ThumbnailPath = @"../../Content/Thumbnails/" + Path.GetFileName(f),
                        ActualPath = @"../../Content/Images/" + GalleryName + "/" + Path.GetFileName(f),
                    }).ToList();
            }
            else
            {
                images = Directory.GetFiles(HttpContext.Current.Server.MapPath(@"/Content/Images"), "*", SearchOption.AllDirectories).Select(f =>
                    new ImageInformation
                    {
                        FileName = Path.GetFileName(f),
                        ThumbnailPath = @"../../Content/Thumbnails/" + Path.GetFileName(f),
                        ActualPath = @"../../Content/Images/" + ImageHelper.GetGalleryName(f) + "/" + Path.GetFileName(f),
                    }).ToList();
            }

            return images;
        }

        public static void CreateThumbnail(string imagePath)
        {
            //Create new image instance with pre-defined thumbnail sizes
            Image imageToThumb = Image.FromFile(imagePath).GetThumbnailImage(thumbnailWidth, thumbnailheight, null, IntPtr.Zero);
            //Create file stream
            FileStream fil = new FileStream(HttpContext.Current.Server.MapPath(@"/Content/Thumbnails/" + Path.GetFileName(imagePath)), FileMode.Create);
            //Save the thumbnail
            imageToThumb.Save(fil, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        public static void CreateReducedSizeImage(string imagePath)
        {
            Image reducedImage = Image.FromFile(imagePath).GetThumbnailImage(reducedWidth,reducedHeight, null, IntPtr.Zero);

            FileStream fil = new FileStream(HttpContext.Current.Server.MapPath(@"/Content/ReducedSizeImages/" + Path.GetFileName(imagePath)), FileMode.Create);

            reducedImage.Save(fil, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }

    public class MessageModel
    {
        public string TitleText { get; set; }
        public string MessageText { get; set; }
    }
}