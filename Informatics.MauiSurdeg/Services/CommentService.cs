using System;
using Informatics.MauiSurdeg.Contexts;
using Informatics.MauiSurdeg.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Informatics.MauiSurdeg.Interfaces;
using System.Threading.Tasks;


namespace Informatics.MauiSurdeg.Services
{
    public class CommentService : ICommentService
    {
        private readonly UniversityContext _context;

        public CommentService(UniversityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            return await _context.Comments
                .AsNoTracking()
                .Include(c => c.AppUser) // Include the user who posted the comment
                .Include(c => c.Recipe) // Include the recipe
                .ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _context.Comments
                .AsNoTracking()
                .Include(c => c.AppUser)
                .Include(c => c.Recipe)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetCommentsForRecipeAsync(int recipeId)
        {
            return await _context.Comments
                .AsNoTracking()
                .Where(c => c.RecipeId == recipeId)
                .Include(c => c.AppUser)
                .ToListAsync();
        }

        public async Task<Comment> SaveCommentAsync(Comment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));

            try
            {
                if (comment.Id > 0) // Update existing comment
                {
                    _context.Comments.Update(comment);
                }
                else // New comment
                {
                    await _context.Comments.AddAsync(comment);
                }

                await _context.SaveChangesAsync();
                return comment;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error saving comment.", ex);
            }
        }

        public async Task<bool> DeleteCommentByIdAsync(int id) //KOLLA
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return false;

            try
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error deleting comment.", ex);
            }
        }
    }
}

