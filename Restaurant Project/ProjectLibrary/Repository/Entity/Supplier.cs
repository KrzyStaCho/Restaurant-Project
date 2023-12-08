using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectLibrary.Repository.Entity;

[Table("Supplier")]
public partial class Supplier
{
    [Key]
    [Column("SupplierID")]
    public int SupplierId { get; set; }

    [StringLength(50)]
    public string CompanyName { get; set; } = null!;

    [Column("CompanyNIP")]
    [StringLength(10)]
    public string CompanyNip { get; set; } = null!;

    [StringLength(60)]
    public string Address { get; set; } = null!;

    [StringLength(50)]
    public string City { get; set; } = null!;

    [StringLength(50)]
    public string? ContactName { get; set; }

    [StringLength(50)]
    public string? ContactTitle { get; set; }

    [StringLength(24)]
    public string? Phone { get; set; }

    public string? HomePage { get; set; }

    [StringLength(50)]
    public string WhoChanged { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime LastModified { get; set; }

    [InverseProperty("Supplier")]
    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
