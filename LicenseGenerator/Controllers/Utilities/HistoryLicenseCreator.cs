using System;
using LicenseGenerator.Models;
using LicenseGenerator.ViewModels;
using System.Web;
using System.Security.Principal;

namespace LicenseGenerator.Controllers.Utilities
{
    public class HistoryLicenseCreator
    {
        public GeneratedLicense GenerateLicense(LicenseViewModel licenseViewModel)
        {
            var vrlGeneratedLicense = new GeneratedLicense
            {
                AddionalInformation =  licenseViewModel.Company2,
                Company = licenseViewModel.Company1,
                GenerationDate = DateTime.Now,
                LicenseTermDate = licenseViewModel.Date,
                NIP = licenseViewModel.Nip,
                NumberOfStands = licenseViewModel.LicenseNumbers,
                PartnerNIP = licenseViewModel.PartnerNip,
                Privileges = licenseViewModel.Privileges,
                ProgramName = licenseViewModel.Name,
                ProgramVersion = licenseViewModel.ProgramVersion,
                UserName = ((WindowsIdentity)HttpContext.Current.User.Identity).Name,
                IsForClient = licenseViewModel.IsForClient,
                LicenseType = GetLicenseType(licenseViewModel)
            };

            if (vrlGeneratedLicense.LicenseTermDate != null)
            {
                vrlGeneratedLicense.LicenseTermDate = vrlGeneratedLicense.LicenseTermDate.Value.Date;
            }

            return vrlGeneratedLicense;
        }

        private LicenseType? GetLicenseType(LicenseViewModel licenseViewModel)
        {
            if (licenseViewModel.IsForClient)
            {
                if (licenseViewModel.Company2.StartsWith("Licencja testowa ważna do "))
                {
                    return LicenseType.Demo;
                }
                else if (licenseViewModel.Company2 == "Licencja bezterminowa")
                {
                    return LicenseType.Version;
                }
                else if (licenseViewModel.Company2.StartsWith("Licencja z abonamentem ważnym do "))
                {
                    return LicenseType.Date;
                }
            }

            return null;
        }

        public LicenseViewModel GenerateViewModel(GeneratedLicense generatedLicense)
        {
            var licenseViewModel = new LicenseViewModel
            {
                Company2 = generatedLicense.AddionalInformation,
                Company1 = generatedLicense.Company,
                Date = generatedLicense.LicenseTermDate,
                Nip = generatedLicense.NIP,
                LicenseNumbers = generatedLicense.NumberOfStands,
                PartnerNip = generatedLicense.PartnerNIP,
                Privileges = generatedLicense.Privileges,
                Name = generatedLicense.ProgramName,
                ProgramVersion = generatedLicense.ProgramVersion,
                IsForClient = generatedLicense.IsForClient,
            };

            return licenseViewModel;
        }
    }
}