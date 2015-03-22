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
    public class SubscriptionsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonNetResult LoadSubscriptions(string filter, int page, int countPerPage)
        {
            using (LicenseGeneratorContext ctx = new LicenseGeneratorContext())
            {
                IQueryable<GeneratedLicense> foundedLicenses = ctx.GeneratedLicensesHistory
                    .Where(l => l.IsForClient &&
                        l.LicenseType != null &&
                        l.LicenseType.Value == LicenseType.Date);

                IEnumerable<DateTime> dates = GetDates(foundedLicenses).ToList();

                IEnumerable<GeneratedLicense> generatedLicenses = foundedLicenses
                    .OrderByDescending(l => l.LicenseTermDate)
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
                    licenses = generatedLicenses,
                    years = dates.Select(d => d.Year),
                    months = dates.Select(d => d.Month)
                };

                return new JsonNetResult(returnObject);
            }
        }

        private IQueryable<DateTime> GetDates(IQueryable<GeneratedLicense> foundedLicenses)
        {
            return foundedLicenses.Select(l => l.LicenseTermDate.Value);
        }
    }
}