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
    
    public partial class Katalog_Ozellik
    {
        public int Id { get; set; }
        public int KatalogId { get; set; }
        public int OzellikId { get; set; }
    
        public virtual YemekKatalog YemekKatalog { get; set; }
        public virtual YemekOzellik YemekOzellik { get; set; }
    }
}
