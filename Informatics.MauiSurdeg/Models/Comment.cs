using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informatics.MauiSurdeg.Models;

[Table("Comment")]
public class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("CommentID")]
    public int CommentID { get; set; }

    [Required]
    [Column("CommentNo")]
    [MaxLength(50)]
    public string CommentNo { get; set; }

    [Required]
    [Column("AppUserID")]
    public int AppUserID { get; set; }

    [Required]
    [Column("RecipeID")]
    public int RecipeID { get; set; }

    [Required]
    [Column("CommentText")]
    public string CommentText { get; set; }

    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    [ForeignKey("AppUserID")]
    public virtual AppUser AppUser { get; set; }
    
    [ForeignKey("RecipeID")]
    public virtual Recipe Recipe { get; set; }
}