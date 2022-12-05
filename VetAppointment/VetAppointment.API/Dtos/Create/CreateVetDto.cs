namespace VetAppointment.API.Dtos.Create
{
    public class CreateVetDto : VetDto
    {
        public CreateVetDto(string name, string surname, string birthdate, string gender, string email, string phone, string specialisation) : base(name, surname, birthdate, gender, email, phone, specialisation)
        {
        }

        public Guid Id { get; set; }
        public Guid ClinicId { get; set; }
    }
}
