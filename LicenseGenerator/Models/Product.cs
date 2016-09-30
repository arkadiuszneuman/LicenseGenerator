using System.Data.Entity.Spatial;

namespace LicenseGenerator.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string LicenseName { get; set; }
        public string NewestVersion { get; set; }
        public bool IsInStore { get; set; }
        public string Privileges { get; set; }
    }
}