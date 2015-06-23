using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Karenbic.UserInfrastructure
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() {}
        public ApplicationRole(string name) : base(name) { }

        // additional properties will go here
    }
}