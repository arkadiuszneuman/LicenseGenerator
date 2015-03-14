using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Threading;
using System.Web.Mvc;
using LicenseGenerator.Controllers.Utilities;
using LicenseGenerator.Controllers.Utilities.Home;
using LicenseGenerator.Controllers.Utilities.Home.LicenseGenerator;
using LicenseGenerator.Controllers.Utilities.JsonConverter;
using LicenseGenerator.Controllers.Utilities.MailSenderController.MailboxConfig;
using LicenseGenerator.ViewModels;
using LicenseGenerator.ViewModels.MailSend;

namespace LicenseGenerator.Controllers
{
    public class MailSendController : Controller
    {
        private readonly IMailboxConfigLoader mailboxConfigLoader;
        private readonly ILicensePathGenerator licensePathGenerator;

        public MailSendController()
            :this(new MailboxConfigLoader(new MailboxConfigStringLoader()), new LicensePathGenerator(new LicenseCreator()))
        {
        }

        public MailSendController(IMailboxConfigLoader mailboxConfigLoader, ILicensePathGenerator licensePathGenerator)
        {
            this.mailboxConfigLoader = mailboxConfigLoader;
            this.licensePathGenerator = licensePathGenerator;
        }

        public ActionResult Send(MailViewModel mail, LicenseViewModel license)
        {
            string licenseName = licensePathGenerator.GenerateLicenseToPath(license);
            string licensePath = Path.Combine(licensePathGenerator.ServerLicensesDirectory, licenseName);

            MailboxConfig mailboxConfig = mailboxConfigLoader.LoadMailboxConfig();

            MailMessage mailMessage = new MailMessage(mailboxConfig.From, mail.Addresses);
            SmtpClient client = new SmtpClient();
            client.Port = mailboxConfig.Port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(mailboxConfig.CredentialsFrom, mailboxConfig.CredentialsPass);
            client.Host = mailboxConfig.Host;
            mailMessage.Subject = mail.Title;
            mailMessage.Body = mail.Message;
            mailMessage.Attachments.Add(new Attachment(licensePath));
            client.Send(mailMessage);

            return new JsonNetResult(new SuccessObject(true, null));
        }
    }
}