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
        // GET: History
        public ActionResult Index()
        {
            using (LicenseGeneratorContext ctx = new LicenseGeneratorContext())
            {
                IEnumerable<GeneratedLicense> vrlGeneratedLicenses = ctx.GeneratedLicensesHistory.OrderByDescending(l => l.GenerationDate).ToList();
                return View(vrlGeneratedLicenses);
            }
        }
    }
}