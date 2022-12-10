using System.Net;
using System.Xml.Linq;

namespace VetAppointment.Business.Test
{
    public class PetTests
    {
        [Fact]
        public void When_CreatePet_Then_ShouldReturnPet()
        {
            // Arrange
            var sut = CreateSUT();

            // Act
            var result = Pet.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
            result.Entity.Name.Should().Be(sut.Item1);
            result.Entity.Birthdate.Should().Be(DateTime.Parse(sut.Item2));
            result.Entity.Race.Should().Be(Enum.Parse<AnimalRace>(sut.Item3));
            result.Entity.Gender.Should().Be(Enum.Parse<AnimalGender>(sut.Item4));
        }

        [Fact]
        public void When_CreatePetWithInvalidGender_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var gender = "male";

            // Act
            var result = Pet.Create(sut.Item1, sut.Item2, sut.Item3, gender);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_CreatePetWithInvalidSpecie_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var specie = "cat";

            // Act
            var result = Pet.Create(sut.Item1, sut.Item2, specie, sut.Item4);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_CreatePetWithInvalidBirthdate_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var badBirthdate = "22/22/22";

            // Act
            var result = Pet.Create(sut.Item1, badBirthdate, sut.Item3, sut.Item4);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_RegisterPetToClinic_Then_IdShouldNotBeNull()
        {
            // Arrange
            var sut = CreateSUT();
            var sutClinic = CreateSUTForClinic();
            var pet = Pet.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4).Entity;
            var clinic = VetClinic.Create(sutClinic.Item1, sutClinic.Item2, sutClinic.Item3, sutClinic.Item4, sutClinic.Item5).Entity;

            // Act
            pet.RegisterPetToClinic(clinic);

            // Assert
            pet.ClinicId.Should().Be(clinic.Id);
        }

        [Fact]
        public void When_ConnectPetToOwner_Then_IdShouldNotBeNull()
        {
            // Arrange
            var sut = CreateSUT();
            var sutOwner = CreateSUTForOwner();
            var pet = Pet.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4).Entity;
            var owner = PetOwner.Create(sutOwner.Item1, sutOwner.Item2, sutOwner.Item3, sutOwner.Item4, sutOwner.Item5, sutOwner.Item6, sutOwner.Item7).Entity;

            // Act
            pet.ConnectToOwner(owner);

            // Assert
            pet.OwnerId.Should().Be(owner.Id);
        }

        [Fact]
        public void When_UpdatePet_Then_ShouldReturnSuccess()
        {
            // Arrange
            var sut = CreateSUT();
            var pet = Pet.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4).Entity;

            // Act
            var result = pet.Update(pet.Name, pet.Birthdate.ToString(), pet.Race.ToString(), pet.Gender.ToString());

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void When_UpdatePetWithInvalidRace_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var pet = Pet.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4).Entity;
            var race = "invalidRace";

            // Act
            var result = pet.Update(pet.Name, pet.Birthdate.ToString(), race, pet.Gender.ToString());

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_UpdatePetWithInvalidGender_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var pet = Pet.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4).Entity;
            var gender = "invalidGender";

            // Act
            var result = pet.Update(pet.Name, pet.Birthdate.ToString(), pet.Race.ToString(), gender);

            // Assert
            result.IsFailure.Should().BeTrue();
        }


        [Fact]
        public void When_UpdatePetWithInvalidBirthdate_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var pet = Pet.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4).Entity;
            var birthdate = "22/22/22";

            // Act
            var result = pet.Update(pet.Name, birthdate, pet.Race.ToString(), pet.Gender.ToString());

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        private static Tuple<string, string, string, string> CreateSUT()
        {
            return new Tuple<string, string, string, string>("Pisacio", "12/06/2020", "Cat", "Male");
        }
        private static Tuple<string, string, int, string, string> CreateSUTForClinic()
        {
            return new Tuple<string, string, int, string, string>("Vet Clinic", "Address", 10, "email@gmail.com", "+40123456789");
        }
        private static Tuple<string, string, string, string, string, string, string> CreateSUTForOwner()
        {
            return new Tuple<string, string, string, string, string, string, string>(
                "John", "Doe", "12/02/2001", "Male", "Address", "john.doe@gmail.com", "+40756221345");
        }
    }
}