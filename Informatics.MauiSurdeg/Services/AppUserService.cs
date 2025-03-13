using System;
using Informatics.MauiSurdeg.Contexts;
using Informatics.MauiSurdeg.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Informatics.MauiSurdeg.Interfaces;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Collections.ObjectModel;

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

    public async Task<AppUser?> GetUserByNoAsync(string userNo)
    {
        return await _context.AppUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserNo == userNo);
    }

    public async Task<AppUser> SaveUserAsync(AppUser user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        try
        {
            if (user.AppUserID > 0) // Update existing user
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
        var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.UserNo == userNo);
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

    // Check if a username exists in the database
    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _context.AppUsers.AnyAsync(u => u.Username == username);
    }

    // Authenticate a user by verifying username & password
    public async Task<bool> AuthenticateUserAsync(string username, string password)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return false;

        return VerifyHashedPassword(user.PasswordHash, password);
    }

    // Hash password using PBKDF2 (you should call this when storing passwords)
    private byte[] HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    // Verify if the hashed password matches the stored hash
    private bool VerifyHashedPassword(byte[] storedHash, string password)
    {
        byte[] inputHash = HashPassword(password);
        return storedHash.SequenceEqual(inputHash);
    }

    //Se till så att Profile är kopplad till personen som loggar in 
    public async Task<int?> GetAuthenticatedUserIdAsync(string username)
    {
        return await _context.AppUsers
            .Where(u => u.Username == username)
            .Select(u => u.AppUserID)
            .FirstOrDefaultAsync();
    }



    //Profile 
    public async Task<IEnumerable<Recipe>> GetUserRecipesAsync(int userId)
    {
        return await _context.Recipes
            .AsNoTracking()
            .Where(r => r.AppUserID == userId)
            .ToListAsync();
    }
    /*public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await _context.AppUsers
            .AsNoTracking()
            .Include(u => u.Recipes)
            .FirstOrDefaultAsync(u => u.Username == username);
    }*/

}