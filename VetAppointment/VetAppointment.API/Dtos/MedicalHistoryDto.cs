using VetAppointment.API.Dtos.Create;

namespace VetAppointment.API.Dtos
{
    public class MedicalHistoryDto : CreateMedicalHistoryDto
    {
        public Guid Id { get; set; }
    }
}
