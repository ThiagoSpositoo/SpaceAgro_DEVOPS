using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpaceAgro.DotNetApi.Services
{
    public class NasaSpaceService
    {
        private readonly HttpClient _httpClient;

        public NasaSpaceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<object> BuscarPrevisaoAgroAsync(double latitude, double longitude)
        {
            // Força o formato com ponto (.) para as coordenadas decimais
            string latStr = latitude.ToString("F4", CultureInfo.InvariantCulture);
            string lonStr = longitude.ToString("F4", CultureInfo.InvariantCulture);

            // ROTA OFICIAL DA NASA (Climatologia de ponto para Agro)
            string url = $"https://power.larc.nasa.gov/api/temporal/climatology/point?parameters=T2M,RH2M&community=AG&longitude={lonStr}&latitude={latStr}&format=JSON";

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "SpaceAgroApi/1.0");

                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(jsonString);
                    return doc.RootElement.Clone();
                }
                
                string erroCorpo = await response.Content.ReadAsStringAsync();
                return new { erro = $"NASA retornou status {response.StatusCode}. Detalhe: {erroCorpo}" };
            }
            catch (Exception ex)
            {
                return new { erro = $"Falha na requisição: {ex.Message}" };
            }
        }
    }
}