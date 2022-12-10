using VetAppointment.API.Dtos.Create;

namespace VetAppointment.API.Dtos
{
    public class TreatmentDto : CreateTreatmentDto
    {
        public Guid Id { get; set; }
    }
}
