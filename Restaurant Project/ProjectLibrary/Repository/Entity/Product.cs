using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectLibrary.Repository.Entity
{
    [Table("Product")]
    public partial class Product
    {
        [Key]
        [Column("ProductID")]
        public int ProductId { get; set; }
        [StringLength(50)]
        public string ProductName { get; set; } = null!;
        [Column(TypeName = "decimal(9, 4)")]
        public decimal InStock { get; set; }
        [Column("UnitID")]
        public int UnitId { get; set; }
        [Column("CategoryID")]
        public int? CategoryId { get; set; }
        [Column("SupplierID")]
        public int? SupplierId { get; set; }
        [StringLength(50)]
        public string WhoChanged { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime LastModified { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty(nameof(ProductCategory.Products))]
        public virtual ProductCategory? Category { get; set; }
        [ForeignKey(nameof(SupplierId))]
        [InverseProperty("Products")]
        public virtual Supplier? Supplier { get; set; }
        [ForeignKey(nameof(UnitId))]
        [InverseProperty(nameof(MeasureUnit.Products))]
        public virtual MeasureUnit Unit { get; set; } = null!;
    }
}
