using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectLibrary.Repository.Entity;

[Table("AccountGroup")]
public partial class AccountGroup
{
    [Key]
    [Column("GroupID")]
    public int GroupId { get; set; }

    [StringLength(50)]
    public string GroupName { get; set; } = null!;

    public bool IsAdmin { get; set; }

    [Required]
    public bool? CanChanged { get; set; }

    [StringLength(50)]
    public string WhoChanged { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime LastModified { get; set; }

    [InverseProperty("Group")]
    public virtual ICollection<Account> Accounts { get; } = new List<Account>();

    [ForeignKey("GroupId")]
    [InverseProperty("Groups")]
    public virtual ICollection<Permission> Permissions { get; } = new List<Permission>();
}
