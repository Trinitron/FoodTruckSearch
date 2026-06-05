using CsvHelper;
using CsvHelper.Configuration;
using FoodTruckSearch.Shared.Models;
using System.Globalization;

namespace FoodTruckSearch.Services;

public class TruckService
{
    private readonly HttpClient _http;
    private Task? _loadTask;
    private List<Truck> _allTrucks = new();

    public TruckService(HttpClient http) => _http = http;

    public async Task<List<TruckSearchResult>> SearchAsync(double lat, double lng, int count, string keyword)
    {
        await EnsureLoadedAsync();
        return _allTrucks
            .Where(t => string.IsNullOrEmpty(keyword) ||
                        t.FoodItems.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .Select(t => new TruckSearchResult(t, Haversine(lat, lng, t.Latitude!.Value, t.Longitude!.Value)))
            .OrderBy(r => r.DistanceKm)
            .Take(count)
            .ToList();
    }

    private Task EnsureLoadedAsync() => _loadTask ??= LoadAsync();

    private async Task LoadAsync()
    {
        var csv = await _http.GetStringAsync("sample-data/Mobile_Food_Facility_Permit.csv");
        using var reader = new StringReader(csv);
        using var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        });
        _allTrucks = csvReader.GetRecords<Truck>()
            .Where(t => t.Status == "APPROVED" && (t.Latitude ?? 0) != 0 && (t.Longitude ?? 0) != 0)
            .ToList();
    }

    private static double Haversine(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371;
        var dLat = (lat2 - lat1) * Math.PI / 180;
        var dLon = (lon2 - lon1) * Math.PI / 180;
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
              + Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180)
              * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    }
}
