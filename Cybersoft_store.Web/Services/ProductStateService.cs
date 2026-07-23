public class ProductStateService
{
    private readonly HttpClient _httpClient;
    public List<ProductDto> Products { get; set; } = [];

    public ProductStateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task LoadProductsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ResponseType<List<ProductDto>>>("http://localhost:5098/api/product/all") ?? new ResponseType<List<ProductDto>>();
        Products = response.DataResponse ?? [];
        StateHasChanged();
    }

    public Action? OnChange { get; set; }
    public void StateHasChanged() => OnChange?.Invoke();

}

