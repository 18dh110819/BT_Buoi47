public class ProductStateService
{
    private readonly HttpClient _httpClient;
    public List<ProductDto> Products { get; set; } = new List<ProductDto>();

    public ProductStateService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("CybersoftShopee");
    }

    public async Task LoadProductsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ResponseType<List<ProductDto>>>("api/product/all") ?? new ResponseType<List<ProductDto>>();
        Products = response.DataResponse ?? new List<ProductDto>();
    }

    public Action? OnChange { get; set; }
    public void StateHasChanged() => OnChange?.Invoke();

}

