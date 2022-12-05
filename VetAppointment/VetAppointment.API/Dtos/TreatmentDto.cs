using VetAppointment.API.Dtos.Create;

namespace VetAppointment.API.Dtos
{
    # nullable disable
    public class TreatmentDto : CreateTreatmentDto
    {
        public Guid Id { get; set; }
    }
}
