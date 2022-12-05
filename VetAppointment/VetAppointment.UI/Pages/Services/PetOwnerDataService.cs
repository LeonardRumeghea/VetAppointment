namespace VetAppointment.UI.Pages.Services
{
    public class PetOwnerDataService : IPetOwnerDataService
    {
        private const string ApiURL = "https://localhost:7112/v1/api/petOwners";
        private readonly HttpClient httpClient;

        public PetOwnerDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<PetOwner>> GetAllPetOwners()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<PetOwner>>(await httpClient.GetStreamAsync(ApiURL),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<PetOwner> AddPetOwner(PetOwner petOwner)
        {
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var json = JsonSerializer.Serialize(petOwner);
            var response = await httpClient.PostAsync
                (ApiURL, new StringContent(json, UnicodeEncoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            return await JsonSerializer.DeserializeAsync<PetOwner>(response.Content.ReadAsStream(), options);
        }

        public async Task<PetOwner> AddPetsToPetOwner(Guid petOwnerId, List<Pet> pets)
        {
            var ApiURLPetOwner = $"{ApiURL}/{petOwnerId}/Pets";
            var response = await httpClient.PostAsJsonAsync(ApiURLPetOwner, pets);
            return await response.Content.ReadFromJsonAsync<PetOwner>();
        }
    }
}
