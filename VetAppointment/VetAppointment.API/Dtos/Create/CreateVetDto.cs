namespace VetAppointment.API.Dtos.Create
{
    public class CreateVetDto : VetDto
    {
        public Guid Id { get; set; }
        public Guid clinicId { get; set; }
    }
}
