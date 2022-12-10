namespace VetAppointment.Business.Test
{
    public class VetTets
    {
        [Fact]
        public void When_CreateVetWithValidData_Then_ShouldReturnSuccess()
        {
            // Arrange
            var vet = CreateSUT();

            //Act
            var result = Vet.Create(vet.Item1, vet.Item2, vet.Item3, vet.Item4, vet.Item5, vet.Item6, vet.Item7);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
            result.Entity.Id.Should().NotBeEmpty();
            result.Entity.Name.Should().Be(vet.Item1);
            result.Entity.Surname.Should().Be(vet.Item2);
            result.Entity.Birthdate.Should().Be(DateTime.Parse(vet.Item3));
            result.Entity.Gender.Should().Be(Enum.Parse<PersonGender>(vet.Item4));
            result.Entity.Phone.Should().Be(vet.Item6);
            result.Entity.Email.Should().Be(vet.Item5);
        }
 

        [Fact]
        public void When_CreateVetWithInvalidPhoneNumber_Then_ShouldReturnFailure()
        {
            // Arrange
            var vet = CreateSUT();
            var invalidPhoneNumber = string.Empty;

            //Act
            var result = Vet.Create(vet.Item1, vet.Item2, vet.Item3, vet.Item4, vet.Item5, invalidPhoneNumber, vet.Item7);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be($"Phone number {invalidPhoneNumber} is not valid");
        }

        [Fact]
        public void When_CreateVetWithInvalidEmail_Then_ShouldReturnFailure()
        {
            // Arrange
            var vet = CreateSUT();
            var invalidEmail = string.Empty;

            //Act
            var result = Vet.Create(vet.Item1, vet.Item2, vet.Item3, vet.Item4, invalidEmail, vet.Item6, vet.Item7);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be($"Email {invalidEmail} is not valid");
        }

        [Fact]
        public void When_CreateVetWithInvalidGender_Then_ShouldReturnFailure()
        {
            // Arrange
            var vet = CreateSUT();
            var invalidGender = "invalid";

            //Act
            var result = Vet.Create(vet.Item1, vet.Item2, vet.Item3, invalidGender, vet.Item5, vet.Item6, vet.Item7);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be($"The provided gender {invalidGender} is not one from the possible races: Male, Female, Other");
        }
        [Fact]
        public void When_CreateVetWithInvalidBirthdate_Then_ShouldReturnFailure()
        {
            // Arrange
            var vet = CreateSUT();
            var invalidBirthdate = string.Empty;

            //Act
            var result = Vet.Create(vet.Item1, vet.Item2, invalidBirthdate, vet.Item4, vet.Item5, vet.Item6, vet.Item7);

            //Assert
            result.IsFailure.Should().BeTrue();
        }
        [Fact]
        public void When_CreateVetWithInvalidSpecialisation_Then_ShouldReturnFailure()
        {
            // Arrange
            var vet = CreateSUT();
            var invalidSpecialization = "Bucatar";

            //Act
            var result = Vet.Create(vet.Item1, vet.Item2, vet.Item3, vet.Item4, vet.Item5, vet.Item6, invalidSpecialization);

            //Assert
            result.IsFailure.Should().BeTrue();
        }
        [Fact]
        public void When_RegisterVetToClinic_Then_IdShouldBeNotNull()
        {
            // Arrange
            var sut = CreateSUT();
            var vet = Vet.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, sut.Item6, sut.Item7).Entity;
            var sutClinic = CreateSUTForClinic();
            var vetClinic = VetClinic.Create(sutClinic.Item1, sutClinic.Item2, sutClinic.Item3, sutClinic.Item4, sutClinic.Item5).Entity;

            //Act
            vet.RegisterVetToClinic(vetClinic);
            //Assert
            vet.ClinicId.Should().Be(vetClinic.Id);
        }
        [Fact]
        public void When_When_UpdateVet_Then_ShouldReturnSuccess()
        {
            // Arrange
            var sut = CreateSUT();
            var vet = Vet.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, sut.Item6, sut.Item7).Entity;

            // Act
            var result = vet.Update(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, sut.Item6, sut.Item7);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_When_UpdateVetWithInvalidEmail_Then_ShouldReturnFeilure()
        {
            // Arrange
            var sut = CreateSUT();
            var vet = Vet.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, sut.Item6, sut.Item7).Entity;
            var invalidEmail = "ad";

            // Act
            var result = vet.Update(sut.Item1, sut.Item2, sut.Item3, sut.Item4, invalidEmail, sut.Item6, sut.Item7);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
        [Fact]
        public void When_When_UpdateVetWithInvalidPhone_Then_ShouldReturnFeilure()
        {
            // Arrange
            var sut = CreateSUT();
            var vet = Vet.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, sut.Item6, sut.Item7).Entity;
            var invalidPhone = "2121212";

            // Act
            var result = vet.Update(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, invalidPhone, sut.Item7);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
        [Fact]
        public void When_When_UpdateVetWithInvalidBirthdate_Then_ShouldReturnFeilure()
        {
            // Arrange
            var sut = CreateSUT();
            var vet = Vet.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, sut.Item6, sut.Item7).Entity;
            var invalidBirthdate = "20.11.12326";

            // Act
            var result = vet.Update(sut.Item1, sut.Item2, invalidBirthdate, sut.Item4, sut.Item5, sut.Item6, sut.Item7);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
        [Fact]
        public void When_When_UpdateVetWithInvalidGender_Then_ShouldReturnFeilure()
        {
            // Arrange
            var sut = CreateSUT();
            var vet = Vet.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, sut.Item6, sut.Item7).Entity;
            var invalidGender = "elicopterBimoto";

            // Act
            var result = vet.Update(sut.Item1, sut.Item2, sut.Item3, invalidGender, sut.Item5, sut.Item6, sut.Item7);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
        [Fact]
        public void When_When_UpdateVetWithInvalidSpecialisation_Then_ShouldReturnFeilure()
        {
            // Arrange
            var sut = CreateSUT();
            var vet = Vet.Create(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, sut.Item6, sut.Item7).Entity;
            var invalidSpecialisation = "Dermatolog";

            // Act
            var result = vet.Update(sut.Item1, sut.Item2, sut.Item3, sut.Item4, sut.Item5, sut.Item6, invalidSpecialisation);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
        //string name, string surname, string birthdate, string gender, string email, string phone
        private static Tuple<string, string, string, string, string, string, string> CreateSUT()
        {
            return new Tuple<string, string, string, string, string, string, string>(
                "John", "Doe", "01/01/1990", "Male", "john.doe@gmail.com", "+40123456789", "PawSurgeon"
                );
        }
        private Tuple<string, string, int, string, string> CreateSUTForClinic()
        {
            return new Tuple<string, string, int, string, string>("Vet Clinic", "Address", 10, "email@gmail.com", "+40123456789");
        }
    }
}
