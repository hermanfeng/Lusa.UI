namespace Lusa.UI.Msic.DataProtectService
{
    public interface IDataProtectService
    {
        string ProtectData(string input, string key = "");
        string UnprotectData(string input, string key = "");
    }
}
