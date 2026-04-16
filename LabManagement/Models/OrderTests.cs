using System.Text.Json.Serialization;

public class OrderTests
{
    [JsonPropertyName("order_test_id")]
    public int OrderTestId { get; set; }

    [JsonPropertyName("order_id")]
    public int OrderId { get; set; }

    [JsonPropertyName("test_id")]
    public int TestId { get; set; }

    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = "Pending";
}