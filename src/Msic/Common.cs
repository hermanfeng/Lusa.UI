using Lusa.UI.Msic.MessageService;

namespace Lusa.UI.Msic
{
    public class UI
    {
        public static void SendProgressMessage(int progress, string msg)
        {
            MessageService.MessageService.Instance.SendProgressMessage(progress, null, msg);
        }
    }
}
