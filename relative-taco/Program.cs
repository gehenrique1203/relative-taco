using System;
using System.Collections.Generic;
using System.Text;

class Program
{
    static void Main()
    {
        // feeding items to test
        /*
        List<Food> listaItens = new List<Food>()
        {
            new Food(){
                Name = "food1", Calories = 300, Protein = 31.67, Carbohydrate = 25.15, Fat = 10
            },
            new Food(){
                Name = "food2", Calories = 400, Protein = 10, Carbohydrate = 20, Fat = 30
            },
            new Food(){
                Name = "food3", Calories = 500, Protein = 25, Carbohydrate = 50, Fat = 75
            },
            new Food(){
                Name = "food4", Calories = 600, Protein = 3, Carbohydrate = 6, Fat = 9
            },
            new Food(){
                Name = "food5", Calories = 700, Protein = 5, Carbohydrate = 10, Fat = 15
            },
            new Food(){
                Name = "food6", Calories = 700, Protein = 30, Carbohydrate = 20, Fat = 10
            }
        };
        */

        List<Food> listaItens = getTacoTableCSV("Assets/taco.csv");

        // Values to compare
        double Protein = 130;
        double Carbohydrate = 126;
        double Fat = 42;

        // Encontrar itens com mesma proporção
        var found = FindSimilarProportion(listaItens, Protein, Carbohydrate, Fat);

        if (found.Count > 0)
        {
            Console.WriteLine("Items with the same proportion:");
            foreach (var item in found)
                Console.WriteLine("Food selected: " + item.Name + " - Calories: " + item.Calories + 
                " (Protein: " + item.Protein + ", Carbohydrate: " + item.Carbohydrate + ", Fat: " + item.Fat + ")" + (item.Protein > item.Carbohydrate ? "OKKKKKKKK" : "NOOOOOOOO"));
        }
        else
            Console.WriteLine("No items found!");
    }

    private static List<Food> getTacoTableCSV(string filePath)
    {
        List<Food> foods = new List<Food>();
        
        using (var reader = new StreamReader(filePath, System.Text.Encoding.UTF8))
        {
            bool header = true;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                
                // Ignoring empty lines
                if (string.IsNullOrEmpty(line)) continue;
                // Ignoring header
                else if (header) { header = false; continue; }

                var values = line.Split(';');

                // Calories treatment
                int roundedNumber = 0;
                if (double.TryParse(values[1].ToString().Replace(",", "."), out double number))
                    roundedNumber = (int)Math.Ceiling(number);
                
                var food = new Food
                {
                    Name = string.IsNullOrEmpty(values[0]) ? "" : values[0],
                    Calories = Convert.ToInt32(roundedNumber),
                    Protein = double.TryParse(values[2].ToString().Replace(",", "."), out double resultProtein) ? resultProtein : 0,
                    Carbohydrate = double.TryParse(values[3].ToString().Replace(",", "."), out double resultCarb) ? resultCarb : 0,
                    Fat = double.TryParse(values[4].ToString().Replace(",", "."), out double resultFat) ? resultFat : 0
                };

                foods.Add(food);
            }
        }

        return foods;
    }

    static (double, double) Proportion(double value1, double value2, double value3)
    {
        // Do the math to get proportion between values
        double proportion1 = value1 / value2;
        double proportion2 = value2 / value3;
        return (proportion1, proportion2);
    }

    static List<Food> FindSimilarProportion(List<Food> foodItems, double desiredValue1, double desiredValue2, double desiredValue3)
    {
        var proportionRef = Proportion(desiredValue1, desiredValue2, desiredValue3);
        List<Food> matchItems = new List<Food>();
        
        foreach (var food in foodItems)
        {
            var itemProporcao = Proportion(food.Protein, food.Carbohydrate, food.Fat);
            
            double tolerance = 0.05; // 0.05 = 5%

            if (itemProporcao.Item1 >= proportionRef.Item1 * (1 - tolerance)/* &&
                itemProporcao.Item1 <= proportionRef.Item1 * (1 + tolerance) &&
                itemProporcao.Item2 >= proportionRef.Item2 * (1 - tolerance) &&
                itemProporcao.Item2 <= proportionRef.Item2 * (1 + tolerance)*/)
            {
                matchItems.Add(food);
            }
        }

        return matchItems;
    }
}
