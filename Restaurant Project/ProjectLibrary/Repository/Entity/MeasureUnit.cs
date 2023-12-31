﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectLibrary.Repository.Entity;

[Table("MeasureUnit")]
public partial class MeasureUnit
{
    [Key]
    [Column("UnitID")]
    public int UnitId { get; set; }

    [StringLength(5)]
    public string Code { get; set; } = null!;

    [InverseProperty("Unit")]
    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
