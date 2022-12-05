#nullable disable
namespace VetAppointment.Domain
{
    public class ClinicOwner : Person
    {
        public string Email { get; private set; }
        public string Phone { get; private set; }
    }
}
