﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectLibrary.Repository.Entity;

[Table("ProductCategory")]
public partial class ProductCategory
{
    [Key]
    [Column("CategoryID")]
    public int CategoryId { get; set; }

    [StringLength(50)]
    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
