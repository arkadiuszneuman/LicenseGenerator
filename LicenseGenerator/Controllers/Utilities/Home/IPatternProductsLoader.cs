using System.Collections.Generic;
using LicenseGenerator.Models;

namespace LicenseGenerator.Controllers.Utilities.Home
{
    public interface IPatternProductsLoader
    {
        IEnumerable<Product> LoadProducts(string pattern);
    }
}