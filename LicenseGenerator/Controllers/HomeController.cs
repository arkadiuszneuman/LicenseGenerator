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
using LicenseGenerator.Controllers.Utilities.Home.LicenseGenerator;
using LicenseGenerator.Controllers.Utilities.Home.LicenseLoader;
using LicenseGenerator.Controllers.Utilities.JsonConverter;
using LicenseGenerator.DAL;
using LicenseGenerator.Models;
using LicenseGenerator.ViewModels;

namespace LicenseGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILicenseLoader licenseLoader;
        private readonly IJsonConverter jsonConverter;
        private readonly IPatternCustomersLoader patternCustomersLoader;
        private readonly ILicensePathGenerator licenseGenerator;
        private readonly ILicenseCreator licenseCreator;
        private readonly IPatternProductsLoader patternProductsLoader;
        private readonly INewestVersionLoader newestVersionLoader;

        public HomeController()
            : this(new CountPatternCustomersLoader(10), new LicensePathGenerator(new LicenseCreator()), new LicenseCreator(),  
            new LicenseLoader(new LicenseToViewModelConverter(), new StreamLicenseLoader()), new JsonJavascriptConverter(), new CountPatternProductsLoader(10),
            new ProductNewestVersionLoader())
        {
        }

        public HomeController(IPatternCustomersLoader patternCustomersLoader, ILicensePathGenerator licenseGenerator, 
            ILicenseCreator licenseCreator, ILicenseLoader licenseLoader, 
            IJsonConverter jsonConverter, IPatternProductsLoader patternProductsLoader, INewestVersionLoader newestVersionLoader)
        {
            this.patternCustomersLoader = patternCustomersLoader;
            this.licenseGenerator = licenseGenerator;
            this.licenseCreator = licenseCreator;
            this.licenseLoader = licenseLoader;
            this.jsonConverter = jsonConverter;
            this.patternProductsLoader = patternProductsLoader;
            this.newestVersionLoader = newestVersionLoader;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HistoryLicense(int id)
        {
            using (LicenseGeneratorContext context = new LicenseGeneratorContext())
            {
                GeneratedLicense license = context.GeneratedLicensesHistory.SingleOrDefault(l => l.Id == id);
                HistoryLicenseCreator historyLicenseCreator = new HistoryLicenseCreator();
                LicenseViewModel licenseViewModel = historyLicenseCreator.GenerateViewModel(license);
                return View("Index", licenseViewModel);
            }
        }

        [HttpPost]
        public JsonNetResult LoadClients(string clientValue)
        {
            IEnumerable<Customer> vrlCustomers = patternCustomersLoader.LoadCustomers(clientValue);
            IEnumerable<CustomerViewModel> vrlCustomerViewModels = Mapper.Map<IEnumerable<CustomerViewModel>>(vrlCustomers);

            return new JsonNetResult(new SuccessObject(true, vrlCustomerViewModels));
        }

        [HttpPost]
        public JsonNetResult LoadProducts(string licenseName)
        {
            try
            {
                IEnumerable<Product> products = patternProductsLoader.LoadProducts(licenseName);

                List<ProductViewModel> productsViewModels = new List<ProductViewModel>();
                foreach (var product in products)
                {
                    productsViewModels.Add(new ProductViewModel() 
                    {
                        LicenseName = product.LicenseName, 
                        ProgramName = product.Name,
                        Version = product.NewestVersion
                    });
                }

                return new JsonNetResult(new SuccessObject(true, productsViewModels));
            }
            catch (Exception exception)
            {
                return new JsonNetResult(new SuccessObject(false, exception.Message));
            }
        }

        [HttpPost]
        public JsonNetResult GetProductNewestVersion(string programName)
        {
            try
            {
                string newestVersion = newestVersionLoader.LoadNewestVersion(programName);

                if (!string.IsNullOrEmpty(newestVersion))
                {
                    return new JsonNetResult(new SuccessObject(true, newestVersion));
                }
                else
                {
                    return new JsonNetResult(new SuccessObject(false, null));
                }
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
                var fileName = licenseGenerator.GenerateLicenseToPath(licenseViewModel);
                SuccessObject successObject = new SuccessObject(true, "licenses/" + fileName);
                return new JsonNetResult(successObject);
            }
            catch (Exception e)
            {
                SuccessObject successObject = new SuccessObject(false, e.Message);
                return new JsonNetResult(successObject);
            }
        }

        [HttpPost]
        public JsonResult GenerateDecryptedLicense(LicenseViewModel licenseViewModel)
        {
            string decryptedLicense = licenseCreator.CreateLicenseFromVM(licenseViewModel);

            licenseGenerator.SaveLicenseHistory(licenseViewModel, false);

            return Json(decryptedLicense);
        }

        [HttpPost]
        public JsonResult GenerateZippedLicense(LicenseViewModel licenseViewModel)
        {
            string zipPath = licenseGenerator.GenerateZippedLicense(licenseViewModel);

            return Json("licenses/" + zipPath);
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
