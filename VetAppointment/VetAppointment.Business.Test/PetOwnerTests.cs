namespace VetAppointment.Business.Test
{
    public class PetOwnerTests
    {
        [Fact]
        public void When_CreatePetOwner_Then_ShouldReturnPetOWner()
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
            var gender = "male";

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
            var result = PetOwner.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item5, sut.Item6, sut.Item7, mail);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_CreatePetOwnerWithInvalidPhoneNumber_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var phoneNumber = "invalidPhoneNumber";

            // Act
            var result = PetOwner.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item6, sut.Item7, phoneNumber);
            
            // Assert
            result.IsFailure.Should().BeTrue();
        }

        private Tuple<string, string, string, string, string, string,string> CreateSUT()
        {
            return new Tuple<string, string, string, string, string, string, string> (
                "John", "Doe", "20/02/2001", "Male", "Str. Scolii Nr. 4 Bl. 2 Sc. 1 Ap. 3 Iasi Romania", "john.doe@gmail.com", "+40756221345");
        }
    }
}
