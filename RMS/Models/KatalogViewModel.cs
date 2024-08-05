using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMS.Models
{
    public class KatalogViewModel
    {

        public List<YemekKatalog> YemekKataloglar { get; set; }
        public List<YemekOzellik> YemekOzellikler { get; set; }
        public List<Katalog_Ozellik> Katalog_Ozellikler { get; set; }

        public List<Menu> Menuler { get; set; }
        public IEnumerable<SelectListItem> Kategoriler { get; set; }


    }
}