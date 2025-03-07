using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Informatics.MauiSurdeg.Models;

namespace Informatics.MauiSurdeg.Interfaces
{

   public interface IAppUserService
    {
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser?> GetUserByIdAsync(int id);
        Task<AppUser?> GetUserByNoAsync(string userNo);
        Task<AppUser> SaveUserAsync(AppUser user);
        Task<bool> DeleteUserByIdAsync(int id);
        Task<bool> DeleteUserByNoAsync(string userNo);
    }
}

