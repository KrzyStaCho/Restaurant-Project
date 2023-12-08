using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectLibrary.Repository.Entity;

[PrimaryKey("RecipeId", "ProductId")]
[Table("RecipeDetail")]
public partial class RecipeDetail
{
    [Key]
    [Column("RecipeID")]
    public int RecipeId { get; set; }

    [Key]
    [Column("ProductID")]
    public int ProductId { get; set; }

    [Column(TypeName = "decimal(9, 4)")]
    public decimal Quantity { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("RecipeDetails")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("RecipeId")]
    [InverseProperty("RecipeDetails")]
    public virtual Recipe Recipe { get; set; } = null!;
}
