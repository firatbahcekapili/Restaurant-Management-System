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

    public partial class YemekKatalog
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public YemekKatalog()
        {
            this.Katalog_Ozellik = new HashSet<Katalog_Ozellik>();
        }

        public int KatalogId { get; set; }
        [Required(ErrorMessage = "Katalog ad� giriniz.")]
        
        public string KatalogAd { get; set; }
        public Nullable<bool> CokluSecim { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Katalog_Ozellik> Katalog_Ozellik { get; set; }
    }
}
