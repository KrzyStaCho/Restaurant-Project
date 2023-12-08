using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectLibrary.Repository.Entity;

[Table("Recipe")]
public partial class Recipe
{
    [Key]
    [Column("RecipeID")]
    public int RecipeId { get; set; }

    [StringLength(50)]
    public string RecipeName { get; set; } = null!;

    [Column("CategoryID")]
    public int? CategoryId { get; set; }

    [StringLength(50)]
    public string WhoChanged { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime LastModified { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Recipes")]
    public virtual RecipeCategory? Category { get; set; }

    [InverseProperty("Recipe")]
    public virtual ICollection<RecipeDetail> RecipeDetails { get; } = new List<RecipeDetail>();
}
