namespace VetAppointment.Business.Test
{
    public class DrugTests
    {
        [Fact]
        public void When_CreateDrug_Then_ShouldReturnDrug()
        {
            // Arrange
            var sut = CreateSUT();

            // Act
            var result = Drug.Create(sut.Item1, sut.Item2, sut.Item3);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
            result.Entity.Name.Should().Be(sut.Item1);
        }
        [Fact]
        public void When_CreateDrugWithInvalidQuantity_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            double invalidQuantity = -5;
            // Act
            var result = Drug.Create(sut.Item1, invalidQuantity, sut.Item3);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
        [Fact]
        public void When_CreateDrugWithInvalidPrice_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            double invalidPrice = -5;
            // Act
            var result = Drug.Create(sut.Item1, sut.Item2, invalidPrice);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateDrug_Then_ShouldReturnSuccess()
        {
            // Arrange
            var sut = CreateSUT();
            var drug = Drug.Create(sut.Item1, sut.Item2, sut.Item3).Entity;

            // Act
            var result = drug.Update(drug.Name, drug.Quantity, drug.UnitPrice);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void When_UpdateDrugWithInvalidName_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var drug = Drug.Create(sut.Item1, sut.Item2, sut.Item3).Entity;
            var invalidName = "";

            // Act
            var result = drug.Update(invalidName, drug.Quantity, drug.UnitPrice);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateDrugWithQuantity_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var drug = Drug.Create(sut.Item1, sut.Item2, sut.Item3).Entity;
            double invalidQuantity = -9;

            // Act
            var result = drug.Update(sut.Item1, invalidQuantity, sut.Item3);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateDrugWithPrice_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var drug = Drug.Create(sut.Item1, sut.Item2, sut.Item3).Entity;
            double invalidPrice = -9;

            // Act
            var result = drug.Update(sut.Item1, sut.Item2, invalidPrice);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
        private Tuple<string, double, double> CreateSUT()
        {
            return new Tuple<string, double, double>("Paracetamol", 11, 95.84);
        }
     
    }
}
