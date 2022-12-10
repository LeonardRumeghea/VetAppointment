#nullable disable
using VetAppointment.API.Dtos.Create;

namespace VetAppointment.API.Dtos
{
    public class VetDto : CreateVetDto
    {
        public Guid Id { get; set; }
        public Guid ClinicId { get; set; }
    }
}
