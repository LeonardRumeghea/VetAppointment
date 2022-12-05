using System.Text;
using System.Text.Json;
using VetAppointment.Shared.Domain;

namespace VetAppointment.UI.Pages.Services
{
    public class AppointmentDataService : IAppointmentDataService
    {
        private const string ApiURL = "https://localhost:7112/v1/api/appointments";
        private readonly HttpClient httpClient;

        public AppointmentDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<Appointment>> GetAllAppointments()
        {
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return await JsonSerializer.DeserializeAsync<IEnumerable<Appointment>>
                (await httpClient.GetStreamAsync(ApiURL), options);
        }

        public async Task<Appointment> AddAppointment(Appointment appointment)
        {
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var json = JsonSerializer.Serialize(appointment);
            var response = await httpClient.PostAsync
                (ApiURL, new StringContent(json, UnicodeEncoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            return await JsonSerializer.DeserializeAsync<Appointment>(response.Content.ReadAsStream(), options);
        }
    }
}
