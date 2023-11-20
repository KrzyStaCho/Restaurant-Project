using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectLibrary.Repository.Entity
{
    [Table("SupplierArchive")]
    public partial class SupplierArchive
    {
        [StringLength(50)]
        public string CompanyName { get; set; } = null!;
        [StringLength(50)]
        public string? ContactName { get; set; }
        [StringLength(50)]
        public string? ContactTitle { get; set; }
        [StringLength(60)]
        public string? Address { get; set; }
        [StringLength(20)]
        public string? City { get; set; }
        [StringLength(24)]
        public string? Phone { get; set; }
        public string? HomePage { get; set; }
        [Column("SupplierID")]
        public int SupplierId { get; set; }
        [Key]
        [Column("ArchiveID")]
        public int ArchiveId { get; set; }
    }
}
