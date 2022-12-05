using Microsoft.EntityFrameworkCore;
using VetAppointment.Application;
using VetAppointment.Domain;

namespace VetAppointment.Infrastructure.Data
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Pet> Pets => Set<Pet>();
        public DbSet<Vet> Vets => Set<Vet>();
        public DbSet<PetOwner> PetOwners => Set<PetOwner>();
        public DbSet<VetClinic> VetClinics => Set<VetClinic>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<Drug> Drugs => Set<Drug>();
        public DbSet<MedicalHistory> MedicalHistories => Set<MedicalHistory>();
        public DbSet<Treatment> Treatments => Set<Treatment>();
        public DbSet<PrescribedDrug> PrescribedDrugs => Set<PrescribedDrug>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source = VetAppointment_Test.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VetClinic>().HasData(new List<VetClinic> {
                VetClinic.Create("Casa Animalelor", "Str. 1 Decembrie Nr. 25", 124, "contact@casa_animalelor.com", "+40712345678").Entity,
                VetClinic.Create("Zoo-Vet", "Str. Primaverii Nr. 15", 64, "contact@zoo_vet.com", "+40778945612").Entity
            });
        }

        public void Save() => SaveChanges();
    }
}