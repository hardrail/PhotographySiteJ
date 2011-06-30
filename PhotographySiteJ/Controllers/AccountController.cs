using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using PhotographySiteJ.Models;
using System.IO;

namespace PhotographySiteJ.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult Admin(string Gallery)
        {
            AdminModel adm = new AdminModel();
            adm.ImageInformationModel = ImageHelper.GetGallery(Gallery);

            List<string> DistinctGalleries = new List<string>();
            //Add a selection for "all". While this is not an actual gallery, it will allow the user to view all of the galleris at one
            DistinctGalleries.Add("all");
            DistinctGalleries.AddRange(Directory.GetDirectories(Server.MapPath(@"/Content/Images")).Select(m => Path.GetFileName(m)).ToList());      
            
            ViewData["DistinctGalleries"] = DistinctGalleries;
            ViewData["UploadGalleries"] = Directory.GetDirectories(Server.MapPath(@"/Content/Images")).Select(m => Path.GetFileName(m)).ToList();
            ViewData["SelectedGallery"] = Gallery;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Admin", adm);
            }
            else
            {
                return View(adm);
            }
        }

        public JsonResult FilterTable(string Gallery)
        {
            var data = new 
            {
                results = Directory.GetFiles(Server.MapPath(@"/Content/Images/" + Gallery)).Select(
                m =>
                new {
                    FileName = Path.GetFileName(m),
                    ThumbnailPath = @"../../Content/Thumbnails/" + Path.GetFileName(m),
                    ActualPath = "@../../Content/Images/" + Gallery + "/" + Path.GetFileName(m)
                })
            };

            return Json(data, JsonRequestBehavior.AllowGet);            
        }

        public ActionResult Contact()
        {
            Contact contact = new Contact();
            return View(contact);
        }

        [HttpPost]
        public ActionResult Contact(Contact correspondence)
        {
            if (ModelState.IsValid)
            {
                var email = correspondence.ContactEmail;
                return Redirect("/");
            }

            return View(correspondence);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public void DeleteImage(string id)
        {
            //Delete the full-size image
            System.IO.File.Delete(Server.MapPath(id));
            //Delete the thumbnail
            System.IO.File.Delete(Server.MapPath(@"/Content/Thumbnails/" + Path.GetFileName(id)));
        }

        [HttpPost]
        public ActionResult UploadImages(AdminModel adm)
        {
            string gallery = @"/Content/Images/" + adm.UploadImageModel.Gallery + "/";

            for (int i = 0; i < Request.Files.Count; i++)
            {
                string savePath = Path.Combine(gallery, Path.GetFileName(Request.Files[i].FileName));

                if (ImageHelper.ImageExists(Server.MapPath(savePath)))
                {
                    throw new Exception("Woops");
                }
                
                //Save the full size image file in the selected gallery folder
                Request.Files[i].SaveAs(Server.MapPath(savePath));
                //Save the thumbnail version of the file in the thumbnails folder
                ImageHelper.CreateThumbnail(Server.MapPath(savePath));

                //Save the reduced size version to the appropriate folder
                ImageHelper.CreateReducedSizeImage(Server.MapPath(savePath));
            }

            MessageModel message = new MessageModel();
            message.TitleText = "Images uploaded successfully.";
            message.MessageText = "Please give the uploaded images about 20 seconds to register on the server. During this time " +
                                  "you may see a little red (x), but the images should appear quickly.";
            return View("Message", message);
        }

        public ActionResult SeniorPictures()
        {
            List<ImageInformation> seniorImages = Directory.GetFiles(Server.MapPath(@"/Content/Images/SeniorPictures"), "*", SearchOption.TopDirectoryOnly).Select(f =>
                new ImageInformation
                {
                    FileName = Path.GetFileName(f),
                    ThumbnailPath = @"../../Content/Thumbnails/" + Path.GetFileName(f),
                    ActualPath = @"../../Content/Images/" + f.Split('\\').GetValue(f.Split('\\').Length - 2) + "/" + Path.GetFileName(f),
                }).ToList();
            return View(seniorImages);
        }

        public ActionResult ajaxFileExists(string gallery, string file)
        {
            string contentPath = @"/Content/Images/" + gallery + "/" + Path.GetFileName(file);


            var returnJ = new { toggle = System.IO.File.Exists(Server.MapPath(contentPath)) };


            return Json(returnJ, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Pricing()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
