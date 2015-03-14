using System;
using System.IO;
using System.Web;
using System.Web.Helpers;
using Microsoft.Ajax.Utilities;

namespace LicenseGenerator.Controllers.Utilities.MailSenderController.MailboxConfig
{
    public class MailboxConfigLoader : IMailboxConfigLoader
    {
        private readonly IMailboxConfigStringLoader mailboxConfigStringLoader;

        public MailboxConfigLoader(IMailboxConfigStringLoader mailboxConfigStringLoader)
        {
            this.mailboxConfigStringLoader = mailboxConfigStringLoader;
        }

        public MailboxConfig LoadMailboxConfig()
        {
            string json = mailboxConfigStringLoader.LoadConfigFile();
            MailboxConfig config = Newtonsoft.Json.JsonConvert.DeserializeObject<MailboxConfig>(json);

            ThrowExceptionIfConfigInvalid(config);

            return config;
        }

        private void ThrowExceptionIfConfigInvalid(MailboxConfig config)
        {
            if (config.CredentialsFrom == null || config.CredentialsPass == null || config.From == null ||
                config.Host == null || config.Port == 0)
            {
                throw new InvalidOperationException("Invalid Json Object. Valid json template: \r\n" +
                    "{\r\n" +
                        "\"From\": \"someFrom@test.pl\",\r\n" +
                        "\"CredentialsFrom\": \"someCredentialsFrom@test.pl\",\r\n" +
                        "\"CredentialsPass\": \"somePass\",\r\n" +
                        "\"Host\": \"some.mail.server.pl\",\r\n" +
                        "\"Port\": 99\r\n" +
                    "}\r\n");
            }
        }
    }
}