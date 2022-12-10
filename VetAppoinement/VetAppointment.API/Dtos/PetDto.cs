using VetAppointment.API.Dtos.Create;

namespace VetAppointment.API.Dtos
{
    public class PetDto : CreatePetDto
    {
        public Guid Id { get; set; }
    }
}
