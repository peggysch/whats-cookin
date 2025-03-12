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
    public int AppUserID { get; set; }  // Changed from Id to match column name

    [Required]
    [Column("UserNo")]
    [MaxLength(50)]
    public string UserNo { get; set; }  // Changed from Number to match column name

    [Required]
    [Column("Username")]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    [Column("PasswordHash")]
    public byte[] PasswordHash { get; set; }

    // Navigation properties
    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}