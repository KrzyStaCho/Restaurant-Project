using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectLibrary.Repository.Entity
{
    [Table("AccountArchive")]
    public partial class AccountArchive
    {
        [StringLength(50)]
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        [StringLength(50)]
        public string GroupName { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime? LastOnline { get; set; }
        [StringLength(50)]
        public string WhoChanged { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime LastModified { get; set; }
        [Column("AccountID")]
        public int AccountId { get; set; }
        [Key]
        [Column("ArchiveID")]
        public int ArchiveId { get; set; }
    }
}
