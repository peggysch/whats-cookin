using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informatics.MauiSurdeg.Models;
namespace Informatics.MauiSurdeg.Interfaces

{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetCommentsAsync();
        Task<Comment?> GetCommentByIdAsync(int id);
        // Task<Comment?> GetCommentByNoAsync(string commentNo);
        Task<IEnumerable<Comment>> GetCommentsForRecipeAsync(int recipeId);
        Task<Comment> SaveCommentAsync(Comment comment);
        Task<bool> DeleteCommentByIdAsync(int id);
        // Task<bool> DeleteCommentByNoAsync(string commentNo);
    }
}