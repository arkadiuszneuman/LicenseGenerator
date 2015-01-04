using System.Reflection.Emit;
using inSolutions.Utilities.Security;
using LicenseGenerator.Controllers.Utilities;
using LicenseGenerator.DAL;
using LicenseGenerator.Models;
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
        private readonly LicenseCreator licenseCreator = new LicenseCreator();
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadClients(string clientValue)
        {
            using (LicenseGeneratorContext context = new LicenseGeneratorContext())
            {
                IEnumerable<Customer> vrlCustomers = context.Companies.Where(c => c.Name.Contains(clientValue) || c.Symbol.Contains(clientValue)).Take(10).ToList();
                IEnumerable<CustomerViewModel> vrlCustomerViewModels = AutoMapper.Mapper.Map<IEnumerable<CustomerViewModel>>(vrlCustomers);

                return Json(vrlCustomerViewModels);
            }
        }

        [HttpPost]
        public JsonResult GenerateLicense(LicenseViewModel license)
        {
            string oneFileLicense = licenseCreator.CreateLicenseFromVM(license);
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

            SaveLicenseHistory(license, true);

            return Json("licenses/" + fileName);
        }

        private static void SaveLicenseHistory(LicenseViewModel license, bool isEncrypted)
        {
            HistoryLicenseCreator vrlHistoryLicenseCreator = new HistoryLicenseCreator();
            var vrlGeneratedLicense = vrlHistoryLicenseCreator.GenerateLicense(license);
            HistoryLicenseSaver vrlHistoryLicenseSaver = new HistoryLicenseSaver(isEncrypted);
            vrlHistoryLicenseSaver.SaveLicenseHistory(vrlGeneratedLicense);
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
            string decryptedLicense = licenseCreator.CreateLicenseFromVM(license);

            SaveLicenseHistory(license, false);

            return Json(decryptedLicense);
        }

        public ActionResult History()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
