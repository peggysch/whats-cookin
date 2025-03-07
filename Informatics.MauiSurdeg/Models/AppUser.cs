using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informatics.MauiSurdeg.Models;

[Table("AppUser")]
public class AppUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("AppUserID")]
    public int Id { get; set; }

    [Required]
    [Column("UserNo")]
    [MaxLength(50)]
    public string Number { get; set; }

    [Required]
    [Column("Username")]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    [Column("PasswordHash")]
    public byte[] PasswordHash { get; set; }

    // Navigation property for the one-to-many relationship with Recipe
    public virtual ICollection<Recipe> Recipes { get; set; }

    // Navigation property for associated ratings
    public virtual ICollection<Rating> Ratings { get; set; }

    // Navigation property for associated comments
    public virtual ICollection<Comment> Comments { get; set; }
}

