#nullable disable
namespace VetAppointment.API.Dtos.Create
{
    public class CreatePetDto
    {
        public string Name { get; set; }
        public string Birthdate { get; set; }
        public string Race { get; set; }
        public string Gender { get; set; }
    }
}
