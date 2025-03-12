using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informatics.MauiSurdeg.Models;

[Table("Rating")]
public class Rating
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("RatingID")]
    public int RatingID { get; set; }

    [Required]
    [Column("RatingNo")]
    [MaxLength(50)]
    public string RatingNo { get; set; }

    [Required]
    [Column("AppUserID")]
    public int AppUserID { get; set; }

    [Required]
    [Column("RecipeID")]
    public int RecipeID { get; set; }

    [Required]
    [Column("Stars")]
    [Range(1, 5)]
    public int Stars { get; set; }

    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    [ForeignKey("AppUserID")]
    public virtual AppUser AppUser { get; set; }
    
    [ForeignKey("RecipeID")]
    public virtual Recipe Recipe { get; set; }
}