using System.Net;
using System.Net.Mail;
using System.Security;
using System.Threading;
using System.Web.Mvc;
using LicenseGenerator.Controllers.Utilities;
using LicenseGenerator.Controllers.Utilities.JsonConverter;
using LicenseGenerator.Controllers.Utilities.MailSenderController.MailboxConfig;
using LicenseGenerator.ViewModels;
using LicenseGenerator.ViewModels.MailSend;

namespace LicenseGenerator.Controllers
{
    public class MailSendController : Controller
    {
        private readonly IMailboxConfigLoader mailboxConfigLoader;

        public MailSendController()
            :this(new MailboxConfigLoader(new MailboxConfigStringLoader()))
        {
        }

        public MailSendController(IMailboxConfigLoader mailboxConfigLoader)
        {
            this.mailboxConfigLoader = mailboxConfigLoader;
        }

        public ActionResult Send(MailViewModel mail, LicenseViewModel license)
        {
            MailboxConfig mailboxConfig = mailboxConfigLoader.LoadMailboxConfig();

            MailMessage mailMessage = new MailMessage(mailboxConfig.From, mail.Addresses);
            SmtpClient client = new SmtpClient();
            client.Port = mailboxConfig.Port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(mailboxConfig.CredentialsFrom, mailboxConfig.CredentialsPass);
            client.Host = mailboxConfig.Host;
            mailMessage.Subject = mail.Title;
            mailMessage.Body = mail.Message;
            //client.Send(mailMessage);

            return new JsonNetResult(new SuccessObject(true, null));
        }
    }
}