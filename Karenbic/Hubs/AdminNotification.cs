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