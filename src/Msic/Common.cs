using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class UI
    {
        public static void SendProgressMessage(int progress, string msg)
        {
            MessageService.Instance.SendProgressMessage(progress, null, msg);
        }
    }
}
