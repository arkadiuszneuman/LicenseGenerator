using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LicenseGenerator.ViewModels
{
    public class LicenseViewModel
    {
        public string Company1 { get; set; }
        public string Company2 { get; set; }
        public string Name { get; set; }
        public string Nip { get; set; }
        public DateTime? Date { get; set; }
        public string Privileges { get; set; }
        public int? LicenseNumbers { get; set; }
        public string ProgramVersion { get; set; }
        public string ProgramNip { get; set; }
    }
}