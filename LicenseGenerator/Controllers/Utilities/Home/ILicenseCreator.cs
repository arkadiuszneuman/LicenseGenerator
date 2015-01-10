using LicenseGenerator.ViewModels;

namespace LicenseGenerator.Controllers.Utilities.Home
{
    public interface ILicenseCreator
    {
        string CreateLicenseFromVM(LicenseViewModel license);
    }
}