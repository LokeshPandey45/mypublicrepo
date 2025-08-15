using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ProxyController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public ProxyController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    [Route("{*url}")]
    public async Task<IActionResult> Get(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return BadRequest("URL is required.");
        }

        // Decode the URL
        var targetUrl = System.Net.WebUtility.UrlDecode(url);

        try
        {
            // Forward the request to the target URL
            var response = await _httpClient.GetAsync(targetUrl);

            // Copy the response content and headers
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, response.Content.Headers.ContentType?.ToString());
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}
