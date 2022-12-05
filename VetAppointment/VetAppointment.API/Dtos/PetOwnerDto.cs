using VetAppointment.API.Dtos.Create;

namespace VetAppointment.API.Dtos
{
    public class PetOwnerDto : CreatePetOwnerDto
    {
        public Guid Id { get; set; }
    }
}
