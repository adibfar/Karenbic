using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Karenbic.UserInfrastructure
{
    public class ApplicationDbInitializer : NullDatabaseInitializer<ApplicationDbContext>
    {
    }

    //public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    //{
    //    protected override void Seed(ApplicationDbContext context)
    //    {
    //        PerformInitialSetup(context);
    //        base.Seed(context);
    //    }

    //    public static void PerformInitialSetup(ApplicationDbContext db)
    //    {
    //        // initial configuration will go here
    //    }
    //}
}