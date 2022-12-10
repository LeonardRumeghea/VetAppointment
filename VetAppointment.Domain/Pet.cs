using VetAppointment.Domain.Enums;
using VetAppointment.Domain.Helpers;

#nullable disable
namespace VetAppointment.Domain
{
    public class Pet
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public DateTime Birthdate { get; private set; }
        public AnimalRace Race { get; private set; }
        public AnimalGender Gender { get; private set; }
        public Guid OwnerId { get; private set; }
        public Guid ClinicId { get; private set; }

        public static Result<Pet> Create(string name, string birthdate, string race, string gender)
        {
            if(!Enum.TryParse<AnimalRace>(race, out var animalRace))
            {
                var expectedRaceValues = Enum.GetNames(typeof(AnimalRace));
                var textExpectedRaceValues = string.Join(", ", expectedRaceValues);
                return Result<Pet>.Failure($"The provided race {race} is not one from the possible races: {textExpectedRaceValues}");
            }
            
            if (!Enum.TryParse<AnimalGender>(gender, out var animalGender))
            {
                var expectedGenderValues = Enum.GetNames(typeof(AnimalGender));
                var textExpectedGenderValues = string.Join(", ", expectedGenderValues);
                return Result<Pet>.Failure($"The provided gender {gender} is not one from the possible races: {textExpectedGenderValues}");
            }

            if (!DateTime.TryParse(birthdate, out DateTime date))
            {
                return Result<Pet>.Failure($"Invalid birthdate - {birthdate}!");
            }

            var pet = new Pet
            {
                Id = Guid.NewGuid(),
                Name = name,
                Birthdate = date,
                Race = animalRace,
                Gender = animalGender
            };

            return Result<Pet>.Success(pet);
        }

        public void RegisterPetToClinic(VetClinic clinic)
        {
            ClinicId = clinic.Id;
        }

        public void ConnectToOwner(PetOwner petOwner)
        {
            OwnerId = petOwner.Id;
        }
    
        public Result Update(string name, string birthdate, string race, string gender)
        {
            if (!Enum.TryParse<AnimalRace>(race, out var animalRace))
            {
                var expectedRaceValues = Enum.GetNames(typeof(AnimalRace));
                var textExpectedRaceValues = string.Join(", ", expectedRaceValues);
                return Result.Failure($"The provided race {race} is not one from the possible races: {textExpectedRaceValues}");
            }

            if (!Enum.TryParse<AnimalGender>(gender, out var animalGender))
            {
                var expectedGenderValues = Enum.GetNames(typeof(AnimalGender));
                var textExpectedGenderValues = string.Join(", ", expectedGenderValues);
                return Result.Failure($"The provided gender {gender} is not one from the possible races: {textExpectedGenderValues}");
            }

            if (!DateTime.TryParse(birthdate, out DateTime date))
            {
                return Result.Failure($"Invalid birthdate - {birthdate}!");
            }

            Name = name;
            Birthdate= date;
            Race = animalRace;
            Gender = animalGender;

            return Result.Success();
        }
    }
}
