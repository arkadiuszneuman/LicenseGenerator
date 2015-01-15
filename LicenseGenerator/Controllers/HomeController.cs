using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using inSolutions.Utilities.Security;
using LicenseGenerator.Controllers.Utilities;
using LicenseGenerator.Controllers.Utilities.Home;
using LicenseGenerator.Controllers.Utilities.Home.LicenseLoader;
using LicenseGenerator.Controllers.Utilities.JsonConverter;
using LicenseGenerator.Models;
using LicenseGenerator.ViewModels;

namespace LicenseGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILicenseCreator licenseCreator;
        private readonly ILicenseLoader licenseLoader;
        private readonly IJsonConverter jsonConverter;
        private readonly IPatternCustomersLoader patternCustomersLoader;
        private readonly IPatternProductsLoader patternProductsLoader;

        public HomeController()
            : this(new CountPatternCustomersLoader(10), new LicenseCreator(),
            new LicenseLoader(new LicenseToViewModelConverter(), new StreamLicenseLoader()), new JsonJavascriptConverter(), new CountPatternProductsLoader(10))
        {
        }

        public HomeController(IPatternCustomersLoader patternCustomersLoader, ILicenseCreator licenseCreator, ILicenseLoader licenseLoader, IJsonConverter jsonConverter, IPatternProductsLoader patternProductsLoader)
        {
            this.patternCustomersLoader = patternCustomersLoader;
            this.licenseCreator = licenseCreator;
            this.licenseLoader = licenseLoader;
            this.jsonConverter = jsonConverter;
            this.patternProductsLoader = patternProductsLoader;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadClients(string clientValue)
        {
            IEnumerable<Customer> vrlCustomers = patternCustomersLoader.LoadCustomers(clientValue);
            IEnumerable<CustomerViewModel> vrlCustomerViewModels = Mapper.Map<IEnumerable<CustomerViewModel>>(vrlCustomers);

            return Json(vrlCustomerViewModels);
        }

        [HttpPost]
        public JsonNetResult LoadProducts(string licenseName)
        {
            try
            {
                IEnumerable<Product> products = patternProductsLoader.LoadProducts(licenseName);
                IEnumerable<ProductViewModel> productsViewModels = Mapper.Map<IEnumerable<ProductViewModel>>(products);

                return new JsonNetResult(new SuccessObject(true, productsViewModels));
            }
            catch (Exception exception)
            {
                return new JsonNetResult(new SuccessObject(false, exception.Message));
            }
        }

        [HttpPost]
        public JsonResult GenerateLicense(LicenseViewModel licenseViewModel)
        {
            try
            {
                var fileName = GenerateLicenseToPath(licenseViewModel);
                SuccessObject successObject = new SuccessObject(true, "licenses/" + fileName);
                return Json(successObject);
            }
            catch (Exception e)
            {
                SuccessObject successObject = new SuccessObject(false, e.Message);
                return Json(successObject);
            }
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
                System.IO.File.Delete(filePath);
                return filePath;
                //for (int i = 1; i < 9999; ++i)
                //{
                //    string vrlNewFilePath = filePath.Insert(filePath.Count() - 4, "_" + i);
                //    if (!System.IO.File.Exists(vrlNewFilePath))
                //    {
                //        return vrlNewFilePath;
                //    }
                //}
            }

            throw new InvalidOperationException("Nie można wygenerować licencji");
        }

        [HttpPost]
        public JsonResult GenerateDecryptedLicense(LicenseViewModel licenseViewModel)
        {
            string decryptedLicense = licenseCreator.CreateLicenseFromVM(licenseViewModel);

            SaveLicenseHistory(licenseViewModel, false);

            return Json(decryptedLicense);
        }

        [HttpPost]
        public JsonResult GenerateZippedLicense(LicenseViewModel licenseViewModel)
        {
            GetEndUserLicenseName(licenseViewModel);

            string fileName = GenerateLicenseToPath(licenseViewModel);
            string vrlDirectory = ControllerContext.HttpContext.Server.MapPath("~/licenses/");
            string zipFileName = Path.GetFileNameWithoutExtension(fileName) + ".zip";
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var demoFile = archive.CreateEntry(GetEndUserLicenseName(licenseViewModel) + "S.txt");

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
            SuccessObject successObject = licenseLoader.LoadLicense(objectToUpload);
            string json = jsonConverter.ConvertToJson(successObject);

            return Content(json, "application/json");
        }
    }
}
