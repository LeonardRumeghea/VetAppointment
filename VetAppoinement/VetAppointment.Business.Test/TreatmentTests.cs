using VetAppointment.Domain.Helpers;

namespace VetAppointment.Business.Test
{
    public class TreatmentTests
    {
        [Fact]
        public void When_CreateTreatment_Then_ShouldReturnTreatment()
        {
            // Arrange
            var sut = CreateSUT();

            // Act
            var result = Treatment.Create(sut.Item1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
            result.Entity.Description.Should().Be(sut.Item1);
        }


        [Fact]
        public void When_CreateTreatmentWithInvalidDescription_Then_ShouldReturnFailure()
        {
            // Arrange

            // Act
            var result = Treatment.Create(null);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_CalculateTreatmentCost_Then_ShouldReturnSumOfDrugs()
        {
            // Arrange
            var sut = CreateSUT();
            var treatment = Treatment.Create(sut.Item1).Entity;
            var sutDrug = CreateSUTForPrescribedDrug();
            var prescribedDrug = PrescribedDrug.Create(sutDrug.Item1, sutDrug.Item2).Entity;
            var drugs = new List<PrescribedDrug>
            {
                prescribedDrug
            };
            treatment.AppendDrugsToTreatment(drugs);

            // Act
            var result = treatment.CalculatePriceOfTreatment();

            // Assert
            result.Should().Be(prescribedDrug.TotalCost);
        }

        [Fact]
        public void When_AppendNullDrugsToTreatment_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var treatment = Treatment.Create(sut.Item1).Entity;
            var drugs = new List<PrescribedDrug>();

            // Act
            var result = treatment.AppendDrugsToTreatment(drugs);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_RemoveDrugFromTreatment_Then_ShouldReturnSuccess()
        {
            // Arrange
            var sut = CreateSUT();
            var treatment = Treatment.Create(sut.Item1).Entity;
            var sutDrug = CreateSUTForPrescribedDrug();
            var prescribedDrug = PrescribedDrug.Create(sutDrug.Item1, sutDrug.Item2).Entity;
            var drugs = new List<PrescribedDrug>
            {
                prescribedDrug
            };
            treatment.AppendDrugsToTreatment(drugs);

            // Act
            var result = treatment.RemoveDrugFromTreatment(prescribedDrug);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void When_RemoveInvalidDrugFromTreatment_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var treatment = Treatment.Create(sut.Item1).Entity;
            var sutDrug = CreateSUTForPrescribedDrug();
            var prescribedDrug1 = PrescribedDrug.Create(sutDrug.Item1, sutDrug.Item2).Entity;
            var prescribedDrug2 = PrescribedDrug.Create(30, sutDrug.Item2).Entity;
            var drugs = new List<PrescribedDrug>
            {
                prescribedDrug1
            };
            treatment.AppendDrugsToTreatment(drugs);

            // Act
            var result = treatment.RemoveDrugFromTreatment(prescribedDrug2);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_UpdateTreatmentDescription_Then_ShouldReturnSuccess()
        {
            // Arrange
            var sut = CreateSUT();
            var treatment = Treatment.Create(sut.Item1).Entity;

            // Act
            var result = treatment.UpdateDescription("newDescription");

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void When_UpdateDescriptionWithNull_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var treatment = Treatment.Create(sut.Item1).Entity;

            // Act
            var result = treatment.UpdateDescription(null);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_UpdateDescriptionWithOldDescription_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var treatment = Treatment.Create(sut.Item1).Entity;

            // Act
            var result = treatment.UpdateDescription(treatment.Description);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        private static Tuple<string> CreateSUT()
        {
            return new Tuple<string>("paraacetamol500mg");
        }
        private static Tuple<double, Drug> CreateSUTForPrescribedDrug()
        {
            var sutDrug = CreateSUTForDrug();
            return new Tuple<double, Drug>(4, Drug.Create(sutDrug.Item1, sutDrug.Item2, sutDrug.Item3).Entity);
        }
        private static Tuple<string, double, double> CreateSUTForDrug()
        {
            return new Tuple<string, double, double>("Paracetamol", 11, 95.84);
        }
    }
}