using VetAppointment.API.Dtos.Create;

namespace VetAppointment.API.Dtos
{
    public class AppointmentDto : CreateAppointmentDto
    {
        public Guid Id { get; set; }
    }
}
