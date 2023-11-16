using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectLibrary.Repository.Entity
{
    [Table("AccountGroup")]
    public partial class AccountGroup
    {
        public AccountGroup()
        {
            Accounts = new HashSet<Account>();
            Permissions = new HashSet<Permission>();
        }

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

        [InverseProperty(nameof(Account.Group))]
        public virtual ICollection<Account> Accounts { get; set; }

        [ForeignKey("GroupId")]
        [InverseProperty(nameof(Permission.Groups))]
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
