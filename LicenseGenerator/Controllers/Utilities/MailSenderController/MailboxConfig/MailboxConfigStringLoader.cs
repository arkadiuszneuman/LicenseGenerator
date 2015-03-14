using System;
using System.IO;
using System.Web;

namespace LicenseGenerator.Controllers.Utilities.MailSenderController.MailboxConfig
{
    public class MailboxConfigStringLoader : IMailboxConfigStringLoader
    {
        public string LoadConfigFile()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/mailboxConf.json");

            if (!File.Exists(filePath))
            {
                throw new ArgumentException(string.Format("You need to create JSON file in {0}", filePath));
            }

            using (StreamReader sr = new StreamReader(filePath))
            {
                return sr.ReadToEnd();
            }
        } 
    }
}