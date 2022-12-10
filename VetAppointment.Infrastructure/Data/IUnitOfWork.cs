using VetAppointment.Application;
using VetAppointment.Domain;

namespace VetAppointment.Infrastructure.Data
{
    public interface IUnitOfWork
    {
        IRepository<Appointment> AppointmentRepository { get; }
        IRepository<Drug> DrugRepository { get; }
        IRepository<MedicalHistory> MedicalHistoryRepository { get; }
        IRepository<PetOwner> PetOwnerRepository { get; }
        IRepository<Pet> PetRepository { get; }
        IRepository<PrescribedDrug> PrescribedDrugRepository { get; }
        IRepository<Treatment> TreatmentRepository { get; }
        IRepository<VetClinic> VetClinicRepository { get; }
        IRepository<Vet> VetRepository { get; }

        void SaveChanges();
    }
}