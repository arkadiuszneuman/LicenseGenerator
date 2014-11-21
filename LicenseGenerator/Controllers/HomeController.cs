using inSolutions.Utilities.Security;
using LicenseGenerator.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LicenseGenerator.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GenerateLicense(LicenseViewModel license)
        {
            string oneFileLicense = CreateLicenseFromVM(license);
            string encryptedLicense = Cl_DataEncryption.EncryptText(oneFileLicense);

            string vrlDirectory = ControllerContext.HttpContext.Server.MapPath("~/licenses/");
            if (!Directory.Exists(vrlDirectory))
            {
                Directory.CreateDirectory(vrlDirectory);
            }

            string fileName = GetFileName(license);

            using (StreamWriter vrlWriter = new StreamWriter(vrlDirectory + fileName, false, Encoding.GetEncoding(1250)))
            {
                vrlWriter.Write(encryptedLicense);
            }
            return Json("licenses/" + fileName);
        }

        private static string GetFileName(LicenseViewModel license)
        {
            string filePath = license.Name + DateTime.Now.ToString()
                            .Replace(" ", "").Replace(":", "").Replace("-", "") + "S.txt";
            
            if (!System.IO.File.Exists(filePath))
            {
                return filePath;
            }
            else
            {
                for (int i = 1; i < 9999; ++i)
                {
                    string vrlNewFilePath = filePath.Insert(filePath.Count() - 4, "_" + i);
                    if (!System.IO.File.Exists(vrlNewFilePath))
                    {
                        return vrlNewFilePath;
                    }
                }
            }

            throw new InvalidOperationException("Nie można wygenerować licencji");
        }

        [HttpPost]
        public JsonResult GenerateDecryptedLicense(LicenseViewModel license)
        {
            string decryptedLicense = CreateLicenseFromVM(license);
            return Json(decryptedLicense);
        }

        private string CreateLicenseFromVM(LicenseViewModel license)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(license.Name)
                .AppendLine(license.Nip)
                .AppendLine(license.Privileges == null ? "0" : license.Privileges)
                .AppendLine(license.Company1)
                .Append(license.Company2);

            if (license.Date != null )
            {
                builder.Append(Environment.NewLine + license.Date.Value.Date.ToString("yyyy-MM-dd"));
            }

            return builder.ToString();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
