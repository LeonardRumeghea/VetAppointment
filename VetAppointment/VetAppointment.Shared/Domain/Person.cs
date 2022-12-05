namespace VetAppointment.Shared.Domain
{
    public abstract class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Birthdate { get; set; }
        public string Gender { get; set; }
    }
}
