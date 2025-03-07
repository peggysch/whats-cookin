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
    public int Id { get; set; }

    [Required]
    [Column("CommentNo")]
    [MaxLength(50)]
    public string Number { get; set; }

    [Required]
    [Column("AppUserID")]
    public int AppUserId { get; set; }

    [Required]
    [Column("RecipeID")]
    public int RecipeId { get; set; }

    [Required]
    [Column("CommentText")]
    public string Text { get; set; }

    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("AppUserId")]
    public virtual AppUser AppUser { get; set; }

    [ForeignKey("RecipeId")]
    public virtual Recipe Recipe { get; set; }
}
