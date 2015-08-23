using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Karenbic.Hubs
{
    [HubName("customerNotification")]
    public class CustomerNotification : Hub
    {
        public static readonly ConcurrentDictionary<string, User> Users = new ConcurrentDictionary<string, User>();

        #region 

        public override Task OnConnected()
        {
            connect();
            return base.OnConnected();
        }

        private void connect()
        {
            var userName = "user"; // Context.User.Identity.Name;
            var connectionId = Context.ConnectionId;

            var user = Users.GetOrAdd(userName,
                _ => new User
                {
                    Name = userName,
                    ConnectionIds = new HashSet<string>()
                });
            lock (user.ConnectionIds)
            {
                user.ConnectionIds.Add(connectionId);
            }
        }

        public override Task OnReconnected()
        {
            connect();
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var userName = "user"; // Context.User.Identity.Name;
            var connectionId = Context.ConnectionId;

            User user;
            Users.TryGetValue(userName, out user);
            if (user != null)
            {
                lock (user.ConnectionIds)
                {
                    user.ConnectionIds.RemoveWhere(cid => cid.Equals(connectionId));

                    if (!user.ConnectionIds.Any())
                    {
                        User removedUser;
                        Users.TryRemove(userName, out removedUser);

                        ///Clients.Others.userDisconnected(userName);
                    }
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        #endregion

        public void GetUnpayedDesignBilling()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                //DomainClasses.Customer customer = context.Customers.Single(x => x.Username == User.Identity.Name);
                DomainClasses.Customer customer = context.Customers.Find(1);

                int count1 = context.DesignOrders
                    .Count(x => x.IsCanceled == false &&
                        x.Customer.Id == customer.Id &&
                        (x.PrepaymentFactor.IsPaid == false || x.FinalFactor.IsPaid == false));

                int count2 = context.FinancialConflicts
                    .Count(x => x.Portal == DomainClasses.Portal.Design && x.IsPaid == false && x.Customer.Id == customer.Id);

                count = count1 + count2;
            }

            Clients.Caller.getUnpayedDesignBilling(count);
        }

        public void GetUnpayedDesignFactor()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                //DomainClasses.Customer customer = context.Customers.Single(x => x.Username == User.Identity.Name);
                DomainClasses.Customer customer = context.Customers.Find(1);

                count = context.DesignOrders
                    .Count(x => x.IsCanceled == false && 
                        x.Customer.Id == customer.Id &&
                        (x.PrepaymentFactor.IsPaid == false || x.FinalFactor.IsPaid == false));
            }

            Clients.Caller.getUnpayedDesignFactor(count);
        }

        public void GetUnpayedDesignFinancialConflict()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                //DomainClasses.Customer customer = context.Customers.Single(x => x.Username == User.Identity.Name);
                DomainClasses.Customer customer = context.Customers.Find(1);

                count = context.FinancialConflicts
                    .Count(x => x.Portal == DomainClasses.Portal.Design && x.IsPaid == false && x.Customer.Id == customer.Id);
            }

            Clients.Caller.getUnpayedDesignFinancialConflict(count);
        }

        public void GetUnpayedPrintBilling()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                //DomainClasses.Customer customer = context.Customers.Single(x => x.Username == User.Identity.Name);
                DomainClasses.Customer customer = context.Customers.Find(1);

                int count1 = context.PrintFactors
                    .Count(x => x.IsPaid == false && x.Order.IsCanceled == false);

                int count2 = context.FinancialConflicts
                    .Count(x => x.Portal == DomainClasses.Portal.Print && x.IsPaid == false && x.Customer.Id == customer.Id);

                count = count1 + count2;
            }

            Clients.Caller.getUnpayedPrintBilling(count);
        }

        public void GetUnpayedPrintFactor()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                //DomainClasses.Customer customer = context.Customers.Single(x => x.Username == User.Identity.Name);
                DomainClasses.Customer customer = context.Customers.Find(1);

                count = context.PrintFactors
                    .Count(x => x.IsPaid == false && x.Order.IsCanceled == false);
            }

            Clients.Caller.getUnpayedPrintFactor(count);
        }

        public void GetUnpayedPrintFinancialConflict()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                //DomainClasses.Customer customer = context.Customers.Single(x => x.Username == User.Identity.Name);
                DomainClasses.Customer customer = context.Customers.Find(1);

                count = context.FinancialConflicts
                    .Count(x => x.Portal == DomainClasses.Portal.Print && x.IsPaid == false && x.Customer.Id == customer.Id);
            }

            Clients.Caller.getUnpayedPrintFinancialConflict(count);
        }

        public void GetUnreviewedDesign()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                //DomainClasses.Customer customer = context.Customers.Single(x => x.Username == User.Identity.Name);
                DomainClasses.Customer customer = context.Customers.Find(1);

                count = context.DesignOrders
                    .Count(x => x.Customer.Id == customer.Id && x.CustomerMustSeeIt == true);
            }

            Clients.Caller.getUnreviewedDesign(count);
        }

        public void GetUnReadMessage()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                //DomainClasses.Customer customer = context.Customers.Single(x => x.Username == User.Identity.Name);
                DomainClasses.Customer customer = context.Customers.Find(1);

                int count1 = context.CustomerMessages
                    .Count(x => x.Sender.Id == customer.Id && x.IsShowCustomer == true && x.IsReadCustomer == false);

                int count2 = context.AdminMessages_Customer
                    .Count(x => x.Customer.Id == customer.Id && x.IsRead == false);

                count = count1 + count2;
            }

            Clients.Caller.getUnReadMessage(count);
        }

        public void GetUnReadReplyMessage()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                //DomainClasses.Customer customer = context.Customers.Single(x => x.Username == User.Identity.Name);
                DomainClasses.Customer customer = context.Customers.Find(1);

                count = context.CustomerMessages
                    .Count(x => x.Sender.Id == customer.Id && x.IsShowCustomer == true && x.IsReadCustomer == false);
            }

            Clients.Caller.getUnReadReplyMessage(count);
        }

        public void GetUnReadAdminMessage()
        {
            int count = 0;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                //DomainClasses.Customer customer = context.Customers.Single(x => x.Username == User.Identity.Name);
                DomainClasses.Customer customer = context.Customers.Find(1);

                count = context.AdminMessages_Customer
                    .Count(x => x.Customer.Id == customer.Id && x.IsRead == false);
            }

            Clients.Caller.getUnReadAdminMessage(count);
        }
    }
}