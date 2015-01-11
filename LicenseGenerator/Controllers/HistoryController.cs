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
        public JsonResult LoadLicenses(string filter, int page, int countPerPage)
        {
            using (LicenseGeneratorContext ctx = new LicenseGeneratorContext())
            {
                IQueryable<GeneratedLicense> foundedLicenses = ctx.GeneratedLicensesHistory
                    .Where(l => filter == "" || l.NIP.Contains(filter) || l.ProgramName.Contains(filter) || l.Company.Contains(filter) || l.PartnerNIP.Contains(filter));

                IEnumerable<GeneratedLicense> generatedLicenses = foundedLicenses
                    .OrderByDescending(l => l.GenerationDate)
                    .Skip((page - 1) * countPerPage)
                    .Take(countPerPage)
                    .ToList();

                int licensesCount = foundedLicenses.Count();

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