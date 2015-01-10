using System;
using System.Text;
using LicenseGenerator.ViewModels;

namespace LicenseGenerator.Controllers.Utilities.Home
{
    public class LicenseCreator : ILicenseCreator
    {
        public string CreateLicenseFromVM(LicenseViewModel license)
        {
            CheckLicense(license);

            StringBuilder builder = new StringBuilder();

            builder.AppendLine(license.Name)
                .AppendLine(license.Nip)
                .AppendLine(license.Privileges ?? "0")
                .AppendLine(license.Company1)
                .AppendLine(license.Company2);

            if (license.Date != null)
            {
                builder.AppendLine(license.Date.Value.Date.ToString("yyyy-MM-dd"));
            }
            else
            {
                builder.AppendLine();
            }

            builder.AppendLine(license.LicenseNumbers.ToString())
                .AppendLine(license.ProgramVersion);

            return builder.ToString().Trim();
        }

        private void CheckLicense(LicenseViewModel vrpLicense)
        {
            if (string.IsNullOrEmpty(vrpLicense.Name))
            {
                throw new ArgumentNullException("vrpLicense.Name");
            }

            if (string.IsNullOrEmpty(vrpLicense.Company1))
            {
                throw new ArgumentNullException("vrpLicense.Company1");
            }

            if (string.IsNullOrEmpty(vrpLicense.Nip))
            {
                throw new ArgumentNullException("vrpLicense.Nip");
            }

            if (vrpLicense.Nip.Length != 10)
            {
                throw new InvalidNipException();
            }

            long number;
            bool isNumeric = long.TryParse(vrpLicense.Nip, out number);

            if (!isNumeric)
            {
                throw new InvalidNipException();
            }
        }
    }
}