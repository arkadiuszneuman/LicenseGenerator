using System;
using LicenseGenerator.Models;
using LicenseGenerator.ViewModels;

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
                ProgramVersion = licenseViewModel.ProgramVersion
            };

            if (vrlGeneratedLicense.LicenseTermDate != null)
            {
                vrlGeneratedLicense.LicenseTermDate = vrlGeneratedLicense.LicenseTermDate.Value.Date;
            }

            return vrlGeneratedLicense;
        }
    }
}