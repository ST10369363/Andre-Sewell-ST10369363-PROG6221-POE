using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        class Recipe
        {
            public string Name { get; set; }
            public List<Ingredient> Ingredients { get; set; }
            public List<string> Steps { get; set; }

            public Recipe()
            {
                Ingredients = new List<Ingredient>();
                Steps = new List<string>();
            }

            public string DisplayRecipe()
            {
                string recipeDetails = $"Recipe: {Name}\n\nIngredients:\n";
                foreach (var ingredient in Ingredients)
                {
                    recipeDetails += $"{ingredient.Quantity} {ingredient.Unit} of {ingredient.Name} - {ingredient.Calories} calories, Food Group: {ingredient.FoodGroup}\n";
                }
                recipeDetails += "\nSteps:\n";
                for (int i = 0; i < Steps.Count; i++)
                {
                    recipeDetails += $"{i + 1}. {Steps[i]}\n";
                }
                return recipeDetails;
            }

            public int CalculateTotalCalories()
            {
                return Ingredients.Sum(ingredient => ingredient.Calories);
            }
        }

        class Ingredient
        {
            public string Name { get; set; }
            public double Quantity { get; set; }
            public string Unit { get; set; }
            public int Calories { get; set; }
            public string FoodGroup { get; set; }
        }

        static List<Recipe> recipes = new List<Recipe>();
        private Recipe currentRecipe;
        private List<TextBox> stepTextBoxes = new List<TextBox>();

        public MainWindow()
        {
            InitializeComponent();
            currentRecipe = new Recipe();
        }

        private void btnAddIngredient_Click(object sender, RoutedEventArgs e)
        {
            Ingredient ingredient = new Ingredient
            {
                Name = txtIngredientName.Text,
                Quantity = double.Parse(txtQuantity.Text),
                Unit = txtUnit.Text,
                Calories = int.Parse(txtCalories.Text),
                FoodGroup = txtFoodGroup.Text
            };

            currentRecipe.Ingredients.Add(ingredient);
            lstIngredients.Items.Add($"{ingredient.Quantity} {ingredient.Unit} of {ingredient.Name} - {ingredient.Calories} calories, Food Group: {ingredient.FoodGroup}");

            ClearIngredientFields();
        }

        private void btnAddRecipe_Click(object sender, RoutedEventArgs e)
        {
            currentRecipe.Name = txtRecipeName.Text;
            foreach (var textBox in stepTextBoxes)
            {
                currentRecipe.Steps.Add(textBox.Text);
            }

            recipes.Add(currentRecipe);
            SortAndDisplayRecipes();

            currentRecipe = new Recipe();
            ClearRecipeFields();
        }

        private void btnDisplayRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (lstRecipes.SelectedItem != null)
            {
                string selectedRecipeName = lstRecipes.SelectedItem.ToString();
                Recipe selectedRecipe = recipes.FirstOrDefault(r => r.Name.Equals(selectedRecipeName, StringComparison.OrdinalIgnoreCase));
                if (selectedRecipe != null)
                {
                    txtRecipeDetails.Text = selectedRecipe.DisplayRecipe();
                    int totalCalories = selectedRecipe.CalculateTotalCalories();
                    lblTotalCalories.Content = $"Total Calories: {totalCalories}";
                    lblWarning.Content = totalCalories > 300 ? "Warning: Total calories exceed 300!" : "";
                }
            }
        }

        private void btnGenerateSteps_Click(object sender, RoutedEventArgs e)
        {
            int numberOfSteps;
            if (int.TryParse(txtNumberOfSteps.Text, out numberOfSteps) && numberOfSteps > 0)
            {
                stepTextBoxes.Clear();
                panelSteps.Children.Clear();
                for (int i = 0; i < numberOfSteps; i++)
                {
                    TextBox stepTextBox = new TextBox
                    {
                        Margin = new Thickness(0, i * 25, 0, 0),
                        Width = 200,
                        Height = 22
                    };
                    stepTextBoxes.Add(stepTextBox);
                    panelSteps.Children.Add(stepTextBox);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number of steps.");
            }
        }

        private void SortAndDisplayRecipes()
        {
            lstRecipes.Items.Clear();
            foreach (var recipe in recipes.OrderBy(r => r.Name))
            {
                lstRecipes.Items.Add(recipe.Name);
            }
        }

        private void ClearIngredientFields()
        {
            txtIngredientName.Clear();
            txtQuantity.Clear();
            txtUnit.Clear();
            txtCalories.Clear();
            txtFoodGroup.Clear();
        }

        private void ClearRecipeFields()
        {
            txtRecipeName.Clear();
            txtNumberOfSteps.Clear();
            panelSteps.Children.Clear();
            lstIngredients.Items.Clear();
        }
    }
}
