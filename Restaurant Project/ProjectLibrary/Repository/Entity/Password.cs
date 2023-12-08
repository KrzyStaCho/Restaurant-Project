using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectLibrary.Repository.Entity;

[Table("Password")]
public partial class Password
{
    [Key]
    [Column("AccountID")]
    public int AccountId { get; set; }

    public string Value { get; set; } = null!;

    public bool CanChanged { get; set; }

    [StringLength(50)]
    public string WhoChanged { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime LastModified { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Password")]
    public virtual Account Account { get; set; } = null!;
}
