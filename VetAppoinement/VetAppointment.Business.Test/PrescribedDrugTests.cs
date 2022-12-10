namespace VetAppointment.Business.Test
{
    public class PrescribedDrugTests
    {
        [Fact]
        public void When_CreatePrescribedDrug_Then_ShouldReturnPrescribedDrug()
        {
            // Arrange
            var sut = CreateSUT();

            // Act
            var result = PrescribedDrug.Create(sut.Item1, sut.Item2);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
        }
        [Fact]
        public void When_CreatePrescribedDrugWithInvalidNegativQuantity_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            double invalidQuantity = -5;
            // Act
            var result = PrescribedDrug.Create(invalidQuantity, sut.Item2);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
        [Fact]
        public void When_CreatePrescribedDrugWithInvalidDrugStock_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var sutDrug = CreateSUTForDrug();
            var drug = Drug.Create(sutDrug.Item1,2, sutDrug.Item3).Entity;
            // Act
            var result = PrescribedDrug.Create(sut.Item1, drug);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
        [Fact]
        public void When_UpdatePrescribedDrug_Then_ShouldReturnSuccess()
        {
            // Arrange
            var sut = CreateSUT();
            var prescribedDrug = PrescribedDrug.Create(sut.Item1, sut.Item2).Entity;

            // Act
            var result = prescribedDrug.Update(sut.Item1, sut.Item2);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_UpdatePrescribedDrugWithInvalidQuantity_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var prescribedDrug = PrescribedDrug.Create(sut.Item1, sut.Item2).Entity;
            double invalidQuantity = -3;
            // Act
            var result = prescribedDrug.Update(invalidQuantity, sut.Item2);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
        [Fact]
        public void When_UpdatePrescribedDrugWithInvalidQuantityAtDrug_Then_ShouldReturnFailure()
        {
            // Arrange
            var sutDrug = CreateSUTForDrug();
            var drug = Drug.Create(sutDrug.Item1, 20, sutDrug.Item3).Entity;
            var prescribedDrug = PrescribedDrug.Create(15, drug).Entity;

            //Act

            var result = prescribedDrug.Update(50,drug);
            // Assert
            result.IsFailure.Should().BeTrue();
        }

        private Tuple<double, Drug> CreateSUT()
        {
            var sutDrug = CreateSUTForDrug();
            return new Tuple<double, Drug>(4, Drug.Create(sutDrug.Item1, sutDrug.Item2, sutDrug.Item3).Entity);
        }
        private Tuple<string, double, double> CreateSUTForDrug()
        {
            return new Tuple<string, double, double>("Paracetamol", 11, 95.84);
        }

    }
}
