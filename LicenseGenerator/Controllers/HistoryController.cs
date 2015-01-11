using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LicenseGenerator.DAL;
using LicenseGenerator.Models;

namespace LicenseGenerator.Controllers
{
    public class HistoryController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadLicenses(int page, int countPerPage)
        {
            using (LicenseGeneratorContext ctx = new LicenseGeneratorContext())
            {
                IEnumerable<GeneratedLicense> generatedLicenses = ctx.GeneratedLicensesHistory
                    .OrderByDescending(l => l.GenerationDate)
                    .Skip((page - 1) * countPerPage)
                    .Take(countPerPage)
                    .ToList();

                int licensesCount = ctx.GeneratedLicensesHistory.Count();

                var returnObject = new
                {
                    count = licensesCount,
                    licenses = generatedLicenses
                };

                return Json(returnObject);
            }
        }
    }
}