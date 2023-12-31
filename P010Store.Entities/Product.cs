﻿using System.ComponentModel.DataAnnotations;

namespace P010Store.Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} Alanı Boş Geçilemez!"), StringLength(50), Display(Name = "ÜrünAdı")]
        public string Name { get; set; }
        [Display(Name = "Ürün Açıklaması")]
        public string? Description { get; set; }
        [StringLength(150), Display(Name = "Ürün Resmi")]
        public string? Image { get; set; }

        [Display(Name = "Eklenme Tarihi"), ScaffoldColumn(false)]// ScaffoldColumn(false) CreateDate property sini view oluşturulurken ekranda div inin oluşmamasını sağlar
        public DateTime? CreateDate { get; set; } = DateTime.Now; // Eğer bu alan boş geçilirse eklenme zamanını sistemden otomatik al

        [Display(Name = "Güncellenme Tarihi"), ScaffoldColumn(false)]
        public DateTime? UpdateDate { get; set; } = DateTime.Now;

        [Display(Name = "Durum")]
        public bool IsActive { get; set; }
        [Display(Name = "Anasayfa")]
        public bool IsHome { get; set; }
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }
        [Display(Name = "Stok")]
        public int Stock { get; set; }
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }
        [Display(Name = "Kategori")]
        public Category? Category { get; set; } // CodeFirst kullanarak product class ı ile Category class ı arasında 1 e 1 ilişki kurduk
        [Display(Name = "Marka")]
        public int BrandId { get; set; }
        [Display(Name = "Marka")]
        public Brand? Brand { get; set; }
    }
}
