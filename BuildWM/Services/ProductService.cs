using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class ProductService
{
    private readonly HttpClient _http;

    public ProductService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ProductResponse?> GetProductsAsync(int limit = 12, int skip = 0)
    {
        var url = $"https://dummyjson.com/products?limit={limit}&skip={skip}";
        return await _http.GetFromJsonAsync<ProductResponse>(url);
    }

    public async Task<ProductResponse?> SearchProductsAsync(string query, int limit = 12, int skip = 0)
    {
        var url = $"https://dummyjson.com/products/search?q={Uri.EscapeDataString(query)}&limit={limit}&skip={skip}";
        return await _http.GetFromJsonAsync<ProductResponse>(url);
    }
}
