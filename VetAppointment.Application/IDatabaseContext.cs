using Microsoft.EntityFrameworkCore;
using VetAppointment.Domain;

namespace VetAppointment.Application
{
    public interface IDatabaseContext
    {
        public DbSet<Pet> Pets { get; }
        public DbSet<Vet> Vets { get; }
        public DbSet<PetOwner> PetOwners { get; }
        public DbSet<VetClinic> VetClinics { get; }
        public DbSet<Appointment> Appointments { get; }
        public DbSet<Drug> Drugs { get; }
        public DbSet<MedicalHistory> MedicalHistories { get; }
        public DbSet<Treatment> Treatments { get; }
        public DbSet<PrescribedDrug> PrescribedDrugs { get; }

        void Save();
    }
}
