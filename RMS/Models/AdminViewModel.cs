using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS.Models.Managers
{
    public class AdminViewModel
    {

        public List<Personeller> Personeller { get; set; } 
        public List<Adminler> Adminler { get; set; }
        public List<Masalar> Masalar { get; set; }
        public List<Menu> Menu { get; set; }
        public List<Siparisler> Siparisler { get; set; }
        public List<YemekKatalog> Kataloglar { get; set; }
        public List<YemekOzellik> Ozellikler { get; set; }
        public AdminViewModel() { }


    }
}