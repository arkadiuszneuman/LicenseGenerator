using System.Collections.Generic;
using LicenseGenerator.Models;

namespace LicenseGenerator.Controllers.Utilities.Home
{
    public interface IPatternCustomersLoader
    {
        IEnumerable<Customer> LoadCustomers(string pattern);
    }
}