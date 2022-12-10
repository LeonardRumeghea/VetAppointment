namespace VetAppointment.Business.Test
{
    public class PetOwnerTests
    {
        [Fact]
        public void When_CreatePetOwner_Then_ShouldReturnPetOwner()
        {
            // Arrange
            var sut = CreateSUT();

            // Act
            var result = PetOwner.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, sut.Item6, sut.Item7);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
        }

        [Fact]
        public void When_CreatePetOwnerWithInvalidGender_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var gender = "invalid";

            // Act
            var result = PetOwner.Create(sut.Item1, sut.Item2, sut.Item3, gender, sut.Item5, sut.Item6, sut.Item7);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_CreatePetOwnerWithInvalidEmail_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var mail = "invalidmail";

            // Act
            var result = PetOwner.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, mail, sut.Item7);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_CreatePetOwnerWithInvalidPhoneNumber_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            string phoneNumber = null;

            // Act
            var result = PetOwner.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, sut.Item6, phoneNumber);
            
            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_CreatePetOwnerWithInvalidBirthdate_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var date = "invaliddate";

            // Act
            var result = PetOwner.Create(sut.Item1, sut.Item2, date, sut.Item4, sut.Item5, sut.Item6, sut.Item7);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
        
        [Fact]
        public void When_RegisterPetsToOwner_Then_ShouldReturnSuccess()
        {
            // Arrange
            var sut = CreateSUT();
            var petOwner = PetOwner.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, sut.Item6, sut.Item7).Entity;

            var sutPet = CreateSUTForPet();
            var pet = Pet.Create(sutPet.Item1, sutPet.Item2, sutPet.Item3, sutPet.Item4).Entity;
            var pets = new List<Pet>();
            pets.Add(pet);

            // Act
            var result = petOwner.RegisterPetsToOwner(pets);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void When_RegisterNoPetsToOwner_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var petOwner = PetOwner.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, sut.Item6, sut.Item7).Entity;

            var pets = new List<Pet>();

            // Act
            var result = petOwner.RegisterPetsToOwner(pets);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        private Tuple<string, string, string, string, string, string,string> CreateSUT()
        {
            return new Tuple<string, string, string, string, string, string, string> (
                "John", "Doe", "12/10/2001", "Male", "Str. Scolii Nr. 4 Bl. 2 Sc. 1 Ap. 3 Iasi Romania", "john.doe@gmail.com", "+40756221345");
        }
        private Tuple<string, string, string, string> CreateSUTForPet()
        {
            return new Tuple<string, string, string, string>("Pisacio", "12/06/2020", "Cat", "Male");
        }
    }
}
