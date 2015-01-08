using System;
using LicenseGenerator.ViewModels;

namespace LicenseGenerator.Controllers.Utilities
{
    public class LicenseLoader
    {
        public LicenseViewModel CreateVMFromLicense(string vrpLicense)
        {
            string[] vrlStrings = vrpLicense.Split(new [] { Environment.NewLine }, StringSplitOptions.None);
            LicenseViewModel vrlViewModel = new LicenseViewModel();
            if (vrlStrings.Length >= 0 && !string.IsNullOrEmpty(vrlStrings[0]))
            {
                vrlViewModel.Name = vrlStrings[0];
            }
            if (vrlStrings.Length > 1 && !string.IsNullOrEmpty(vrlStrings[1]))
            {
                vrlViewModel.Nip = vrlStrings[1];
            }

            if (vrlStrings.Length > 2)
            {
                vrlViewModel.Privileges = vrlStrings[2] == "0" ? null : "";
            }

            if (vrlStrings.Length > 3 && !string.IsNullOrEmpty(vrlStrings[3]))
            {
                vrlViewModel.Company1 = vrlStrings[3];
            }
            if (vrlStrings.Length > 4 && !string.IsNullOrEmpty(vrlStrings[4]))
            {
                vrlViewModel.Company2 = vrlStrings[4];
            }
            if (vrlStrings.Length > 5 && !string.IsNullOrEmpty(vrlStrings[5]))
            {
                vrlViewModel.Date = Convert.ToDateTime(vrlStrings[5]);
            }

            if (vrlStrings.Length > 6 && !string.IsNullOrEmpty(vrlStrings[6]))
            {
                vrlViewModel.LicenseNumbers = Convert.ToInt32(vrlStrings[6]);
            }

            if (vrlStrings.Length > 7 && !string.IsNullOrEmpty(vrlStrings[7]))
            {
                vrlViewModel.ProgramVersion = vrlStrings[7];
            }

            return vrlViewModel;
        }
    }
}