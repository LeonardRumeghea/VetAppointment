using VetAppointment.Domain.Enums;
using VetAppointment.Domain.Helpers;

#nullable disable
namespace VetAppointment.Domain
{
    public class Vet : Person
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public Guid ClinicId { get; private set; }

        public VetSpecialisation Specialisation { get; private set; }

        public static Result<Vet> Create(string name, string surname, string birthdate,
            string gender, string email, string phone, string specialisation)
        {
            if (!Validations.IsValidEmail(email))
            {
                return Result<Vet>.Failure($"Email {email} is not valid");
            }

            if (!Validations.IsValidPhoneNumber(phone))
            {
                return Result<Vet>.Failure($"Phone number {phone} is not valid");
            }

            if (!DateTime.TryParse(birthdate, out DateTime date))
            {
                return Result<Vet>.Failure($"Invalid birthdate - {birthdate}!");
            }

            if (!Enum.TryParse<PersonGender>(gender, out var personGender))
            {
                var expectedGenderValues = Enum.GetNames(typeof(PersonGender));
                var textExpectedGenderValues = string.Join(", ", expectedGenderValues);
                return Result<Vet>.Failure($"The provided gender {gender} is not one from the possible races: {textExpectedGenderValues}");
            }
            if (!Enum.TryParse<VetSpecialisation>(specialisation, out var vetSpecialisation))
            {
                var expectedProfileValues = Enum.GetNames(typeof(VetSpecialisation));
                var textExpectedProfileValues = string.Join(", ", expectedProfileValues);
                return Result<Vet>.Failure($"The provided specialisation {specialisation} is not one from the possible specialisations: {textExpectedProfileValues}");
            }


            var vet = new Vet
            {
                Id = Guid.NewGuid(),
                Name = name,
                Surname = surname,
                Birthdate = date,
                Gender = personGender,
                Email = email,
                Phone = phone,
                Specialisation = vetSpecialisation
            };
            
            return Result<Vet>.Success(vet);
        }

        public void RegisterVetToClinic(VetClinic clinic)
        {
            ClinicId = clinic.Id;
        }
    }
}
