using System.Threading;
using System.Web.Mvc;
using LicenseGenerator.Controllers.Utilities;
using LicenseGenerator.Controllers.Utilities.JsonConverter;
using LicenseGenerator.ViewModels.MailSend;

namespace LicenseGenerator.Controllers
{
    public class MailSendController : Controller
    {
        public ActionResult Send(MailViewModel mail)
        {
            Thread.Sleep(2000);
            return new JsonNetResult(new SuccessObject(true, null));
        }
    }
}