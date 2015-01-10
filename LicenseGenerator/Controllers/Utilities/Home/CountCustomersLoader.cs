using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using LicenseGenerator.DAL;
using LicenseGenerator.Models;
using LicenseGenerator.ViewModels;

namespace LicenseGenerator.Controllers.Utilities.Home
{
    public class CountPatternCustomersLoader : IPatternCustomersLoader
    {
        private readonly int count;

        public CountPatternCustomersLoader(int count)
        {
            this.count = count;
        }

        public IEnumerable<Customer> LoadCustomers(string pattern)
        {
            using (LicenseGeneratorContext context = new LicenseGeneratorContext())
            {
                return context.Companies.Where(c => c.Name.Contains(pattern) || c.Symbol.Contains(pattern)).Take(count).ToList();
            }
        }
    }
}