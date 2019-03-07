namespace Lusa.UI.WorkBenchContract.Controls.Pane
{
    public interface IPanViewSerializer
    {
        string Serializer();
        void DeSerializer(string data);
    }
}