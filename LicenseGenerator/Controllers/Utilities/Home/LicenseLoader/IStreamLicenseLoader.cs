using System.IO;

namespace LicenseGenerator.Controllers.Utilities.Home.LicenseLoader
{
    public interface IStreamLicenseLoader
    {
        string LoadLicense(Stream stream);
    }
}