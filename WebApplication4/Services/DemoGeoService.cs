using System.Net;

namespace WebApplication4.Services;

public class DemoGeoService : IGeoLocationsService
{
    public Task<GeoInfo> LookupAsync(IPAddress ip, CancellationToken ct = default)
    {
        if (IPAddress.IsLoopback(ip))
        {
            return Task.FromResult<GeoInfo?>(new GeoInfo("Local","Local"));
        }

        return Task.FromResult<GeoInfo?>(new GeoInfo("NoContent", "NoContent"));
    }
}
