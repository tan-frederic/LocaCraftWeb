using LocaCraftAPI.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Xml.Linq;

namespace LocaCraftAPI.Services
{
    public class InseeService : IInseeService
    {
        #region Attributes
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        private const string BaseUrl = "https://www.bdm.insee.fr/series/sdmx/data/SERIES_BDM";
        private static readonly Dictionary<string, string> IdBanks = new()
        {
            { "IRL",  "001762471" },
            { "ILC",  "001762462" },
            { "ICC",  "001762463" },
            { "ILAT", "001762464" }
        };

        #endregion

        #region Constructor
        public InseeService(HttpClient httpClient, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _cache = memoryCache;
        }
        #endregion

        public async Task<List<InseeIndexModel>> GetIndexAsync(int lastNIndex = 20)
        {
            const string cacheKey = "index_insee";

            if (!_cache.TryGetValue(cacheKey, out List<InseeIndexModel>? indexes))
            {
                indexes = await FetchAndMergeAsync(lastNIndex);
                _cache.Set(cacheKey, indexes, TimeSpan.FromHours(24));
            }
            if (indexes == null)
                indexes = new();
            return indexes;
        }

        private async Task<List<InseeIndexModel>> FetchAndMergeAsync(int lastN)
        {
            var tasks = IdBanks.ToDictionary(
                kvp => kvp.Key,
                kvp => FetchSerieAsync(kvp.Value, lastN)
            );

            await Task.WhenAll(tasks.Values);

            var results = tasks.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Result
            );

            var periodes = results.Values
                .SelectMany(d => d.Keys)
                .Distinct()
                .OrderByDescending(p => p);

            return periodes.Select(p => new InseeIndexModel
            {
                Period = p,
                IRL = results["IRL"].GetValueOrDefault(p),
                ILC = results["ILC"].GetValueOrDefault(p),
                ICC = results["ICC"].GetValueOrDefault(p),
                ILAT = results["ILAT"].GetValueOrDefault(p)
            }).ToList();
        }

        private async Task<Dictionary<string, double>> FetchSerieAsync(string idbank, int lastN)
        {
            var url = $"{BaseUrl}/{idbank}?lastNObservations={lastN}";
            var response = await _httpClient.GetStringAsync(url);
            var xml = XDocument.Parse(response);

            return xml.Descendants()
                   .Where(e => e.Name.LocalName == "Obs")
                   .ToDictionary(
                       obs => obs.Attribute("TIME_PERIOD")?.Value ?? "",
                       obs => double.TryParse(
                           obs.Attribute("OBS_VALUE")?.Value,
                           System.Globalization.NumberStyles.Any,
                           System.Globalization.CultureInfo.InvariantCulture,
                           out var val) ? val : 0.0
                   );
        }
    }
}
