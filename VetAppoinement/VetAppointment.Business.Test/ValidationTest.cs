namespace VetAppointment.Business.Test
{
    public class ValidationTest
    {
        [Fact]
        public void When_EmailIsValid_Then_ShouldReturnTrue()
        {
            // Arrange
            var email = "leonard9692@gmai.com";

            // Act
            var result = Validations.IsValidEmail(email);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void When_EmailIsNotValid_Then_ShouldReturnFalse()
        {
            // Arrange
            var email = "leonard9692@gmai.com.";

            // Act
            var result = Validations.IsValidEmail(email);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void When_PhoneNumberIsValid_Then_ShouldReturnTrue()
        {
            // Arrange
            var phoneNumber = "+40754949140";

            // Act
            var result = Validations.IsValidPhoneNumber(phoneNumber);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void When_PhoneNumberIsNotValid_Then_ShouldReturnFalse()
        {
            // Arrange
            var phoneNumber = "+4075494914";

            // Act
            var result = Validations.IsValidPhoneNumber(phoneNumber);

            // Assert
            result.Should().BeFalse();
        }
    }
}