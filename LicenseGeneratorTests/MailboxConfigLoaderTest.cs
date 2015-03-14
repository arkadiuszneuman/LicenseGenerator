using System;
using FakeItEasy;
using LicenseGenerator.Controllers.Utilities.Home;
using LicenseGenerator.Controllers.Utilities.MailSenderController.MailboxConfig;
using LicenseGenerator.ViewModels;
using Xunit;

namespace LicenseGeneratorTests
{
    public class MailboxConfigLoaderTest
    {
        private MailboxConfigLoader CreateMailboxConfigLoader()
        {
            IMailboxConfigStringLoader mailboxConfigStringLoader = A.Fake<IMailboxConfigStringLoader>();
            A.CallTo(() => mailboxConfigStringLoader.LoadConfigFile()).Returns(GetValidConfigString());
            return new MailboxConfigLoader(mailboxConfigStringLoader);
        }

        [Fact]
        public void MailboxConfig_Loads_Valid_From_Property()
        {
            MailboxConfigLoader mailboxConfigLoader = CreateMailboxConfigLoader();

            MailboxConfig mailboxConfig = mailboxConfigLoader.LoadMailboxConfig();

            Assert.Equal("someFrom@test.pl", mailboxConfig.From);
        }

        [Fact]
        public void MailboxConfig_Loads_Valid_CredentialsFrom_Property()
        {
            MailboxConfigLoader mailboxConfigLoader = CreateMailboxConfigLoader();

            MailboxConfig mailboxConfig = mailboxConfigLoader.LoadMailboxConfig();

            Assert.Equal("someCredentialsFrom@test.pl", mailboxConfig.CredentialsFrom);
        }

        [Fact]
        public void MailboxConfig_Loads_Valid_CredentialsPass_Property()
        {
            MailboxConfigLoader mailboxConfigLoader = CreateMailboxConfigLoader();

            MailboxConfig mailboxConfig = mailboxConfigLoader.LoadMailboxConfig();

            Assert.Equal("somePass", mailboxConfig.CredentialsPass);
        }

        [Fact]
        public void MailboxConfig_Loads_Valid_Host_Property()
        {
            MailboxConfigLoader mailboxConfigLoader = CreateMailboxConfigLoader();

            MailboxConfig mailboxConfig = mailboxConfigLoader.LoadMailboxConfig();

            Assert.Equal("some.mail.server.pl", mailboxConfig.Host);
        }

        [Fact]
        public void MailboxConfig_Loads_Valid_Port_Property()
        {
            MailboxConfigLoader mailboxConfigLoader = CreateMailboxConfigLoader();

            MailboxConfig mailboxConfig = mailboxConfigLoader.LoadMailboxConfig();

            Assert.Equal(99, mailboxConfig.Port);
        }

        [Fact]
        public void MailboxConfig_Should_Throw_InvalidOperationException_On_Invalid_Json()
        {
            IMailboxConfigStringLoader mailboxConfigStringLoader = A.Fake<IMailboxConfigStringLoader>();
            A.CallTo(() => mailboxConfigStringLoader.LoadConfigFile()).Returns("{ \"someNotValid\": \"json\" }");
            MailboxConfigLoader mailboxConfigLoader = new MailboxConfigLoader(mailboxConfigStringLoader);

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
            {
                mailboxConfigLoader.LoadMailboxConfig();
            });

            Assert.Equal(exception.Message, "Invalid Json Object. Valid json template: \r\n" + GetValidConfigString());
        }

        private string GetValidConfigString()
        {
            return "{\r\n" +
                        "\"From\": \"someFrom@test.pl\",\r\n" +
                        "\"CredentialsFrom\": \"someCredentialsFrom@test.pl\",\r\n" +
                        "\"CredentialsPass\": \"somePass\",\r\n" +
                        "\"Host\": \"some.mail.server.pl\",\r\n" +
                        "\"Port\": 99\r\n" +
                    "}\r\n";
        }
    }
}