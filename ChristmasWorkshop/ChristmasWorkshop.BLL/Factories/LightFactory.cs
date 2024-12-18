using ChristmasWorkshop.DAL.Models;

namespace ChristmasWorkshop.BLL.Factories;

public class LightFactory
{
    public static readonly Random Random = new Random();
    private static readonly string[] Colors = { "blue-lt", "blue-dk", "red", "gold-lt", "gold-dk" };
    private static readonly string[] Effects = { "g1", "g2", "g3" };

    public static Light CreateLight(double x, double y, string description, string color, string effects, double radius, string ct)
    {
        return new Light
        {
            X = x,
            Y = y,
            Radius = radius,
            Color = color,
            Effects = effects,
            Desc = description,
            CT = ct,
        };
    }

    public static string GetRandomColor(string lastColor)
    {
        string color;
        do
        {
            color = Colors[Random.Next(Colors.Length)];
        }
        while (color == lastColor);

        return color;
    }

    public static string GetRandomEffect() => Effects[Random.Next(Effects.Length)];

    public static double GetRandomRadius() => Random.Next(3, 7);
}