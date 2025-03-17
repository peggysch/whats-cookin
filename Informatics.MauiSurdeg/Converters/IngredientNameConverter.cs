using System;
using System.Globalization;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Informatics.MauiSurdeg.Models;

namespace Informatics.MauiSurdeg.Converters
{
    public class IngredientNameConverter : IValueConverter
    {
        public ObservableCollection<Ingredient> AvailableIngredients { get; set; }

        public IngredientNameConverter(ObservableCollection<Ingredient> availableIngredients)
        {
            AvailableIngredients = availableIngredients;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int ingredientId)
            {
                var ingredient = AvailableIngredients.FirstOrDefault(i => i.IngredientID == ingredientId);
                return ingredient?.IngredientName ?? "Unknown Ingredient";
            }

            return "Unknown Ingredient";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}