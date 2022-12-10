using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using VetAppointment.API.Controllers;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.Integration.Tests
{
    public class BaseIntegrationTests
    {
        private DbContextOptions<DatabaseContext> options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlite("Data Source = VetAppointmentTest.db").Options;
        private DatabaseContext databaseContext { get; set; }


        protected HttpClient HttpClient { get; private set; }
        protected BaseIntegrationTests()
        {
            HttpClient = new WebApplicationFactory<VetClinicsController>().WithWebHostBuilder(builder => { }).CreateClient();
            databaseContext = new DatabaseContext(options);
            CleanDatabases();
        }

        protected void CleanDatabases()
        {
            databaseContext.PetOwners.RemoveRange(databaseContext.PetOwners.ToList());
            databaseContext.VetClinics.RemoveRange(databaseContext.VetClinics.ToList());
            databaseContext.Appointments.RemoveRange(databaseContext.Appointments.ToList());
            databaseContext.Pets.RemoveRange(databaseContext.Pets.ToList());
            databaseContext.Vets.RemoveRange(databaseContext.Vets.ToList());
            databaseContext.Drugs.RemoveRange(databaseContext.Drugs.ToList());
            databaseContext.SaveChanges();
        }
    }
}
