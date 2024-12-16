using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SistemaEducativoWeb.Models; 

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Tutor>> GetTutoresFromApiAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://api.tuapi.com/tutores");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var tutores = JsonConvert.DeserializeObject<List<Tutor>>(json);

            return tutores ?? new List<Tutor>();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener tutores desde la API: " + ex.Message);
        }
    }
}
