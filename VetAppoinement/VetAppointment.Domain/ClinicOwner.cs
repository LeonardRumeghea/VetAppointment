#nullable disable
using VetAppointment.Domain.Enums;
using VetAppointment.Domain.Helpers;

namespace VetAppointment.Domain
{
    public class ClinicOwner : Person
    {
        public string Email { get; private set; }
        public string Phone { get; private set; }

        public static Result<ClinicOwner> Create(string name, string surname, string birthdate, string gender, string email, string phone)
        {
            if (!Validations.IsValidEmail(email))
            {
                return Result<ClinicOwner>.Failure($"Email {email} is not valid");
            }

            if (!Validations.IsValidPhoneNumber(phone))
            {
                return Result<ClinicOwner>.Failure($"Phone number {phone} is not valid");
            }

            if (!DateTime.TryParse(birthdate, out DateTime date))
            {
                return Result<ClinicOwner>.Failure($"Invalid birthdate - {birthdate}!");
            }

            if (!Enum.TryParse<PersonGender>(gender, out PersonGender personGender))
            {
                var expectedGenderValues = Enum.GetNames(typeof(PersonGender));
                var textExpectedGenderValues = string.Join(", ", expectedGenderValues);
                return Result<ClinicOwner>.Failure($"The provided gender {gender} is not one from the possible genders: {textExpectedGenderValues}");
            }

            var clinicOwner = new ClinicOwner
            {
                Name = name,
                Surname = surname,
                Birthdate = date,
                Gender = personGender,
                Email = email,
                Phone = phone,
            };

            return Result<ClinicOwner>.Success(clinicOwner);
        }
    }
}
