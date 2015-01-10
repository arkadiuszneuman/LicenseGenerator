using System.Web.Mvc;
using LicenseGenerator.Controllers.Utilities.About;
using LicenseGenerator.ViewModels.About;

namespace LicenseGenerator.Controllers
{
    public class AboutController : Controller
    {
        private readonly IVersionGetter versionGetter;

        public AboutController()
            : this(new ProgramVersionGetter())
        {
        }

        public AboutController(IVersionGetter versionGetter)
        {
            this.versionGetter = versionGetter;
        }

        // GET: About
        public ActionResult Index()
        {
            var version = versionGetter.GetVersion();
            return View(new VersionViewModel("v" + version));
        }
    }
}