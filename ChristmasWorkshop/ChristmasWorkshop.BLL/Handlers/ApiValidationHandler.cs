using RestSharp;
using System;
using System.Threading.Tasks;

namespace ChristmasWorkshop.BLL.Handlers;

public class ApiValidationHandler : ValidationHandler
{
    public override async Task<bool> ValidateAsync(double x, double y)
    {
        string url = $"https://polygon.gsk567.com/?x={x}&y={y}";
        var client = new RestClient(url);
        var request = new RestRequest()
            .AddQueryParameter("x", x.ToString())
            .AddQueryParameter("y", y.ToString());
        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            return response?.Content?.Contains("true") ?? false;
        }
        else
        {
            throw new Exception($"Error calling polygon API: {response.StatusCode}");
        }
    }
}