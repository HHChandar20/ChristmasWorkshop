using System.ComponentModel.DataAnnotations;

namespace ChristmasWorkshop.DAL.Models;

public class Light
{
    [Key]
    public int Id { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Radius { get; set; }
    public string Color { get; set; }
    public string Effects { get; set; }
    public string Desc { get; set; }
    public string CT { get; set; }
}