//using VetAppointment.API.Dtos;

namespace VetAppointment.UI.Pages.Services
{
    public class VetClinicDataService : IVetClinicDataService
    {
        private const string ApiURL = "https://localhost:7112/v1/api/VetClinics";
        private readonly HttpClient httpClient;

        public VetClinicDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<VetClinic>> GetAllClinics()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<VetClinic>>(
                await httpClient.GetStreamAsync(ApiURL),
                new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true });
        }

        public async Task<VetClinic> AddClinic(VetClinic clinic)
        {
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var json = JsonSerializer.Serialize(clinic);
            var response = await httpClient.PostAsync
                    (ApiURL, new StringContent(json, UnicodeEncoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            return await JsonSerializer.DeserializeAsync<VetClinic>(response.Content.ReadAsStream(), options);

            //var response = await httpClient.PostAsJsonAsync(ApiURL, clinic);
            //return await response.Content.ReadFromJsonAsync<VetClinic>();
        }

        //public async Task<Pet> AddPetsToClinic(Guid clinicId, List<Pet> pets)
        //{
        //    var ApiURLClinic = $"{ApiURL}/{{{clinicId}}}/Pets";
        //    var response = await httpClient.PostAsJsonAsync(ApiURLClinic, pets);
        //    return await response.Content.ReadFromJsonAsync<Pet>();
        //}

        public async Task<Vet> AddVetToClinic(Guid clinicId, Vet vet)
        {
            var ApiURLClinic = $"{ApiURL}/{clinicId}/vet";
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var result = new VetAppointment.API.Dtos.VetDto(vet.Name, vet.Surname, vet.Birthdate, vet.Gender, vet.Email,
                vet.Phone, vet.Specialisation);
            var json = JsonSerializer.Serialize(result);
            var response = await httpClient.PostAsync
                    (ApiURLClinic, new StringContent(json, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            return await JsonSerializer.DeserializeAsync<Vet>(response.Content.ReadAsStream(), options);
        }

        public async Task<VetClinic> GetClinicById(Guid id)
        {
            return await JsonSerializer.DeserializeAsync<VetClinic>(
                await httpClient.GetStreamAsync(ApiURL + "/" + id),
                new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true });
        }       

    }
}
