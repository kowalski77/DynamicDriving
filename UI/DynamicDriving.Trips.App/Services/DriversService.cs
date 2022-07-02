using DynamicDriving.Models;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.Trips.App.Support;
using IdentityModel.Client;

namespace DynamicDriving.Trips.App.Services;

public class DriversService : IDriversService
{
    private readonly HttpClient httpClient;
    private readonly TokenManager tokenManager;

    public DriversService(HttpClient httpClient, TokenManager tokenManager)
    {
        this.httpClient = httpClient;
        this.tokenManager = tokenManager;
    }

    public async Task<IEnumerable<DriverSummary>> GetDriversSummaryAsync()
    {
        this.httpClient.SetBearerToken(await this.tokenManager.RetrieveAccessTokenAsync());

        var response = await this.httpClient.GetAsync("/api/v1/drivers");
        
        var drivers = await response.Content.ReadFromJsonAsync<SuccessEnvelope<DriversSummaryResponse>>();

        return drivers.Data.Drivers;
    }
}
