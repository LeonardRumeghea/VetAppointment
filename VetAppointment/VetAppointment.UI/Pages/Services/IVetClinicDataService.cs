using VetAppointment.Shared.Domain;

namespace VetAppointment.UI.Pages.Services
{
    public interface IVetClinicDataService
    {
        Task<IEnumerable<VetClinic>> GetAllClinics();
        Task<VetClinic> AddClinic(VetClinic clinic);
        Task<VetClinic> GetClinicById(Guid id);
        Task<string> AddVetToClinic(Guid clinicId, Vet vet);
        //Task<Pet> AddPetsToClinic(Guid clinicId, List<Pet> pets);
    }
}
