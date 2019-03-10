namespace Lusa.UI.Msic.DataProtectService
{
    public interface IDataProtectService
    {
        byte[] ProtectData(byte[] data, string key = "");
        byte[] UnprotectData(byte[] data, string key = "");
    }
}
