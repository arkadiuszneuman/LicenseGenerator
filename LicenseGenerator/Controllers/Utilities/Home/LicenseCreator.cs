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

        private void CheckLicense(LicenseViewModel license)
        {
            if (string.IsNullOrEmpty(license.Name))
            {
                throw new ArgumentNullException("license.Name");
            }

            if (string.IsNullOrEmpty(license.Company1))
            {
                throw new ArgumentNullException("license.Company1");
            }

            if (string.IsNullOrEmpty(license.Nip))
            {
                throw new ArgumentNullException("license.Nip");
            }

            if (license.Nip.Length != 10)
            {
                throw new InvalidNipException();
            }

            long number;
            bool isNumeric = long.TryParse(license.Nip, out number);

            if (!isNumeric)
            {
                throw new InvalidNipException();
            }
        }
    }
}