using System.Threading;
using LicenseGenerator.DAL;
using LicenseGenerator.Models;

namespace LicenseGenerator.Controllers.Utilities
{
    public class HistoryLicenseSaver
    {
        private readonly bool isEncrypted;

        public HistoryLicenseSaver(bool vrpIsEncrypted)
        {
            isEncrypted = vrpIsEncrypted;
        }

        public void SaveLicenseHistory(GeneratedLicense license)
        {
            license.IsEncrypted = isEncrypted;
            using (LicenseGeneratorContext ctx = new LicenseGeneratorContext())
            {
                ctx.GeneratedLicensesHistory.Add(license);
                ctx.SaveChanges();
            }
        }
    }
}