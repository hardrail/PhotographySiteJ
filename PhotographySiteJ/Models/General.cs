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
    public abstract class General
    {
        public static List<string> GetFilesInFolder(string folderPath)
        {
            folderPath.Replace(@"\", "/");

            return Directory.GetFiles(folderPath).Select(
                element => "~" + element.Substring(element.IndexOf(@"\Content"), element.Length - element.IndexOf(@"\Content"))).ToList();
        }

        public static List<string> GetAllFoldersInPath(string folderPath)
        {
            return Directory.GetDirectories(folderPath, "*", SearchOption.AllDirectories).Select(m => "~" + m.Substring(m.IndexOf(@"\Content"), m.Length - m.IndexOf(@"\Content"))).ToList();
        }

    }
}