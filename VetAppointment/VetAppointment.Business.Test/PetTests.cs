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
        
        private Tuple<string, string, string, string> CreateSUT()
        {
            return new Tuple<string, string, string, string>("Pisacio", "12/6/2020", "Cat", "Male");
        }
    }
}
