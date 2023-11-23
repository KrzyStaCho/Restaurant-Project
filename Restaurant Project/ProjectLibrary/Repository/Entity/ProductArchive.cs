using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectLibrary.Repository.Entity
{
    [Keyless]
    [Table("ProductArchive")]
    public partial class ProductArchive
    {
        [StringLength(50)]
        public string ProductName { get; set; } = null!;
        [StringLength(5)]
        public string UnitCode { get; set; } = null!;
        [StringLength(50)]
        public string? CategoryName { get; set; }
        [StringLength(50)]
        public string? SupplierName { get; set; }
        [StringLength(50)]
        public string WhoChanged { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime LastModified { get; set; }
    }
}
