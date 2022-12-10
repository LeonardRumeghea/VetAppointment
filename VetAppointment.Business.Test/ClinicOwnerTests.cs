namespace VetAppointment.Business.Test
{
    public class ClinicOwnerTests
    {
        [Fact]
        public void When_CreateClinicOwner_Then_ShouldReturnClinicOwner()
        {
            // Arrange
            var sut = CreateSUT();

            // Act
            var result = ClinicOwner.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, sut.Item6);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
        }
        [Fact]
        public void When_CreatePetOwnerWithInvalidBirthdate_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var date = "invaliddate";

            // Act
            var result = ClinicOwner.Create(sut.Item1, sut.Item2, date, sut.Item4, sut.Item5, sut.Item6);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_CreateClinicOwnerWithInvalidGender_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var gender = "invalid";

            // Act
            var result = ClinicOwner.Create(sut.Item1, sut.Item2, sut.Item3, gender, sut.Item5, sut.Item6);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_CreateClinicOwnerWithInvalidEmail_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var mail = "invalidmail";

            // Act
            var result = ClinicOwner.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4, mail, sut.Item6);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_CreateClinicOwnerWithInvalidPhoneNumber_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            string phoneNumber = null;

            // Act
            var result = ClinicOwner.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, phoneNumber);

            // Assert
            result.IsFailure.Should().BeTrue();
        }        

        private Tuple<string, string, string, string, string, string> CreateSUT()
        {
            return new Tuple<string,string, string, string, string, string>(
                "John", "Doe", "12/10/2001", "Male", "john.doe@gmail.com", "+40756221345");
        }
    }
}
