using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using VetAppointment.Shared.Domain;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VetAppointment.UI.Pages.Services
{
    public class PetDataService : IPetDataService
    {
        private const string ApiURL = "https://localhost:7112/v1/api/pets";
        private readonly HttpClient httpClient;

        public PetDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<Pet>> GetAllPets()
        {
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return await JsonSerializer.DeserializeAsync<IEnumerable<Pet>>
                (await httpClient.GetStreamAsync(ApiURL), options);
        }

        public async Task<Pet> AddPet(Pet pet)
        {
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var json = JsonSerializer.Serialize(pet);
            var response = await httpClient.PostAsync
                (ApiURL, new StringContent(json, UnicodeEncoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            return await JsonSerializer.DeserializeAsync<Pet>(response.Content.ReadAsStream(), options);
        }
    }
}
