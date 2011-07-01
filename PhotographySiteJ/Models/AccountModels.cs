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

namespace PhotographySiteJ.Models
{
     
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class Contact
    {
        [Required(ErrorMessage = "Email address is requred.")]
        [Email(ErrorMessage = "Not a valid email address.")]
        [Display(Name = "Enter the email address for replies:")]
        public string ContactEmail { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        [Display(Name = "Give a subject for you correspondence:")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Correspondence is required.")]
        [Display(Name = "Type your message:")]
        public string Message { get; set; }
    }

    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base(@"\b[a-z0-9._%-]+@[a-z0-9.-]+\.[a-z]{2,4}\b") 
              {}
    }

    public class UploadImage
    {
        public string Gallery { get; set; }
        public string File { get; set; }
    }

    public class AdminModel
    {
        public UploadImage UploadImageModel { get; set; }
        public List<ImageInformation> ImageInformationModel { get; set; }
    }
}
