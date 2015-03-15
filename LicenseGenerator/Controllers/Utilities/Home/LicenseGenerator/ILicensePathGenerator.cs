using LicenseGenerator.ViewModels;

namespace LicenseGenerator.Controllers.Utilities.Home.LicenseGenerator
{
    public interface ILicensePathGenerator
    {
        string GenerateLicenseToPath(LicenseViewModel license);
        void SaveLicenseHistory(LicenseViewModel license, bool isEncrypted);
        string ServerLicensesDirectory { get; }
        string GenerateZippedLicense(LicenseViewModel licenseViewModel);
    }
}