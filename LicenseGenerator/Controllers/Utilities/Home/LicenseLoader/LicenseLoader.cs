using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using inSolutions.Utilities.Security;
using LicenseGenerator.Controllers.Utilities.JsonConverter;
using LicenseGenerator.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LicenseGenerator.Controllers.Utilities.Home.LicenseLoader
{
    public class LicenseLoader : ILicenseLoader
    {
        private readonly ILicenseToViewModelConverter licenseToViewModelConverter;
        private readonly IStreamLicenseLoader streamLicenseLoader;

        public LicenseLoader(ILicenseToViewModelConverter licenseToViewModelConverter, IStreamLicenseLoader streamLicenseLoader)
        {
            this.licenseToViewModelConverter = licenseToViewModelConverter;
            this.streamLicenseLoader = streamLicenseLoader;
        }

        public SuccessObject LoadLicense(HttpPostedFileBase uploadedLicense)
        {
            try
            {
                if (uploadedLicense != null && uploadedLicense.ContentLength > 0 && uploadedLicense.ContentType == "text/plain")
                {
                    string license = streamLicenseLoader.LoadLicense(uploadedLicense.InputStream);
                    var vrlLicenseViewModel = licenseToViewModelConverter.CreateVMFromLicense(license);
                
                    return new SuccessObject(true, vrlLicenseViewModel);
                }

                return new SuccessObject(false, "Nieprawidłowy typ pliku.");
            }
            catch (Exception e)
            {
                return new SuccessObject(false, e.Message);
            }
        }
    }
}