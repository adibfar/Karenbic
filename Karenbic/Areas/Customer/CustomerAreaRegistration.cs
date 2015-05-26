using System.Web.Mvc;

namespace Karenbic.Areas.Customer
{
    public class CustomerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Customer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Customer_default",
                url: "Customer/{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new string[] { "Karenbic.Areas.Customer.Controllers" }
            );
        }
    }
}