namespace LicenseGenerator.Controllers.Utilities.JsonConverter
{
    public class SuccessObject
    {
        public SuccessObject(bool success, object obj)
        {
            Success = success;
            Object = obj;
        }

        public bool Success { get; private set; }
        public object Object { get; private set; }
    }
}