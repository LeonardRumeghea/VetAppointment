#nullable disable
namespace VetAppointment.Shared.Domain
{
    public class Pet
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Birthdate { get; set; }
        public string Race { get; set; }
        public string Gender { get; set; }
    }
}
