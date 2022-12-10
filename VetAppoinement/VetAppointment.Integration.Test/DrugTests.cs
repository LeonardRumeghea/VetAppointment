using VetAppointment.API.Dtos;
using VetAppointment.Integration.Tests;

namespace VetAppointment.Integration.Test
{
    public class DrugTests : BaseIntegrationTests, IDisposable
    {

        private const string ApiURL = "v1/api/drugs";

        [Fact]
        public async void When_CreateDrugWithValidData_Then_ShouldAddToDataBase()
        {
            var drugSUT = CreateDrugSUT();

            // Act
            var createDrugResponse = await HttpClient.PostAsJsonAsync(ApiURL, drugSUT);
            var getDrugResult = await HttpClient.GetAsync(ApiURL);

            // Assert
            createDrugResponse.EnsureSuccessStatusCode();
            createDrugResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            getDrugResult.EnsureSuccessStatusCode();
            var clinics = await getDrugResult.Content.ReadFromJsonAsync<List<VetClinicDto>>();

            clinics.Should().HaveCount(1);
            clinics.Should().NotBeNull();
        }

        private static DrugDto CreateDrugSUT()
        {
            return new DrugDto
            {
                Name = "Aspirina Saracului",
                Quantity = 9999,
                UnitPrice = 0.1
            };
        }

        public void Dispose()
        {
            CleanDatabases();
        }
    }
}
