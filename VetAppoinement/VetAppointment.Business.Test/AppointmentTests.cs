namespace VetAppointment.Business.Test
{
    public class AppointmentTests
    {
        [Fact]
        public void When_CreateAppointmentWithValidData_Then_ShouldReturnSuccess()
        {
            {
                // Arrange
                var appointment = CreateSUT();

                //Act
                var result = Appointment.SettleAppointment(appointment.Item1, appointment.Item2, appointment.Item3.ToString(), appointment.Item4);

                //Assert
                result.IsSuccess.Should().BeTrue();
                result.Entity.Should().NotBeNull();
                result.Entity.Id.Should().NotBeEmpty();
                result.Entity.VetId.Should().Be(appointment.Item1.Id);
                result.Entity.PetId.Should().Be(appointment.Item2.Id);
                result.Entity.ScheduledDate.Should().Be(DateTime.Parse(appointment.Item3.ToString()));
                result.Entity.EstimatedDurationInMinutes.Should().Be(appointment.Item4);
            }
        }

        [Fact]
        public void When_CreateAppoimentWithInvalidScheduledDate_Then_ShouldReturnFailure()
        {
            // Arrange
            var appoinment = CreateSUT();
            var invalidScheduledDate = DateTime.MinValue;

            // Act
            var result = Appointment.SettleAppointment(appoinment.Item1, appoinment.Item2, invalidScheduledDate.ToString(), appoinment.Item4);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Date cannot be in the past");
        }

        [Fact]
        public void When_CreateAppointmentWithInvalidDurationTime_Then_ShouldReturnFailure()
        {
            // Arrange
            var appointment = CreateSUT();
            var invalidDurationTime = -1;

            // Act
            var result = Appointment.SettleAppointment(appointment.Item1, appointment.Item2, appointment.Item3.ToString(), invalidDurationTime);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be($"Duration cannot be less than 0");
        }

        [Fact]
        public void When_AttachTreatmentToAppointment_Then_ShouldHaveTreatment()
        {
            // Arrange
            var sut = CreateSUT();
            var appointment = Appointment.SettleAppointment(sut.Item1, sut.Item2, sut.Item3.ToString(), sut.Item4).Entity;
            var sutTreatment = CreateSUTForTreatment();
            var treatment = Treatment.Create(sutTreatment.Item1).Entity;

            // Act
            appointment.AttachTreatmentToAppointment(treatment);

            // Assert
            appointment.TreatmentId.Should().Be(treatment.Id);
        }

        [Fact]
        public void When_UpdateAppointment_Then_ShouldReturnSuccess()
        {
            // Arrange
            var sut = CreateSUT();
            var appointment = Appointment.SettleAppointment(sut.Item1, sut.Item2, sut.Item3.ToString(), sut.Item4).Entity;

            // Act
            var result = appointment.Update(appointment.VetId, appointment.PetId, appointment.ScheduledDate.ToString(), appointment.EstimatedDurationInMinutes, appointment.TreatmentId, appointment.MedicalHistoryId);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void When_UpdateAppointmentWithInvalidDuration_Then_ShouldReturnFailure()
        {
            // Arrange
            var sut = CreateSUT();
            var appointment = Appointment.SettleAppointment(sut.Item1, sut.Item2, sut.Item3.ToString(), sut.Item4).Entity;

            // Act
            var result = appointment.Update(appointment.VetId, appointment.PetId, appointment.ScheduledDate.ToString(), -5, appointment.TreatmentId, appointment.MedicalHistoryId);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]//nu stiu cum sa pun ziua de ieri todo
        public void When_UpdateAppointmentWithInvalidDate_Then_ShouldReturnSuccess()
        {
            // Arrange
            var sut = CreateSUT();
            var appointment = Appointment.SettleAppointment(sut.Item1, sut.Item2, sut.Item3.ToString(), sut.Item4).Entity;

            // Act
            var result = appointment.Update(appointment.VetId, appointment.PetId, DateTime.Now.AddYears(-1).ToString(), appointment.EstimatedDurationInMinutes, appointment.TreatmentId, appointment.MedicalHistoryId);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_AttachAppointmentToMedicalHistory_Then_ShouldAttach()
        {
            var appointmentSUT = CreateSUT();
            var app = Appointment.SettleAppointment(appointmentSUT.Item1, appointmentSUT.Item2, 
                appointmentSUT.Item3.ToString(), appointmentSUT.Item4);

            var medicalHistory = MedicalHistory.Create().Entity;

            app.Entity.AttachAppointmentToMedicalHistory(medicalHistory);

            app.Entity.MedicalHistoryId.Should().Be(medicalHistory.Id);
        }

        //public static Tuple<Guid, Guid, DateTime, int> CreateSUT()
        //{
        //    return new Tuple<Guid, Guid, DateTime, int>(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.AddDays(1), 30);
        //}
        private static Tuple<string, string, string, string, string, string, string> CreateSUTForVet()
        {
            return new Tuple<string, string, string, string, string, string, string>(
                "John", "Doe", "01/01/1990", "Male", "john.doe@gmail.com", "+40123456789", "PawSurgeon"
                );
        }
        private static Tuple<string, string, string, string> CreateSUTForPet()
        {
            return new Tuple<string, string, string, string>("Pisacio", "12/06/2020", "Cat", "Male");
        }
        private static Tuple<string> CreateSUTForTreatment()
        {
            return new Tuple<string>("paraacetamol500mg");
        }
        public static Tuple<Vet, Pet, DateTime, int> CreateSUT()
        {
            var sutVet = CreateSUTForVet();
            var sutPet = CreateSUTForPet();
            return new Tuple<Vet, Pet, DateTime, int>(
                Vet.Create(sutVet.Item1, sutVet.Item2, sutVet.Item3, sutVet.Item4, sutVet.Item5, sutVet.Item6, sutVet.Item7).Entity, 
                Pet.Create(sutPet.Item1, sutPet.Item2, sutPet.Item3, sutPet.Item4).Entity, 
                DateTime.Now.AddDays(1), 
                30
            );
        }
    }
}