using System.Net;

namespace WebApplication4.Services;

public record GeoInfo(string? Country, string? City);

public interface IGeoLocationsService
{
    Task<GeoInfo> LookupAsync(IPAddress ip, CancellationToken ct = default);
}
