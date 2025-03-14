using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informatics.MauiSurdeg.Models;

namespace Informatics.MauiSurdeg.Interfaces
{

   public interface IAppUserService
    {
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser?> GetUserByNoAsync(string userNo);
        Task<AppUser> SaveUserAsync(AppUser user);
        Task<bool> DeleteUserByIdAsync(int id);
        Task<bool> DeleteUserByNoAsync(string userNo);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> AuthenticateUserAsync(string username, string password);

        //Profile 
        Task<int?> GetAuthenticatedUserIdAsync(string username);
        Task<IEnumerable<Recipe>> GetUserRecipesAsync(int userId);
        Task<AppUser> AddNewUserAsync(string username, string password);
    }
}