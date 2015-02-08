using LicenseGenerator.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LicenseGenerator.Controllers.Utilities.Home
{
    public class ProductNewestVersionLoader : INewestVersionLoader
    {
        public string LoadNewestVersion(string program)
        {
            using (LicenseGeneratorContext context = new LicenseGeneratorContext())
            {
                return context.Products.Where(c => c.LicenseName.ToLower() == program)
                    .Where(c => c.NewestVersion != null && c.NewestVersion != "").Select(l => l.NewestVersion).SingleOrDefault();
            }
        }
    }
}