using System;

namespace LicenseGenerator.Models
{
    public class GeneratedLicense
    {
        public string ProgramName { get; set; }
        public string Company { get; set; }
        public string NIP { get; set; }
        public string Privileges { get; set; }
        public DateTime LicenseTermDate { get; set; }
        public string AddionalInformation { get; set; }
        public int NumberOfStands { get; set; }
        public string ProgramVersion { get; set; }
        public string PartnerNIP { get; set; }
    }
}