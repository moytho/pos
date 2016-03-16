using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.UserExtends
{
    public class CustomUser : IdentityUser
    {
        public CustomUser() { }
        public CustomUser(string userName)
            : base(userName)
        {

        }

        public int CodigoEmpresa { get; set; }
    }


    public class CustomUserDbContext : IdentityDbContext<CustomUser>
    {
        public CustomUserDbContext() : base("DefaultConnection") { }
    }
}