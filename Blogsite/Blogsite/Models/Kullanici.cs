namespace Blogsite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kullanici")]
    public partial class Kullanici
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kullanici()
        {
            Makalelers = new HashSet<Makaleler>();
            Yorums = new HashSet<Yorum>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(10)]
        public string KullaniciAdi { get; set; }

        [Required]
        [StringLength(10)]
        public string Sifre { get; set; }

        [StringLength(50)]
        public string isim { get; set; }

        [StringLength(50)]
        public string soyisim { get; set; }

        public int YetkiId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? KayitTarihi { get; set; }

        public virtual Yetki Yetki { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Makaleler> Makalelers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Yorum> Yorums { get; set; }
    }
}
