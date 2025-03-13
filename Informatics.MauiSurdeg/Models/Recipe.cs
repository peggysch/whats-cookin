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
    public int RecipeID { get; set; }

    [Required]
    [Column("RecipeNo")]
    [MaxLength(50)]
    public string RecipeNo { get; set; }

    [Required]
    [Column("Title")]
    [MaxLength(255)]
    public string RecipeName { get; set; }

    [Required]
    [Column("RecipeDescription")]
    public string RecipeDescription { get; set; }

    [Required]
    [Column("Instructions")]
    public string Instructions { get; set; }

    [Required]
    [Column("CookingTime")]
    public int CookingTime { get; set; }

    [Required]
    [Column("CostPerPortion")]
    public decimal CostPerPortion { get; set; }

    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }

    [Required]
    [Column("AppUserID")]
    public int AppUserID { get; set; }

    // Navigation properties
    [ForeignKey("AppUserID")]
    public virtual AppUser AppUser { get; set; }
    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}