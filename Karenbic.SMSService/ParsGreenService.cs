using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karenbic.SMSService
{
    public class ParsGreenService : ISMSService
    {
        public SendSMSState Send(string[] to, string text, bool isFlash)
        {
            int returnValue = PARSGREEN.SMS.SendGroupSmsSimple("6B630D84-10A6-4AF4-8530-B8C57C31075A", 
                                                               "500028354620", to, text, isFlash, "");

            switch (returnValue)
            {
                case 1:
                    return SendSMSState.Success;

                case 0:
                    return SendSMSState.Unsuccess;

                default:
                    return SendSMSState.Error;
            }
        }

        public DeliverySMSState Delivery(string id)
        {
            throw new NotImplementedException();
        }

        public DeleteSMSState Delete(string id)
        {
            throw new NotImplementedException();
        }

        public DeleteSMSState DeleteAll()
        {
            throw new NotImplementedException();
        }
    }
}
