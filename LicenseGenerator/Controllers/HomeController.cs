using System.Reflection.Emit;
using inSolutions.Utilities.Security;
using LicenseGenerator.Controllers.Utilities;
using LicenseGenerator.DAL;
using LicenseGenerator.Models;
using LicenseGenerator.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LicenseGenerator.Controllers.Utilities.Home;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LicenseGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILicenseCreator licenseCreator;
        private readonly IPatternCustomersLoader patternCustomersLoader;

        public HomeController()
            : this(new CountPatternCustomersLoader(10), new LicenseCreator())
        {
        }

        public HomeController(IPatternCustomersLoader patternCustomersLoader, ILicenseCreator licenseCreator)
        {
            this.patternCustomersLoader = patternCustomersLoader;
            this.licenseCreator = licenseCreator;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadClients(string clientValue)
        {
            IEnumerable<Customer> vrlCustomers = patternCustomersLoader.LoadCustomers(clientValue);
            IEnumerable<CustomerViewModel> vrlCustomerViewModels = AutoMapper.Mapper.Map<IEnumerable<CustomerViewModel>>(vrlCustomers);

            return Json(vrlCustomerViewModels);
        }

        [HttpPost]
        public JsonResult GenerateLicense(LicenseViewModel license)
        {
            var fileName = GenerateLicenseToPath(license);

            return Json("licenses/" + fileName);
        }

        private string GenerateLicenseToPath(LicenseViewModel license)
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
            return fileName;
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

        [HttpPost]
        public JsonResult GenerateZippedLicense(LicenseViewModel license)
        {
            GetEndUserLicenseName(license);

            string fileName = GenerateLicenseToPath(license);
            string vrlDirectory = ControllerContext.HttpContext.Server.MapPath("~/licenses/");
            string zipFileName = Path.GetFileNameWithoutExtension(fileName) + ".zip";
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var demoFile = archive.CreateEntry(GetEndUserLicenseName(license) + "S.txt");

                    using (var entryStream = demoFile.Open())
                    using (Stream licenseStreamReader = new FileStream(vrlDirectory + fileName, FileMode.Open))
                    {
                        licenseStreamReader.CopyTo(entryStream);
                    }
                }

                using (var fileStream = new FileStream(vrlDirectory + zipFileName, FileMode.Create))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    memoryStream.CopyTo(fileStream);
                }
            }

            return Json("licenses/" + zipFileName);
        }

        private string GetEndUserLicenseName(LicenseViewModel license)
        {
            var licenseName = license.Name + "_" + license.Nip;

            if (license.PartnerNip != null)
            {
                licenseName += "_" + license.PartnerNip;
            }

            return licenseName;
        }

        [HttpPost]
        public ContentResult LoadLicense(HttpPostedFileBase objectToUpload)
        {
            try
            {
                if (objectToUpload != null && objectToUpload.ContentLength > 0 && objectToUpload.ContentType == "text/plain")
                {
                    LicenseViewModel vrlLicenseViewModel = null;
                    using (StreamReader reader = new StreamReader(objectToUpload.InputStream, Encoding.GetEncoding(1250)))
                    {
                        string license = reader.ReadToEnd();
                        try
                        {
                            license = Cl_DataEncryption.DecryptText(license);
                        }
                        catch (CryptographicException)
                        {
                        }

                        LicenseLoader licenseLoader = new LicenseLoader();
                        vrlLicenseViewModel = licenseLoader.CreateVMFromLicense(license);
                    }

                    var returnObject = new
                    {
                        success = true,
                        license = vrlLicenseViewModel
                    };

                    string json =
                          JsonConvert.SerializeObject(
                            returnObject,
                            Formatting.Indented,
                            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                          );

                    return this.Content(json, "application/json");
                }

                var returnObject2 = new
                {
                    success = false,
                    message = "Nieprawidłowy typ pliku."
                };

                string json2 =
                     JsonConvert.SerializeObject(
                       returnObject2,
                       Formatting.Indented,
                       new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                     );

                return this.Content(json2, "application/json");
            }
            catch (Exception e)
            {
                var returnObject = new
                {
                    success = false,
                    message = e.Message
                };

                string json =
                     JsonConvert.SerializeObject(
                       returnObject,
                       Formatting.Indented,
                       new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                     );

                return this.Content(json, "application/json");
            }
        }
    }
}
