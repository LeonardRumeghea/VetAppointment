using VetAppointment.Application;
using VetAppointment.Domain;
using VetAppointment.Infrastructure.Repositories.GenericRepositories;

namespace VetAppointment.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext context;

        public UnitOfWork(DatabaseContext context) => this.context = context;

        private IRepository<Appointment> appointmentRepository;
        public IRepository<Appointment> AppointmentRepository
        {
            get
            {
                appointmentRepository ??= new AppointmentRepository(context);
                return appointmentRepository;
            }
        }


        private IRepository<Drug> drugRepository;
        public IRepository<Drug> DrugRepository
        {
            get
            {
                drugRepository ??= new DrugRepository(context);
                return drugRepository;
            }
        }

        private IRepository<MedicalHistory> medicalHistoryRepository;
        public IRepository<MedicalHistory> MedicalHistoryRepository
        {
            get
            {
                medicalHistoryRepository ??= new MedicalHistoryRepository(context);
                return medicalHistoryRepository;
            }
        }

        private IRepository<PetOwner> petOwnerRepository;
        public IRepository<PetOwner> PetOwnerRepository
        {
            get
            {
                petOwnerRepository ??= new PetOwnerRepository(context);
                return petOwnerRepository;
            }
        }

        private IRepository<Pet> petRepository;
        public IRepository<Pet> PetRepository
        {
            get
            {
                petRepository ??= new PetRepository(context);
                return petRepository;
            }
        }

        private IRepository<PrescribedDrug> prescribedDrugRepository;
        public IRepository<PrescribedDrug> PrescribedDrugRepository
        {
            get
            {
                prescribedDrugRepository ??= new PrescribedDrugRepository(context);
                return prescribedDrugRepository;
            }
        }

        private IRepository<Treatment> treatmentRepository;
        public IRepository<Treatment> TreatmentRepository
        {
            get
            {
                treatmentRepository ??= new TreatmentRepository(context);
                return treatmentRepository;
            }
        }

        private IRepository<VetClinic> vetClinicRepository;
        public IRepository<VetClinic> VetClinicRepository
        {
            get
            {
                vetClinicRepository ??= new VetClinicRepository(context);
                return vetClinicRepository;
            }
        }

        private IRepository<Vet> vetRepository;
        public IRepository<Vet> VetRepository
        {
            get
            {
                vetRepository ??= new VetRepository(context);
                return vetRepository;
            }
        }

        public void SaveChanges() => context.SaveChanges();
    }
}
