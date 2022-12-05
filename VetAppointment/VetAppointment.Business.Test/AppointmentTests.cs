//namespace VetAppointment.Business.Test
//{
//    public class AppointmentTests
//    {
//        [Fact]
//        public void When_CreateAppointmentWithValidData_Then_ShouldReturnSuccess()
//        {
//            {
//                // Arrange
//                var appointment = CreateSUT();
                
//                //Act
//                var result = Appointment.SettleAppointment(appointment.Item1, appointment.Item2, appointment.Item3, appointment.Item4);
                
//                //Assert
//                result.IsSuccess.Should().BeTrue();
//                result.Entity.Should().NotBeNull();
//                result.Entity.Id.Should().NotBeEmpty();
//                result.Entity.VetId.Should().Be(appointment.Item1);
//                result.Entity.PetId.Should().Be(appointment.Item2);
//                result.Entity.ScheduledDate.Should().Be(appointment.Item3);
//                result.Entity.EstimatedDurationInMinutes.Should().Be(appointment.Item4);
//            }
//        }

//        [Fact]
//        public void When_CreateAppoimentWithInvalidVetId_The_ShouldReturnFailure()
//        {
//            {
//                // Arrange
//                var appointment = CreateSUT();
//                var invalidVetId = Guid.Empty;

//                //Act
//                var result = Appointment.SettleAppointment(invalidVetId, appointment.Item2, appointment.Item3, appointment.Item4);

//                //Assert
//                result.IsFailure.Should().BeTrue();
//                result.Error.Should().Be($"Vet id cannot be empty");
//            }
//        }

//        [Fact]
//        public void When_CreateAppoimentWithInvalidPetId_Then_ShouldReturnFailure()
//        {
//            // Arrange
//            var appointment = CreateSUT();
//            var invalidPetId = Guid.Empty;

//            // Act
//            var result = Appointment.SettleAppointment(appointment.Item1, invalidPetId, appointment.Item3, appointment.Item4);

//            // Assert
//            result.IsFailure.Should().BeTrue();
//            result.Error.Should().Be($"Pet id cannot be empty");
//        }

//        [Fact]
//        public void When_CreateAppoimentWithInvalidScheduledDate_Then_ShouldReturnFailure()
//        {
//            // Arrange
//            var appoinment = CreateSUT();
//            var invalidScheduledDate = DateTime.MinValue;

//            // Act
//            var result = Appointment.SettleAppointment(appoinment.Item1, appoinment.Item2, invalidScheduledDate, appoinment.Item4);

//            // Assert
//            result.IsFailure.Should().BeTrue();
//            result.Error.Should().Be("Date cannot be in the past");
//        }

//        [Fact]
//        public void When_CreateAppointmentWithInvalidDurationTime_Then_ShouldReturnFailure()
//        {
//            // Arrange
//            var appointment = CreateSUT();
//            var invalidDurationTime = -1;

//            // Act
//            var result = Appointment.SettleAppointment(appointment.Item1, appointment.Item2, appointment.Item3, invalidDurationTime);

//            // Assert
//            result.IsFailure.Should().BeTrue();
//            result.Error.Should().Be($"Duration cannot be less than 0");
//        }

//        public static Tuple<Guid, Guid, DateTime, int> CreateSUT()
//        {
//            return new Tuple<Guid, Guid, DateTime, int>(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.AddDays(1), 30);
//        }
//    }
//}
