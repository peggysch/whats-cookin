using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Models;
using Microsoft.Extensions.Configuration;

namespace Informatics.MauiSurdeg.Services
{
    public class RecipeSuggestionService : IRecipeSuggestionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IRecipeService _recipeService;

        // OpenAI Parameters
        private const string OpenAiUrl = "https://api.openai.com/v1/chat/completions";
        private const string ModelName = "gpt-4o-mini";
        private const int MaxTokens = 2000;
        private const double Temperature = 0.7;

        public RecipeSuggestionService(HttpClient httpClient, IConfiguration configuration, IRecipeService recipeService)
        {
            _httpClient = httpClient;
            _apiKey = configuration["AppSettings:OpenAIApiKey"];
            _recipeService = recipeService;
        }

        public async Task<List<GeneratedRecipe>> GenerateRecipeSuggestions(string type, int userId)
        {
            try
            {
                var prompt = await BuildPrompt(type, userId);
                if (string.IsNullOrEmpty(prompt))
                    return new List<GeneratedRecipe>();

                var requestData = new
                {
                    model = ModelName,
                    messages = new List<object>
            {
                new { role = "system", content = "You are a recipe assistant that generates structured JSON recipe suggestions." },
                new { role = "user", content = prompt }
            },
                    temperature = Temperature,
                    max_tokens = MaxTokens
                };

                var requestContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, OpenAiUrl)
                {
                    Content = requestContent
                };
                request.Headers.Add("Authorization", $"Bearer {_apiKey}");

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"⚠️ OpenAI API Error: {response.StatusCode} - {errorResponse}");
                    return new List<GeneratedRecipe>();
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                // 🔥 Log the full response before parsing
                Console.WriteLine($"📩 AI Response (raw): {responseContent}");

                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);

                // 🔥 Check if OpenAI returned an error response
                if (jsonResponse.TryGetProperty("error", out var errorProperty))
                {
                    Console.WriteLine($"❌ OpenAI Error: {errorProperty.GetProperty("message").GetString()}");
                    return new List<GeneratedRecipe>();
                }

                // 🔥 Check if 'choices' exist in the JSON response
                if (!jsonResponse.TryGetProperty("choices", out var choices) || choices.GetArrayLength() == 0)
                {
                    Console.WriteLine("⚠️ OpenAI did not return choices.");
                    return new List<GeneratedRecipe>();
                }

                var jsonMessage = choices[0].GetProperty("message").GetProperty("content").GetString();
                if (string.IsNullOrWhiteSpace(jsonMessage))
                {
                    Console.WriteLine("⚠️ OpenAI response content was empty.");
                    return new List<GeneratedRecipe>();
                }

                try
                {
                    var generatedRecipes = JsonSerializer.Deserialize<List<GeneratedRecipe>>(jsonMessage);
                    return generatedRecipes ?? new List<GeneratedRecipe>();
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"⚠️ JSON Parsing Error: {ex.Message}");
                    return new List<GeneratedRecipe>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Unexpected Error: {ex.Message}");
                return new List<GeneratedRecipe>();
            }
        }



        private async Task<string> BuildPrompt(string type, int userId)
        {
            string recipeData = "";

            if (type == "personalized")
            {
                // Fetch user's recipes for personalization
                var userRecipes = await _recipeService.GetRecipesByUserIdAsync(userId);
                if (!userRecipes.Any())
                    return "User has no known recipes.";

                recipeData = string.Join("\n", userRecipes.Select(recipe =>
                {
                    var ingredients = recipe.RecipeIngredients
                        .Select(ri => $"{ri.Ingredient.IngredientName} ({ri.Quantity})")
                        .ToList();

                    string ingredientList = ingredients.Any() ? string.Join(", ", ingredients) : "No ingredients listed";
                    return $"- {recipe.RecipeName} (Cooking Time: {recipe.CookingTime} min): Ingredients - {ingredientList}";
                }));
            }
            else if (type == "community")
            {
                // Fetch the 10 newest recipes from the community (feed)
                var communityRecipes = await _recipeService.GetLatestRecipesAsync(10);
                if (!communityRecipes.Any())
                    return "No recent community recipes available.";

                recipeData = string.Join("\n", communityRecipes.Select(recipe =>
                {
                    var ingredients = recipe.RecipeIngredients
                        .Select(ri => $"{ri.Ingredient.IngredientName} ({ri.Quantity})")
                        .ToList();

                    string ingredientList = ingredients.Any() ? string.Join(", ", ingredients) : "No ingredients listed";
                    return $"- {recipe.RecipeName} (Cooking Time: {recipe.CookingTime} min): Ingredients - {ingredientList}";
                }));
            }
            else if (type == "unique")
            {
                // Fetch the 10 newest recipes from the community (feed)
                var communityRecipes = await _recipeService.GetLatestRecipesAsync(10);
                if (!communityRecipes.Any())
                    return "No recent community recipes available.";

                recipeData = string.Join("\n", communityRecipes.Select(recipe =>
                {
                    var ingredients = recipe.RecipeIngredients
                        .Select(ri => $"{ri.Ingredient.IngredientName} ({ri.Quantity})")
                        .ToList();

                    string ingredientList = ingredients.Any() ? string.Join(", ", ingredients) : "No ingredients listed";
                    return $"- {recipe.RecipeName} (Cooking Time: {recipe.CookingTime} min): Ingredients - {ingredientList}";
                }));
                
            }

            return $@"
    You are an AI assistant that only generates JSON data.
    You NEVER include markdown, explanations, or any extra text in your responses.

    Generate a JSON array of **exactly 5 new recipes** in this format:
    ```json
    [
        {{
            ""Title"": ""Example Recipe"",
            ""CookingTime"": 30,
            ""Instructions"": ""Step 1: Do this. Step 2: Do that."",
            ""Ingredients"": [
                {{ ""Name"": ""Flour"", ""Quantity"": ""2 cups"" }},
                {{ ""Name"": ""Eggs"", ""Quantity"": ""3"" }}
            ]
        }},
        ...
    ]
    ```

    {(type == "personalized" ? $"- Make recipes similar to these:\n{recipeData}" : "")}
    {(type == "community" ? $"- Base the new recipes on these recent community recipes:\n{recipeData}" : "")}
    {(type == "unique" ? $"- Make recipes that are **entirely different** from these:\n{recipeData}" : "")}

    - **Ensure every response is valid JSON, without markdown or explanations.**
    - **Do NOT use Markdown or extra text. Just return the JSON array.**
    ";
        }

    }
}
