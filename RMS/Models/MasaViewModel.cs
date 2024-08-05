using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS.Models
{
    public class MasaViewModel
    {
        public List<Menu> Menu { get; set; }
        public List<Masalar> Masalar { get; set; }
        public List<Personeller> Personeller { get; set; }
        public List<Adminler> Adminler { get; internal set; }
        public List<Siparisler> Siparisler { get; set;} 
        public List<YemekKatalog> YemekKatalog { get; set; }
        public List<YemekOzellik> YemekOzellik { get; set; }
        public List<Katalog_Ozellik> Katalog_Ozellikler { get; set; }
    }
}