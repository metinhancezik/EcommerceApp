using System.Text.Json.Serialization;

namespace ECommerceView.Models;

public class PaymentResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }

    [JsonPropertyName("htmlContent")]
    public string? HtmlContent { get; set; }
}