using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectLibrary.Repository.Entity
{
    [Table("Account")]
    public partial class Account
    {
        [Key]
        [Column("AccountID")]
        public int AccountId { get; set; }
        [StringLength(50)]
        public string Username { get; set; } = null!;
        [Column("GroupID")]
        public int GroupId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LastOnline { get; set; }
        [Required]
        public bool? CanChanged { get; set; }
        [StringLength(50)]
        public string WhoChanged { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime LastModified { get; set; }

        [ForeignKey(nameof(GroupId))]
        [InverseProperty(nameof(AccountGroup.Accounts))]
        public virtual AccountGroup Group { get; set; } = null!;
        [InverseProperty("Account")]
        public virtual Password Password { get; set; } = null!;
    }
}
