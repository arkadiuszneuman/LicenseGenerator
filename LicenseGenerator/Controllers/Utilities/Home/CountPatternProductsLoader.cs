using System.Collections.Generic;
using System.Linq;
using LicenseGenerator.DAL;
using LicenseGenerator.Models;

namespace LicenseGenerator.Controllers.Utilities.Home
{
    public class CountPatternProductsLoader : IPatternProductsLoader
    {
        private readonly int count;

        public CountPatternProductsLoader(int count)
        {
            this.count = count;
        }

        public IEnumerable<Product> LoadProducts(string pattern)
        {
            using (LicenseGeneratorContext context = new LicenseGeneratorContext())
            {
                return context.Products.Where(c => c.LicenseName.Contains(pattern) || c.Name.Contains(pattern))
                    .Where(c => c.LicenseName != "" && !c.IsInStore).Distinct().OrderBy(p => p.LicenseName).Take(count).ToList();
            }
        }
    }
}