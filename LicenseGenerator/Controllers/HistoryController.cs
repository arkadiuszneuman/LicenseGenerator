using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LicenseGenerator.Controllers.Utilities;
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
        public JsonNetResult LoadLicenses(string filter, int page, int countPerPage)
        {
            using (LicenseGeneratorContext ctx = new LicenseGeneratorContext())
            {
                IQueryable<GeneratedLicense> foundedLicenses = ctx.GeneratedLicensesHistory
                    .Where(l => filter == "" || l.NIP.Contains(filter) || l.ProgramName.Contains(filter) || l.Company.Contains(filter) || l.PartnerNIP.Contains(filter)
                    || l.UserName.Contains(filter));

                IEnumerable<GeneratedLicense> generatedLicenses = foundedLicenses
                    .OrderByDescending(l => l.GenerationDate)
                    .Skip((page - 1) * countPerPage)
                    .Take(countPerPage)
                    .ToList();

                int licensesCount = foundedLicenses.Count();

                foreach (var license in generatedLicenses)
                {
                    license.UserName = license.UserName.Replace(@"INSOLUTIONS\", "");
                }

                var returnObject = new
                {
                    count = licensesCount,
                    licenses = generatedLicenses
                };

                return new JsonNetResult(returnObject);
            }
        }
    }
}