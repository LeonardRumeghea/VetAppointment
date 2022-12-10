namespace VetAppointment.Shared.Domain
{
    public class Vet : Person
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ClinicId { get; set; }

        public string Specialisation { get; set; }
    }
}