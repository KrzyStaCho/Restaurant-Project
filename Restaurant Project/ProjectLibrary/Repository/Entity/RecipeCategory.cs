using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectLibrary.Repository.Entity;

[Table("RecipeCategory")]
public partial class RecipeCategory
{
    [Key]
    [Column("CategoryID")]
    public int CategoryId { get; set; }

    [StringLength(50)]
    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Recipe> Recipes { get; } = new List<Recipe>();
}
