using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informatics.MauiSurdeg.Models;

[Table("RecipeIngredient")]
public class RecipeIngredient
{
    // No [Key] attributes here! The composite key will be configured in OnModelCreating
    [Column("RecipeID")]
    public int RecipeID { get; set; }

    [Column("IngredientID")]
    public int IngredientID { get; set; }

    [Required]
    [Column("Quantity")]
    [MaxLength(50)]
    public string Quantity { get; set; }

    // Navigation properties
    [ForeignKey("RecipeID")]
    public virtual Recipe Recipe { get; set; }
    
    [ForeignKey("IngredientID")]
    public virtual Ingredient Ingredient { get; set; }
}