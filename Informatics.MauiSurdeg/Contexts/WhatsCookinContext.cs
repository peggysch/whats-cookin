using Microsoft.EntityFrameworkCore;
using Informatics.MauiSurdeg.Models;

namespace Informatics.MauiSurdeg.Contexts
{
    public class WhatsCookinContext : DbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public WhatsCookinContext(DbContextOptions<WhatsCookinContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  // Call base first as best practice

            // Configure the composite key for RecipeIngredient
            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeID, ri.IngredientID });

            // AppUser and Recipe relationship
            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.AppUser)
                .WithMany(u => u.Recipes)
                .HasForeignKey(r => r.AppUserID)
                .OnDelete(DeleteBehavior.Cascade);

            // RecipeIngredient relationships - properly configured for a junction table with data
            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeID);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientID);

            // Rating relationships
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Recipe)
                .WithMany(r => r.Ratings)
                .HasForeignKey(r => r.RecipeID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.AppUser)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.AppUserID)
                .OnDelete(DeleteBehavior.ClientCascade); // Using ClientCascade to avoid multiple cascade paths

            // Comment relationships
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Recipe)
                .WithMany(r => r.Comments)
                .HasForeignKey(c => c.RecipeID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.AppUser)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AppUserID)
                .OnDelete(DeleteBehavior.ClientCascade); // Using ClientCascade to avoid multiple cascade paths

            // Configure unique constraints
            modelBuilder.Entity<AppUser>().HasIndex(u => u.UserNo).IsUnique();
            modelBuilder.Entity<AppUser>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Recipe>().HasIndex(r => r.RecipeNo).IsUnique();
            modelBuilder.Entity<Ingredient>().HasIndex(i => i.IngredientNo).IsUnique();
            modelBuilder.Entity<Rating>().HasIndex(r => r.RatingNo).IsUnique();
            modelBuilder.Entity<Comment>().HasIndex(c => c.CommentNo).IsUnique();
            
            // Rating unique constraint (one rating per user per recipe)
            modelBuilder.Entity<Rating>()
                .HasIndex(r => new { r.AppUserID, r.RecipeID })
                .IsUnique();
        }
    }
}