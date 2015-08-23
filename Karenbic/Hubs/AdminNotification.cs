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
                int count1 = context.DesignOrders
                    .Count(x => x.IsCanceled == false &&
                        x.IsConfirm == false);

                int count2 = context.DesignOrders
                    .Count(x => x.IsCanceled == false &&
                        (x.IsPaidPrepayment == true || x.IsPaidFinal == true) &&
                        x.IsAcceptDesign == false &&
                        x.AdminMustSeeIt == true);

                int count3 = context.DesignOrders
                    .Count(x => x.IsCanceled == false &&
                        x.IsAcceptDesign == true &&
                        x.IsSendFinalDesign == false);

                count = count1 + count2 + count3;
            }

            Clients.Caller.getUnCheckedDesignOrders(count);
        }

        public void GetNewDesignOrders()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                count = context.DesignOrders
                    .Count(x => x.IsCanceled == false &&
                        x.IsConfirm == false);
            }

            Clients.Caller.getNewDesignOrders(count);
        }

        public void GetUnCheckedOngoingDesignOrders()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                count = context.DesignOrders
                    .Count(x => x.IsCanceled == false &&
                        (x.IsPaidPrepayment == true || x.IsPaidFinal == true) && 
                        x.IsAcceptDesign == false &&
                        x.AdminMustSeeIt == true);
            }

            Clients.Caller.getUnCheckedOngoingDesignOrders(count);
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

        public void GetUnCheckedPrintOrders()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                int count1 = context.PrintOrders
                    .Count(x => x.IsCanceled == false &&
                        x.IsConfirm == false);

                int count2 = context.PrintOrders
                    .Count(x => x.IsCanceled == false &&
                        x.OrderState == DomainClasses.PrintOrderState.Paid);

                count = count1 + count2;
            }

            Clients.Caller.getUnCheckedPrintOrders(count);
        }

        public void GetNewPrintOrders()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                count = context.PrintOrders
                    .Count(x => x.IsCanceled == false &&
                        x.IsConfirm == false);
            }

            Clients.Caller.getNewPrintOrders(count);
        }

        public void GetNewPaidPrintOrders()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                count = context.PrintOrders
                    .Count(x => x.IsCanceled == false &&
                        x.OrderState == DomainClasses.PrintOrderState.Paid);
            }

            Clients.Caller.getNewPaidPrintOrders(count);
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