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

    //Register new user
    public async Task<AppUser> AddNewUserAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Username and password cannot be empty.");

        if (await UsernameExistsAsync(username))
            throw new InvalidOperationException("Username already exists.");

        // Generate new UserNo
        string newUserNo = await GenerateNextUserNoAsync();

        var hashedPassword = HashPassword(password); // Hash the password

        var newUser = new AppUser
        {
            UserNo = newUserNo,
            Username = username,
            PasswordHash = hashedPassword
        };

        return await SaveUserAsync(newUser); // Save user to database
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

    // ✅ Generate a new user number (U001, U002, ...)
    private async Task<string> GenerateNextUserNoAsync()
    {
        var latestUser = await _context.AppUsers
            .OrderByDescending(u => u.UserNo) // Senaste först
            .FirstOrDefaultAsync();

        if (latestUser == null || string.IsNullOrEmpty(latestUser.UserNo))
            return "U001"; // First user

        // Extract numeric part from "UXXX"
        string latestNumberStr = latestUser.UserNo.Substring(1); // Remove 'U'
        if (int.TryParse(latestNumberStr, out int latestNumber))
        {
            return $"U{(latestNumber + 1):D3}"; // Format as UXXX
        }

        throw new InvalidOperationException("Failed to generate UserNo.");
    }

    // ✅ Check if a username exists in the database
    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _context.AppUsers.AnyAsync(u => u.Username == username);
    }

    // ✅ Authenticate a user by verifying username & password
    public async Task<bool> AuthenticateUserAsync(string username, string password)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return false;

        return VerifyHashedPassword(user.PasswordHash, password);
    }

    // ✅ Hash password using PBKDF2 (you should call this when storing passwords)
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
    public async Task<bool> UpdateUsernameAsync(int userId, string newUsername)
    {
        if (string.IsNullOrWhiteSpace(newUsername))
            throw new ArgumentException("Username cannot be empty.");

        // Use the existing method to check if the username already exists
        bool exists = await UsernameExistsAsync(newUsername);
        if (exists)
            throw new InvalidOperationException("Username already exists.");

        var user = await _context.AppUsers.FindAsync(userId);
        if (user == null)
            return false;

        user.Username = newUsername;

        try
        {
            _context.AppUsers.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Error updating username.", ex);
        }
    }
}