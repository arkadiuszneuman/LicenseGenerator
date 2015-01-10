using LicenseGenerator.ViewModels;

namespace LicenseGenerator.Controllers.Utilities.Home.LicenseLoader
{
    public interface ILicenseToViewModelConverter
    {
        LicenseViewModel CreateVMFromLicense(string vrpLicense);
    }
}