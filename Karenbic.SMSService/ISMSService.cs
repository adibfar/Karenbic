using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karenbic.SMSService
{
    public interface ISMSService
    {
        SendSMSState Send(string[] to, string text, bool isFlash);
        DeliverySMSState Delivery(string id);
        DeleteSMSState Delete(string id);
        DeleteSMSState DeleteAll();
    }
}
