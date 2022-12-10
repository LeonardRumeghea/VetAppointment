namespace VetAppointment.API.Dtos.Create
{
    public class CreateAppointmentDto
    {
        public string ScheduledDate { get; set; }

        public int EstimatedDurationInMinutes { get; set; }

        public Guid VetId { get; set; }

        public Guid PetId { get; set; }

        public Guid TreatmentId { get; set; }

        public Guid MedicalHistoryId { get; set; }
    }
}
