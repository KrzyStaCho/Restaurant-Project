using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectLibrary.Repository.Entity;

[Table("Permission")]
public partial class Permission
{
    [Key]
    [Column("PermissionID")]
    public int PermissionId { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [ForeignKey("PermissionId")]
    [InverseProperty("Permissions")]
    public virtual ICollection<AccountGroup> Groups { get; } = new List<AccountGroup>();
}
