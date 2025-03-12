using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informatics.MauiSurdeg.Models;

[Table("Ingredient")]
public class Ingredient
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IngredientID")]
    public int IngredientID { get; set; }

    [Required]
    [Column("IngredientNo")]
    [MaxLength(50)]
    public string IngredientNo { get; set; }

    [Required]
    [Column("IngredientName")]
    [MaxLength(100)]
    public string IngredientName { get; set; }

    // Navigation property
    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}