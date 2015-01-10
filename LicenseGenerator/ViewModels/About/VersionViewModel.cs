namespace LicenseGenerator.ViewModels.About
{
    public class VersionViewModel
    {
        public VersionViewModel(string version)
        {
            Version = version;
        }

        public string Version { get; private set; } 
    }
}