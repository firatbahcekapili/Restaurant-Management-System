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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Personeller
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Personeller()
        {
            this.Siparisler = new HashSet<Siparisler>();
        }

        public int PersonelId { get; set; }
        [Required(ErrorMessage = "İsim alanı gereklidir.")]
        public string Ad { get; set; }
        [Required(ErrorMessage = "Soyisim alanı gereklidir.")]
        public string Soyad { get; set; }
        [Required(ErrorMessage = "Kullanıcı adı giriniz.")]
        [DisplayName("Kullanıcı Adı")]
        public string KullaniciAdi { get; set; }
        [Required(ErrorMessage = "Şifre giriniz.")]
        public string Sifre { get; set; }
        public string Rol { get; set; }
        public string PersonelResim { get; set; }
        [Required(ErrorMessage = "Cinsiyet seçiniz.")]
        public string Cinsiyet { get; set; }
        [Required(ErrorMessage = "Maaş alanı gereklidir")]
        public Nullable<double> Maaş { get; set; }
        public Nullable<bool> Durum { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Siparisler> Siparisler { get; set; }
    }
}
