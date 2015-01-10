namespace LicenseGenerator.Controllers.Utilities.JsonConverter
{
    public interface IJsonConverter
    {
        string ConvertToJson(object obj);
    }
}