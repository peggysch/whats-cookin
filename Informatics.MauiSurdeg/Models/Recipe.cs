using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informatics.MauiSurdeg.Models;


[Table("Recipe")]
public class Recipe
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("RecipeID")]
    public int Id { get; set; }

    [Required]
    [Column("RecipeNo")]
    [MaxLength(50)]
    public string Number { get; set; }

    [Required]
    [Column("Title")]
    [MaxLength(255)]
    public string Title { get; set; }

    [Required]
    [Column("RecipeDescription")]
    public string Description { get; set; }

    [Required]
    [Column("CookingTime")]
    public int CookingTime { get; set; }

    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required]
    [Column("AppUserID")]
    public int AppUserId { get; set; }

    [ForeignKey("AppUserId")]
    public virtual AppUser AppUser { get; set; }

    // Navigation property to access the associated Ratings
    public virtual ICollection<Rating> Ratings { get; set; }

    // Navigation property to access the associated RecipeIngredients
    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }

    // Navigation property for associated comments
    public virtual ICollection<Comment> Comments { get; set; }

    // Define the navigation property for Ingredients
    public ICollection<Ingredient> Ingredients { get; set; }
}
