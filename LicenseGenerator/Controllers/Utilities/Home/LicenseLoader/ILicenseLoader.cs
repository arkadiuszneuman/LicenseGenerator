using System.Web;
using LicenseGenerator.Controllers.Utilities.JsonConverter;

namespace LicenseGenerator.Controllers.Utilities.Home.LicenseLoader
{
    public interface ILicenseLoader
    {
        SuccessObject LoadLicense(HttpPostedFileBase uploadedLicense);
    }
}