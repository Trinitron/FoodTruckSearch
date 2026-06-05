namespace FoodTruckSearch.Shared.Models;

public class Truck
{
    public string Applicant { get; set; } = "";
    public string Address { get; set; } = "";
    public string Status { get; set; } = "";
    public string FoodItems { get; set; } = "";
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
