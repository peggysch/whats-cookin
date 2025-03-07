using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informatics.MauiSurdeg.Models;

[Table("RecipeIngredient")]
public class RecipeIngredient
{
    [Key]
    [Column(Order = 1)]
    [ForeignKey("Recipe")]
    [Required]
    public int RecipeId { get; set; }

    [Key]
    [Column(Order = 2)]
    [ForeignKey("Ingredient")]
    [Required]
    public int IngredientId { get; set; }

    [Required]
    [Column("Quantity")]
    [MaxLength(50)]
    public string Quantity { get; set; }

    public virtual Recipe Recipe { get; set; }
    public virtual Ingredient Ingredient { get; set; }
}
