using Cybersoft_store.Web.Pages;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

public class UserStateService
{
    private readonly ILocalStorageService _localStorageService;
    public string accessToken;
    public ProfileDto CurrentUser { get; private set; } = new ProfileDto();
    private readonly HttpClient _httpClient;
    public Action OnChange;

    public void StateHasChanged() => OnChange?.Invoke();

    public UserStateService(ILocalStorageService localStorageService, IHttpClientFactory httpClientFactory)
    {
        _localStorageService = localStorageService;
        _httpClient = httpClientFactory.CreateClient("CybersoftShopee");
    }

    public async Task Login(LoginDto model)
    {
        var res = await _httpClient.PostAsJsonAsync("api/User/login", model);
        if (res.IsSuccessStatusCode)
        {
            var resData = await res.Content.ReadFromJsonAsync<ResponseType<string>>();
            if (resData != null && resData.StatusCode == 202)
            {
                accessToken = resData.DataResponse;
                await _localStorageService.SetItemAsync("accessToken", accessToken);
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
    }

    public async Task GetProfile()
    {
        var token = await _localStorageService.GetItemAsync<string>("accessToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            try
            {
                var resMsg = await _httpClient.GetAsync("api/Profile");
                if (resMsg.IsSuccessStatusCode)
                {
                    var res = await resMsg.Content.ReadFromJsonAsync<ResponseType<ProfileDto>>();
                    if (res?.DataResponse != null)
                    {
                        Console.WriteLine($@"{JsonSerializer.Serialize(res.DataResponse)}");
                        CurrentUser = res.DataResponse;
                        accessToken = token;
                    }
                }
                else if (resMsg.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await _localStorageService.RemoveItemAsync("accessToken");
                    accessToken = string.Empty;
                    CurrentUser = new ProfileDto();
                }
                // ignore other non-success statuses (404, etc.) to avoid throwing
            }
            catch (HttpRequestException)
            {
                // network or other error - clear user state
                accessToken = string.Empty;
                CurrentUser = new ProfileDto();
            }
        }
    }
}