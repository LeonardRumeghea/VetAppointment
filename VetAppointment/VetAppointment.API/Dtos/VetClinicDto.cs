using VetAppointment.API.Dtos.Create;

namespace VetAppointment.API.Dtos
{
    public class VetClinicDto : CreateVetClinicDto
    {
        public Guid Id { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
