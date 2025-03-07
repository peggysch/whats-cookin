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
    public int Id { get; set; }

    [Required]
    [Column("IngredientNo")]
    [MaxLength(50)]
    public string Number { get; set; }

    [Required]
    [Column("IngredientName")]
    [MaxLength(100)]
    public string Name { get; set; }

    // Navigation property for the many-to-many relationship with Recipe through RecipeIngredient
    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }

    // Define the navigation property for Recipes
    // public ICollection<Recipe> Recipes { get; set; }
}
