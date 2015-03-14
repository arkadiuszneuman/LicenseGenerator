using System;
using System.IO;
using System.Text;
using System.Web;
using inSolutions.Utilities.Security;
using LicenseGenerator.ViewModels;

namespace LicenseGenerator.Controllers.Utilities.Home.LicenseGenerator
{
    public class LicensePathGenerator : ILicensePathGenerator
    {
        private readonly ILicenseCreator licenseCreator;

        public LicensePathGenerator(ILicenseCreator licenseCreator)
        {
            this.licenseCreator = licenseCreator;
            ServerLicensesDirectory = HttpContext.Current.Server.MapPath("~/licenses/");
        }

        public string GenerateLicenseToPath(LicenseViewModel license)
        {
            string oneFileLicense = licenseCreator.CreateLicenseFromVM(license);
            string encryptedLicense = Cl_DataEncryption.EncryptText(oneFileLicense);

            if (!Directory.Exists(ServerLicensesDirectory))
            {
                Directory.CreateDirectory(ServerLicensesDirectory);
            }

            string fileName = GetFileName(license);

            using (StreamWriter vrlWriter = new StreamWriter(ServerLicensesDirectory + fileName, false, Encoding.GetEncoding(1250)))
            {
                vrlWriter.Write(encryptedLicense);
            }

            SaveLicenseHistory(license, true);
            return fileName;
        }

        public void SaveLicenseHistory(LicenseViewModel license, bool isEncrypted)
        {
            HistoryLicenseCreator vrlHistoryLicenseCreator = new HistoryLicenseCreator();
            var vrlGeneratedLicense = vrlHistoryLicenseCreator.GenerateLicense(license);
            HistoryLicenseSaver vrlHistoryLicenseSaver = new HistoryLicenseSaver(isEncrypted);
            vrlHistoryLicenseSaver.SaveLicenseHistory(vrlGeneratedLicense);
        }

        private string GetFileName(LicenseViewModel license)
        {
            string filePath = license.Name + DateTime.Now.ToString()
                            .Replace(" ", "").Replace(":", "").Replace("-", "") + "S.txt";

            if (!File.Exists(filePath))
            {
                return filePath;
            }
            else
            {
                File.Delete(filePath);
                return filePath;
            }
        }

        public string ServerLicensesDirectory
        {
            get; 
            private set;
        }
    }
}