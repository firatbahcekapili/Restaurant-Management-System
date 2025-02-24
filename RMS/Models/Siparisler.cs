//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Siparisler
    {
        public int SiparisId { get; set; }
        public Nullable<int> MasaId { get; set; }
        public Nullable<int> YemekId { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public string Durum { get; set; }
        [Required]
        public string YemekAd { get; set; }
        public Nullable<int> PersonelId { get; set; }
        public Nullable<int> AdminId { get; set; }

        public Nullable<decimal> YemekFiyat { get; set; }
        public int Adet { get; set; }
        public string AdisyonId { get; set; }
        public string YemekOzellikJson { get; set; }

        public virtual Masalar Masalar { get; set; }
        public virtual Adminler Adminler { get; set; }
        public virtual Personeller Personeller { get; set; }
        public virtual Menu Menu { get; set; }
    }
}
