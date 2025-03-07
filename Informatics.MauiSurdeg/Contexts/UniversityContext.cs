using Microsoft.EntityFrameworkCore;
using Informatics.MauiSurdeg.Models;

namespace Informatics.MauiSurdeg.Contexts
{
    public class RecipeContext : DbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public RecipeContext(DbContextOptions<RecipeContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // AppUser and Recipe: One-to-many relationship
            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.AppUser)
                .WithMany(u => u.Recipes)
                .HasForeignKey(r => r.AppUserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // Recipe and RecipeIngredient: Many-to-many relationship
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Ingredients)
                .WithMany(i => i.Recipes)
                .UsingEntity<RecipeIngredient>(
                    j => j.HasOne(ri => ri.Ingredient).WithMany().HasForeignKey(ri => ri.IngredientId),
                    j => j.HasOne(ri => ri.Recipe).WithMany().HasForeignKey(ri => ri.RecipeId));

            // Rating and Recipe: One-to-many relationship
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Recipe)
                .WithMany(rc => rc.Ratings)
                .HasForeignKey(r => r.RecipeId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // Rating and AppUser: One-to-many relationship
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.AppUser)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.AppUserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // Comment and Recipe: One-to-many relationship
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Recipe)
                .WithMany(r => r.Comments)
                .HasForeignKey(c => c.RecipeId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // Comment and AppUser: One-to-many relationship
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.AppUser)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AppUserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            base.OnModelCreating(modelBuilder);
        }
    }
}