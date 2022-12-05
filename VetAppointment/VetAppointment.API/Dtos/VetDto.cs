#nullable disable
namespace VetAppointment.API.Dtos
{
    public class VetDto
    {
        public VetDto()
        {
            
        }
        public VetDto(string name, string surname, string birthdate, string gender,
            string email, string phone, string specialisation)
        {
            Name = name;
            Surname = surname;
            Birthdate = birthdate;
            Gender = gender;
            Email = email;
            Phone = phone;
            Specialisation = specialisation;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Birthdate { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Specialisation { get; set; }
    }
}
