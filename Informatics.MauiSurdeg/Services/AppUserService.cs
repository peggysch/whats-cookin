using System;
using Informatics.MauiSurdeg.Contexts;
using Informatics.MauiSurdeg.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Informatics.MauiSurdeg.Interfaces;
using System.Threading.Tasks;

namespace Informatics.MauiSurdeg.Services;

public class AppUserService : IAppUserService
    {
        private readonly UniversityContext _context;

        public AppUserService(UniversityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.AppUsers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AppUser?> GetUserByIdAsync(int id)
        {
            return await _context.AppUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<AppUser?> GetUserByNoAsync(string userNo)
        {
            return await _context.AppUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Number == userNo);
        }

        public async Task<AppUser> SaveUserAsync(AppUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            try
            {
                if (user.Id > 0) // Update existing user
                {
                    _context.AppUsers.Update(user);
                }
                else // New user
                {
                    await _context.AppUsers.AddAsync(user);
                }

                await _context.SaveChangesAsync();
                return user;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error saving user.", ex);
            }
        }

        public async Task<bool> DeleteUserByIdAsync(int id)
        {
            var user = await _context.AppUsers.FindAsync(id);
            if (user == null) return false;

            try
            {
                _context.AppUsers.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error deleting user.", ex);
            }
        }

        public async Task<bool> DeleteUserByNoAsync(string userNo) //KOLLA
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Number == userNo);
            if (user == null) return false;

            try
            {
                _context.AppUsers.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error deleting user.", ex);
            }
        }
    }