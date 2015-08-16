using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Karenbic.Hubs
{
    [HubName("adminNotification")]
    public class AdminNotification : Hub
    {
        public void GetUnCheckedDesignOrders()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                count = context.DesignOrders
                    .Count(x => x.IsCanceled == false &&
                        x.IsAcceptDesign == true &&
                        x.IsSendFinalDesign == false);
            }

            Clients.Caller.getUnCheckedDesignOrders(count);
        }

        public void GetUnSendedFinalDesignOfDesignOrders()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                count = context.DesignOrders
                    .Count(x => x.IsCanceled== false && 
                        x.IsAcceptDesign == true && 
                        x.IsSendFinalDesign == false);
            }

            Clients.Caller.getUnSendedFinalDesignOfDesignOrders(count);
        }

        public void GetUnReadCustomerMessage()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                count = context.CustomerMessages
                    .Count(x => x.IsReadAdmin == false && x.IsShowAdmin == true);
            }

            Clients.Caller.getUnReadCustomerMessage(count);
        }
    }
}