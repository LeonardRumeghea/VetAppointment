using VetAppointment.Domain.Enums;

namespace VetAppointment.Integration.Tests
{
    public class AppointmentsControllerTests : BaseIntegrationTests, IDisposable
    {
        private const string ApiURL = "v1/api/appointments";

        public void Dispose()
        {
            CleanDatabases();
        }

        //[Fact]
        //public async Task When_CreateAppointment_Then_ShouldReturnAppointmentInTheGetRequestAsync()
        //{
        //    // Arrange

        //    var pet = CreatePetSUT();
        //    var vet = CreateVetSUT();
        //    var postPet = await PostAsync("v1/api", pet);

        //    CreateAppointmentDto appointmentDto = CreateAppoinmentSUT();
        //    // Act
        //    var createAppointmentResponse = await HttpClient.PostAsJsonAsync(ApiURL, appointmentDto);
        //    var getAppointmentResult = await HttpClient.GetAsync(ApiURL);
        //    // Assert
        //    createAppointmentResponse.EnsureSuccessStatusCode();
        //    createAppointmentResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        //    getAppointmentResult.EnsureSuccessStatusCode();
        //    var appointments = await getAppointmentResult.Content.ReadFromJsonAsync<List<AppointmentDto>>();
        //    appointments.Should().HaveCount(1);
        //    appointments.Should().NotBeNull();
        //}

        private CreateAppointmentDto CreateAppoinmentSUT()
        {
            return new CreateAppointmentDto
            {
                ScheduledDate = DateTime.Now.AddDays(1).ToString(),
                EstimatedDurationInMinutes = 20,
                VetId = CreateVetSUT().Id,
                PetId = CreatePetSUT().Id,
            };
        }

        private PetDto CreatePetSUT()
        {
            return new PetDto
            {
                Name = "TestPet",
                Birthdate = DateTime.Now.AddYears(-1).ToString(),
                Gender = AnimalGender.Male.ToString(),
                Race = AnimalRace.Cat.ToString()
            };
        }

        private VetDto CreateVetSUT()
        {
            return new VetDto
            {
                Name = "TestVet",
                Surname = "TestSurname",
                Birthdate = DateTime.Now.AddYears(-22).ToString(),
                Email = "test@email.com",
                Phone = "+40123456789",
                Gender = PersonGender.Male.ToString(),
                Specialisation = VetSpecialisation.DentalCaretaker.ToString()
            };
        }


    }
}
