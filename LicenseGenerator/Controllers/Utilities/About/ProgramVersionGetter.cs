using System;
using System.Reflection;

namespace LicenseGenerator.Controllers.Utilities.About
{
    public class ProgramVersionGetter : IVersionGetter
    {
        public string GetVersion()
        {
            Assembly reference = typeof(HomeController).Assembly;
            Version version = reference.GetName().Version;

            return version.Major + "." + version.Minor;
        }
    }
}