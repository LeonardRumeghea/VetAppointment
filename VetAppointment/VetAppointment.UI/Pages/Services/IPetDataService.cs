namespace VetAppointment.UI.Pages.Services
{
    public interface IPetDataService
    {
        Task<IEnumerable<Pet>> GetAllPets();
        //Task<Pet> GetPetsDetail(Guid petId);
        Task<Pet> AddPet(Pet pet);
        //Task UpdatePet(Pet pet);
        //Task DeletePet(Guid petId);
    }
}