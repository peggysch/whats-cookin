using System.Collections.Generic;

namespace Informatics.MauiSurdeg.Models
{
    public class GeneratedRecipe
    {
        public string Title { get; set; }
        public int CookingTime { get; set; }
        public string Instructions { get; set; }
        public List<GeneratedIngredient> Ingredients { get; set; }
    }

    public class GeneratedIngredient
    {
        public string Name { get; set; }
        public string Quantity { get; set; }
    }
}

