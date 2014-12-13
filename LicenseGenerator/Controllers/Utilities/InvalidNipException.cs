using System;

namespace LicenseGenerator.Controllers.Utilities
{
    public class InvalidNipException : Exception
    {
        public InvalidNipException() : base("Podany NIP jest nieprawid³owy")
        {
        }
    }
}