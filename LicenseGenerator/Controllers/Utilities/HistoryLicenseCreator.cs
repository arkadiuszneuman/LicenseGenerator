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
                UserName = ((WindowsIdentity)HttpContext.Current.User.Identity).Name
            };

            if (vrlGeneratedLicense.LicenseTermDate != null)
            {
                vrlGeneratedLicense.LicenseTermDate = vrlGeneratedLicense.LicenseTermDate.Value.Date;
            }

            return vrlGeneratedLicense;
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
                ProgramVersion = generatedLicense.ProgramVersion
            };

            return licenseViewModel;
        }
    }
}