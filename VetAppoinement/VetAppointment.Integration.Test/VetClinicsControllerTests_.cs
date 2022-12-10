#nullable disable
using VetAppointment.Domain.Enums;

namespace VetAppointment.Integration.Tests
{
    public class VetClinicsControllerTests : BaseIntegrationTests, IDisposable
    {
        private const string ApiURL = "v1/api/vetclinics";

        [Fact]
        public async void When_CreateClinic_Then_ShouldReturnClinicInTheGetRequestAsync()
        {
            // Arrange
            CreateVetClinicDto vetClinicDto = CreateClinitSUT();
            
            // Act
            var createClinicResponse = await HttpClient.PostAsJsonAsync(ApiURL, vetClinicDto);
            var getClinicResult = await HttpClient.GetAsync(ApiURL);

            // Assert
            createClinicResponse.EnsureSuccessStatusCode();
            createClinicResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            getClinicResult.EnsureSuccessStatusCode();
            var clinics = await getClinicResult.Content.ReadFromJsonAsync<List<VetClinicDto>>();

            clinics.Should().HaveCount(1);
            clinics.Should().NotBeNull();
        }

        [Fact]
        public async Task When_RegisterPetsFamilyToClinic_Then_ShouldSavePetsInClinicAsync()
        {
            // Arrange
            var createVetClinicDto = CreateClinitSUT();
            var createClinicResponse = await HttpClient.PostAsJsonAsync(ApiURL, createVetClinicDto);
            var pets = new List<PetDto> { CreatePetSUT() };
            var clinic = await createClinicResponse.Content.ReadFromJsonAsync<VetClinicDto>();

            // Act
            var resultResponse = await HttpClient.PostAsJsonAsync($"{ApiURL}/{clinic.Id}/pets", pets);

            // Assert
            //resultResponse();
            resultResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async void When_RegisterEmptyListOfPetsToClinic_Then_ShouldReturnBadRequestAsync()
        {
            // Arrange
            CreateVetClinicDto createVetClinicDto = CreateClinitSUT();
            var createClinicResponse = await HttpClient.PostAsJsonAsync(ApiURL, createVetClinicDto);
            var pets = new List<PetDto>();
            var clinic = await createClinicResponse.Content.ReadFromJsonAsync<VetClinicDto>();

            // Act
            var resultResponse = await HttpClient.PostAsJsonAsync($"{ApiURL}/{clinic.Id}/pets", pets);

            // Assert
            resultResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void When_RegisterVetToClinic_Then_ShouldSaveVetInClinicAsync()
        {
            // Arrange
            var createVetClinicDto = CreateClinitSUT();
            var createClinicResponse = await HttpClient.PostAsJsonAsync(ApiURL, createVetClinicDto);
            var vet = CreateVetSUT();
            var clinic = await createClinicResponse.Content.ReadFromJsonAsync<VetClinicDto>();

            // Act
            var resultResponse = await HttpClient.PostAsJsonAsync($"{ApiURL}/{clinic.Id}/vet", vet);

            // Assert
            resultResponse.EnsureSuccessStatusCode();
            resultResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async void When_UpdateVet_Then_ShouldUpdateVetInClinic()
        {
            // Arrange
            var clinicSUT = CreateClinitSUT();
            var createClinicResponse = await HttpClient.PostAsJsonAsync(ApiURL, clinicSUT);
            var clinic = await createClinicResponse.Content.ReadFromJsonAsync<VetClinicDto>();

            var vetSUT = CreateVetSUT();
            var resultResponse = await HttpClient.PostAsJsonAsync($"{ApiURL}/{clinic.Id}/vet", vetSUT);
            var vet = await resultResponse.Content.ReadFromJsonAsync<VetDto>();

            vet.Name = "UpdatedFirstName";

            // Act
            var updateVetResponse = await HttpClient.PutAsJsonAsync($"{ApiURL}/{clinic.Id}/vet/{vet.Id}", vet);

            // Assert
            updateVetResponse.EnsureSuccessStatusCode();
            updateVetResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async void When_DeleteVet_Then_ShouldDeleteVetFromClinic()
        {
            // Arrange
            var clinicSUT = CreateClinitSUT();
            var createClinicResponse = await HttpClient.PostAsJsonAsync(ApiURL, clinicSUT);
            var clinic = await createClinicResponse.Content.ReadFromJsonAsync<VetClinicDto>();

            var vetSUT = CreateVetSUT();
            var resultResponse = await HttpClient.PostAsJsonAsync($"{ApiURL}/{clinic.Id}/vet", vetSUT);
            var vet = await resultResponse.Content.ReadFromJsonAsync<VetDto>();

            // Act
            var deleteVetResponse = await HttpClient.DeleteAsync($"{ApiURL}/{clinic.Id}/vet/{vet.Id}");

            // Assert
            deleteVetResponse.EnsureSuccessStatusCode();
            deleteVetResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async void When_DeleteClinic_Then_ShouldDeleteClinic()
        {
            // Arrange
            var clinicSUT = CreateClinitSUT();
            var createClinicResponse = await HttpClient.PostAsJsonAsync(ApiURL, clinicSUT);
            var clinic = await createClinicResponse.Content.ReadFromJsonAsync<VetClinicDto>();

            // Act
            var deleteClinicResponse = await HttpClient.DeleteAsync($"{ApiURL}/{clinic.Id}");

            // Assert
            deleteClinicResponse.EnsureSuccessStatusCode();
            deleteClinicResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        private static CreateVetClinicDto CreateClinitSUT()
        {
            return new CreateVetClinicDto()
            {
                Name = "Pet Lovers",
                Address = "strada animalutelor",
                NumberOfPlaces = 3,
                ContactEmail = "love@pets.com",
                ContactPhone = "+40742845280"
            };
        }

        private static PetDto CreatePetSUT()
        {
            return new PetDto()
            {
                Name = "Bobita",
                Birthdate = DateTime.Now.AddYears(-1).ToString(),
                Race = AnimalRace.Rabbit.ToString(),
                Gender = AnimalGender.Male.ToString()
            };
        }

        private static VetDto CreateVetSUT()
        {
            return new VetDto()
            {
                Name = "Maria",
                Surname = "Popovici",
                Birthdate = DateTime.Now.AddYears(-32).ToString(),
                Email = "maria.popovici@gmail.com",
                Phone = "+40123456789",
                Gender = PersonGender.Female.ToString(),
                Specialisation = VetSpecialisation.Nutritionist.ToString()
            };
        }

        public void Dispose()
        {
            CleanDatabases();
        }
    }
}
