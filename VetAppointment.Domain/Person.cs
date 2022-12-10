using VetAppointment.Domain.Enums;

#nullable disable
namespace VetAppointment.Domain
{
    public abstract class Person
    {
        public string Name { get; protected set; }
        public string Surname { get; protected set; }
        public DateTime Birthdate { get; protected set; }
        public PersonGender Gender { get; protected set; }
    }
}
