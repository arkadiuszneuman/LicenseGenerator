using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LicenseGenerator.Controllers.Utilities.Home
{
    public interface INewestVersionLoader
    {
        string LoadNewestVersion(string someObject);
    }
}