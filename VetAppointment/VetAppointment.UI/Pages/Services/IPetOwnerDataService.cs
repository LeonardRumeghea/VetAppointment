using VetAppointment.Shared.Domain;

namespace VetAppointment.UI.Pages.Services
{
    public interface IPetOwnerDataService
    {
        //Task<PetOwner> AddPetOwner(PetOwner petOwner);
        //Task<PetOwner> AddPetsToPetOwner(Guid petOwnerId, List<Pet> pets);
        Task<IEnumerable<PetOwner>> GetAllPetOwners();
    }
}