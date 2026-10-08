using System.Security.Cryptography;
using System.Text;
using Informatics.MauiSurdeg.Contexts;
using Informatics.MauiSurdeg.Models;

namespace Informatics.MauiSurdeg.DataBase;

/// <summary>
/// Creates the local SQLite database from the EF Core model and fills it with demo data
/// (based on WhatsCookin_dll.sql and the Figma prototypes) the first time the app starts.
/// </summary>
public static class DatabaseSeeder
{
    public static void EnsureCreatedAndSeeded(WhatsCookinContext db)
    {
        db.Database.EnsureCreated();

        if (db.AppUsers.Any())
            return; // Already seeded

        // ---------- Users (same hashing as AppUserService: SHA-256) ----------
        var users = new[]
        {
            NewUser("U001", "Pasta_papi", "password123"),
            NewUser("U002", "Mumsmamman", "mySecureP@ssw0rd"),
            NewUser("U003", "test123", "test123"),
            NewUser("U004", "Bobbe bollo", "bobthebuilder2025"),
        };
        db.AppUsers.AddRange(users);
        db.SaveChanges();

        // ---------- Basic ingredients ----------
        string[] ingredientNames =
        {
            "Bacon", "Basil", "Bell Pepper", "Black Pepper", "Bread", "Broccoli", "Butter", "Carrots",
            "Cheddar", "Chicken Breast", "Chili", "Coconut Milk", "Cream", "Crushed Tomatoes",
            "Cucumber", "Curry Powder", "Eggs", "Flour", "Garlic", "Ginger", "Ground Beef",
            "Kidney Beans", "Lemon", "Lettuce", "Milk", "Mushrooms", "Olive Oil", "Onion",
            "Parmesan", "Pasta", "Potatoes", "Rice", "Salmon", "Salt", "Soy Sauce", "Spaghetti",
            "Spinach", "Stock", "Sugar", "Tomatoes", "Tortillas", "Yellow Onion", "Zucchini"
        };
        var ingredients = ingredientNames
            .Select((name, i) => new Ingredient { IngredientNo = $"I{i + 1:D3}", IngredientName = name })
            .ToList();
        db.Ingredients.AddRange(ingredients);
        db.SaveChanges();
        Ingredient Ing(string name) => ingredients.First(i => i.IngredientName == name);

        // ---------- Recipes ----------
        var now = DateTime.UtcNow;
        var recipes = new List<Recipe>
        {
            NewRecipe("R001", "Carbonara", "Made a really nice Carbonara today for lunch! Both quick and easy!",
                "Boil the pasta. Fry the bacon until crispy. Whisk eggs with grated parmesan and black pepper. " +
                "Mix the hot pasta with the bacon, take off the heat and stir in the egg mixture.",
                30, 45, users[0], now.AddDays(-1)),
            NewRecipe("R002", "Spaghetti Bolognese", "A classic Italian dish with ground beef, tomatoes, and spices.",
                "Fry onion and garlic, brown the ground beef, add crushed tomatoes and simmer for 20 minutes. Serve with spaghetti.",
                40, 35, users[0], now.AddDays(-6)),
            NewRecipe("R003", "Chicken Curry", "A spicy chicken curry with coconut milk and curry powder.",
                "Fry onion, garlic and ginger. Add chicken and curry powder, pour in coconut milk and simmer for 25 minutes. Serve with rice.",
                45, 40, users[1], now.AddDays(-5)),
            NewRecipe("R004", "Tomato Soup", "Today I made a yummy tomato soup, really recommend! Super easy to make!",
                "Fry onion and garlic in olive oil, add tomatoes and stock, simmer for 20 minutes and blend. Top with cream and basil.",
                40, 30, users[1], now.AddDays(-2)),
            NewRecipe("R005", "Vegetable Stir Fry", "A quick stir fry with seasonal vegetables and soy sauce.",
                "Slice the vegetables, stir fry on high heat for 5 minutes and finish with soy sauce. Serve with rice.",
                20, 25, users[2], now.AddDays(-4)),
            NewRecipe("R006", "Beef Stew", "Tender beef chunks slow-cooked with carrots and potatoes in a savory broth.",
                "Brown the beef, add onion, carrots, potatoes and stock. Simmer on low heat for two hours.",
                120, 55, users[3], now.AddDays(-3)),
        };
        db.Recipes.AddRange(recipes);
        db.SaveChanges();

        // ---------- Recipe ingredients ----------
        void AddIngredients(Recipe recipe, params (string name, string qty)[] items)
        {
            foreach (var (name, qty) in items)
                db.RecipeIngredients.Add(new RecipeIngredient
                {
                    RecipeID = recipe.RecipeID,
                    IngredientID = Ing(name).IngredientID,
                    Quantity = qty
                });
        }

        AddIngredients(recipes[0], ("Spaghetti", "400 g"), ("Bacon", "150 g"), ("Eggs", "3 pcs"), ("Parmesan", "1 dl"), ("Black Pepper", "1 tsp"));
        AddIngredients(recipes[1], ("Spaghetti", "400 g"), ("Ground Beef", "500 g"), ("Crushed Tomatoes", "400 g"), ("Yellow Onion", "1 pcs"), ("Garlic", "2 cloves"));
        AddIngredients(recipes[2], ("Chicken Breast", "500 g"), ("Coconut Milk", "400 ml"), ("Curry Powder", "2 tbsp"), ("Ginger", "1 tbsp"), ("Rice", "3 dl"));
        AddIngredients(recipes[3], ("Tomatoes", "800 g"), ("Onion", "1 pcs"), ("Garlic", "2 cloves"), ("Stock", "5 dl"), ("Cream", "1 dl"), ("Basil", "1 bunch"));
        AddIngredients(recipes[4], ("Broccoli", "1 pcs"), ("Carrots", "2 pcs"), ("Bell Pepper", "1 pcs"), ("Soy Sauce", "3 tbsp"), ("Rice", "3 dl"));
        AddIngredients(recipes[5], ("Ground Beef", "600 g"), ("Carrots", "3 pcs"), ("Potatoes", "4 pcs"), ("Onion", "1 pcs"), ("Stock", "1 l"));
        db.SaveChanges();

        // ---------- Ratings (one per user and recipe) ----------
        var ratings = new (int user, int recipe, int stars)[]
        {
            (1, 0, 4), (2, 0, 5), (3, 0, 4),
            (0, 2, 5), (3, 2, 4),
            (0, 3, 4), (2, 3, 5),
            (0, 4, 3), (1, 4, 4),
            (0, 5, 5), (1, 5, 5),
            (2, 1, 4), (3, 1, 3),
        };
        int ratingNo = 1;
        foreach (var (user, recipe, stars) in ratings)
            db.Ratings.Add(new Rating
            {
                RatingNo = $"RA{ratingNo++:D3}",
                AppUserID = users[user].AppUserID,
                RecipeID = recipes[recipe].RecipeID,
                Stars = stars,
                CreatedAt = now
            });

        // ---------- Comments ----------
        var comments = new (int user, int recipe, string text)[]
        {
            (1, 0, "This recipe is fantastic! A family favorite!"),
            (0, 2, "I love how spicy this is, perfect for a chilly day."),
            (2, 3, "Quick and easy! My go-to recipe for busy days."),
            (1, 5, "The beef stew was amazing, so tender and flavorful!"),
        };
        int commentNo = 1;
        foreach (var (user, recipe, text) in comments)
            db.Comments.Add(new Comment
            {
                CommentNo = $"C{commentNo++:D3}",
                AppUserID = users[user].AppUserID,
                RecipeID = recipes[recipe].RecipeID,
                CommentText = text,
                CreatedAt = now
            });

        db.SaveChanges();
    }

    private static AppUser NewUser(string userNo, string username, string password) => new()
    {
        UserNo = userNo,
        Username = username,
        PasswordHash = SHA256.HashData(Encoding.UTF8.GetBytes(password))
    };

    private static Recipe NewRecipe(string no, string name, string description, string instructions,
        int minutes, int cost, AppUser user, DateTime created) => new()
    {
        RecipeNo = no,
        RecipeName = name,
        RecipeDescription = description,
        RecipeInstructions = instructions,
        CookingTime = minutes,
        RecipeCost = cost,
        AppUserID = user.AppUserID,
        CreatedAt = created
    };
}
