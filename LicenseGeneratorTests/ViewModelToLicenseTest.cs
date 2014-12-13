using System;
using LicenseGenerator.Controllers.Utilities;
using LicenseGenerator.ViewModels;
using Xunit;

namespace LicenseGeneratorTests
{
    public class ViewModelToLicenseTest
    {
        [Fact]
        public void LicenseCreator_Should_Throw_On_Null_License_Name()
        {
            LicenseCreator vrlLicenseCreator = new LicenseCreator();
            LicenseViewModel vrlLicenseViewModel = GetLicenseViewModel();
            vrlLicenseViewModel.Name = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                vrlLicenseCreator.CreateLicenseFromVM(vrlLicenseViewModel);
            });
        }

        [Fact]
        public void LicenseCreator_Should_Throw_On_Null_Nip()
        {
            LicenseCreator vrlLicenseCreator = new LicenseCreator();
            LicenseViewModel vrlLicenseViewModel = GetLicenseViewModel();
            vrlLicenseViewModel.Nip = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                vrlLicenseCreator.CreateLicenseFromVM(vrlLicenseViewModel);
            });
        }

        [Fact]
        public void LicenseCreator_Should_Throw_On_Too_Short_Nip()
        {
            LicenseCreator vrlLicenseCreator = new LicenseCreator();
            LicenseViewModel vrlLicenseViewModel = GetLicenseViewModel();
            vrlLicenseViewModel.Nip = "1";

            Assert.Throws<InvalidNipException>(() =>
            {
                vrlLicenseCreator.CreateLicenseFromVM(vrlLicenseViewModel);
            });
        }

        [Fact]
        public void LicenseCreator_Should_Throw_On_Too_Long_Nip()
        {
            LicenseCreator vrlLicenseCreator = new LicenseCreator();
            LicenseViewModel vrlLicenseViewModel = GetLicenseViewModel();
            vrlLicenseViewModel.Nip = "12345678901";

            Assert.Throws<InvalidNipException>(() =>
            {
                vrlLicenseCreator.CreateLicenseFromVM(vrlLicenseViewModel);
            });
        }

        [Fact]
        public void LicenseCreator_Should_Throw_On_Null_Company()
        {
            LicenseCreator vrlLicenseCreator = new LicenseCreator();
            LicenseViewModel vrlLicenseViewModel = GetLicenseViewModel();
            vrlLicenseViewModel.Company1 = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                vrlLicenseCreator.CreateLicenseFromVM(vrlLicenseViewModel);
            });
        }

        [Fact]
        public void LicenseCreator_Should_Return_Valid_License()
        {
            LicenseCreator vrlLicenseCreator = new LicenseCreator();
            LicenseViewModel vrlLicenseViewModel = GetLicenseViewModel();

            string vrlLicense = vrlLicenseCreator.CreateLicenseFromVM(vrlLicenseViewModel);
            const string correctLicense = "Program\r\n" +
                                             "1234567890\r\n" +
                                             "priv1, priv2\r\n" +
                                             "Company\r\n" +
                                             "Addional info\r\n" +
                                             "2010-12-15\r\n" +
                                             "5\r\n" +
                                             "1.0.1";

            Assert.Equal(correctLicense, vrlLicense);
        }

        [Fact]
        public void LicenseCreator_Should_Return_Valid_License_On_Empty_License_Numbers()
        {
            LicenseCreator vrlLicenseCreator = new LicenseCreator();
            LicenseViewModel vrlLicenseViewModel = GetLicenseViewModel();
            vrlLicenseViewModel.LicenseNumbers = null;

            string vrlLicense = vrlLicenseCreator.CreateLicenseFromVM(vrlLicenseViewModel);
            const string correctLicense = "Program\r\n" +
                                             "1234567890\r\n" +
                                             "priv1, priv2\r\n" +
                                             "Company\r\n" +
                                             "Addional info\r\n" +
                                             "2010-12-15\r\n" +
                                             "\r\n" +
                                             "1.0.1";

            Assert.Equal(correctLicense, vrlLicense);
        }

        [Fact]
        public void LicenseCreator_Should_Return_Valid_License_On_Empty_License_Numbers_And_Version()
        {
            LicenseCreator vrlLicenseCreator = new LicenseCreator();
            LicenseViewModel vrlLicenseViewModel = GetLicenseViewModel();
            vrlLicenseViewModel.LicenseNumbers = null;
            vrlLicenseViewModel.ProgramVersion = null;

            string vrlLicense = vrlLicenseCreator.CreateLicenseFromVM(vrlLicenseViewModel);
            const string correctLicense = "Program\r\n" +
                                          "1234567890\r\n" +
                                          "priv1, priv2\r\n" +
                                          "Company\r\n" +
                                          "Addional info\r\n" +
                                          "2010-12-15";

            Assert.Equal(correctLicense, vrlLicense);
        }

        [Fact]
        public void LicenseCreator_Should_Return_Valid_License_On_Null_Privileges()
        {
            LicenseCreator vrlLicenseCreator = new LicenseCreator();
            LicenseViewModel vrlLicenseViewModel = GetLicenseViewModel();
            vrlLicenseViewModel.Privileges = null;

            string vrlLicense = vrlLicenseCreator.CreateLicenseFromVM(vrlLicenseViewModel);
            const string correctLicense = "Program\r\n" +
                                             "1234567890\r\n" +
                                             "0\r\n" +
                                             "Company\r\n" +
                                             "Addional info\r\n" +
                                             "2010-12-15\r\n" +
                                             "5\r\n" +
                                             "1.0.1";

            Assert.Equal(correctLicense, vrlLicense);
        }

        [Fact]
        public void LicenseCreator_Should_Return_Valid_License_On_Null_Addional_Info()
        {
            LicenseCreator vrlLicenseCreator = new LicenseCreator();
            LicenseViewModel vrlLicenseViewModel = GetLicenseViewModel();
            vrlLicenseViewModel.Company2 = null;

            string vrlLicense = vrlLicenseCreator.CreateLicenseFromVM(vrlLicenseViewModel);
            const string correctLicense = "Program\r\n" +
                                             "1234567890\r\n" +
                                             "priv1, priv2\r\n" +
                                             "Company\r\n" +
                                             "\r\n" +
                                             "2010-12-15\r\n" +
                                             "5\r\n" +
                                             "1.0.1";

            Assert.Equal(correctLicense, vrlLicense);
        }

        private LicenseViewModel GetLicenseViewModel()
        {
            return new LicenseViewModel()
            {
                Company1 = "Company",
                Company2 = "Addional info",
                Date = new DateTime(2010, 12, 15),
                LicenseNumbers = 5,
                Name = "Program",
                Nip = "1234567890",
                Privileges = "priv1, priv2",
                ProgramVersion = "1.0.1"
            };
        }
    }
}
