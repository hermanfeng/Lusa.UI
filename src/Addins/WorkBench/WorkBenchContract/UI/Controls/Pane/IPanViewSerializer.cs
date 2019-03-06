namespace Lusa.UI.WorkBenchContract.UI.Controls.Pane
{
    public interface IPanViewSerializer
    {
        string Serializer();
        void DeSerializer(string data);
    }
}