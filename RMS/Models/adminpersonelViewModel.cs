using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS.Models
{
    public class adminpersonelViewModel :IdentityUser
    {
        public List<Personeller> Personeller { get; set;}
        public List<Adminler> Adminler { get; set;} 



    }
}