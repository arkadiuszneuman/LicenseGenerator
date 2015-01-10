using System.IO;
using System.Security.Cryptography;
using System.Text;
using inSolutions.Utilities.Security;

namespace LicenseGenerator.Controllers.Utilities.Home.LicenseLoader
{
    public class StreamLicenseLoader : IStreamLicenseLoader
    {
        public string LoadLicense(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(1250)))
            {
                string license = reader.ReadToEnd();
                try
                {
                    license = Cl_DataEncryption.DecryptText(license);
                }
                catch (CryptographicException)
                {
                }

                return license;
            }
        }
    }
}