#nullable disable
namespace VetAppointment.Integration.Tests
{
    public class PetOwnersControllerTests : BaseIntegrationTests, IDisposable
    {
        private const string ApiURL = "v1/api/petowners";

        [Fact]
        public async Task When_RegisterPetsToOwner_Then_ShouldSavePetsToOwnerAsync()
        {
            // Arrange
            CreatePetOwnerDto createPetOwnerDto = CreateSUT();
            var createPetOwnerResponse = await HttpClient.PostAsJsonAsync("v1/API/PetOwners", createPetOwnerDto);
            var pets = new List<PetDto>
            {
                new PetDto
                {
                    Name = "Bobita",
                    Birthdate = "9/10/2021",
                    Race = "Dog",
                    Gender = "Male"
                }
            };
            var petOwner = await createPetOwnerResponse.Content.ReadFromJsonAsync<PetOwnerDto>();

            // Act
            var resultResponse = await HttpClient.PostAsJsonAsync($"{ApiURL}/{petOwner.Id}/pets", pets);

            // Assert
            resultResponse.EnsureSuccessStatusCode();
            resultResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async void When_RegisterEmptyListOfPetsToOwner_Then_ShouldReturnBadRequestAsync()
        {
            // Arrange
            CreatePetOwnerDto createPetOwnerDto = CreateSUT();

            var createPetOwnerResponse = await HttpClient.PostAsJsonAsync(ApiURL, createPetOwnerDto);
            var pets = new List<PetDto>();
            var petOwner = await createPetOwnerResponse.Content.ReadFromJsonAsync<PetOwnerDto>();

            // Act
            var resultResponse = await HttpClient.PostAsJsonAsync($"{ApiURL}/{petOwner.Id}/pets", pets);

            // Assert
            resultResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private static CreatePetOwnerDto CreateSUT()
        {
            return new CreatePetOwnerDto
            {
                Name = "Ion",
                Surname = "Ionescu",
                Birthdate = "19-10-1986",
                Gender = "Male",
                Address = "Iasi",
                Email = "ion@gmail.com",
                Phone = "+40732961298"
            };
        }

        public void Dispose()
        {
            CleanDatabases();
        }
    }
}
