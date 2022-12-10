namespace VetAppointment.Business.Test
{
    public class MedicalHistoryTests
    {
        [Fact]
        public void When_CreateMedicalHistory_Then_ShouldReturnMedicalHistory()
        {
            // Arrange

            // Act
            var result = MedicalHistory.Create();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Entity.Should().NotBeNull();
        }

        [Fact]
        public void When_RegisterAppointmentToMedicalHistory_Then_ShouldReturnSuccess()
        {
            // Arrange
            var medicalHistory = MedicalHistory.Create().Entity;
            var sutAppointment = CreateSUTForAppointment();
            var appointment = Appointment.SettleAppointment(sutAppointment.Item1, sutAppointment.Item2, sutAppointment.Item3.ToString(), sutAppointment.Item4).Entity;

            // Act
            var result = medicalHistory.RegisterAppointmentToHistory(appointment);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void When_RegisterNullAppointmentToMedicalHistory_Then_ShouldReturnFailure()
        {
            // Arrange
            var medicalHistory = MedicalHistory.Create().Entity;

            // Act
            var result = medicalHistory.RegisterAppointmentToHistory(null);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void When_AtachToClinic_Then_MedicalHistoryIsAttached()
        {
            {
                // Arrange
                var medicalHistory = MedicalHistory.Create().Entity;
                var sutClinic = CreateSUTForClinic();
                var clinic = VetClinic.Create(sutClinic.Item1, sutClinic.Item2, sutClinic.Item3, sutClinic.Item4, sutClinic.Item5).Entity;

                // Act
                medicalHistory.AtachToClinic(clinic.Id);

                // Assert
                medicalHistory.ClinicId.Should().Be(clinic.Id);
            }
        }

        [Fact]
        public void When_RemoveAppointmentFromHistoryWithNullAppointment_Then_ShouldReturnFailure()
        {
            {
                // Arrange
                var medicalHistory = MedicalHistory.Create().Entity;

                // Act
                var result = medicalHistory.RemoveAppointmentFromHistory(null);

                // Assert
                result.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void When_RemoveAppointmentFromHistoryWithValidAppointment_Then_ShouldReturnSucces()
        {
            {
                // Arrange
                var medicalHistory = MedicalHistory.Create().Entity;
                var appointment = CreateAppSUT();
                var app = Appointment.SettleAppointment(appointment.Item1, appointment.Item2, appointment.Item3.ToString(), appointment.Item4);


                // Act
                var result = medicalHistory.RemoveAppointmentFromHistory(app.Entity);

                // Assert
                result.IsSuccess.Should().BeTrue();
            }
        }

        public static Tuple<Vet, Pet, DateTime, int> CreateSUTForAppointment()
        {
            return new Tuple<Vet, Pet, DateTime, int>(
                Vet.Create("Vasile", "Ion", "20/02/2001", "Male", "m2@gmail.com", "+40123456789", VetSpecialisation.DentalCaretaker.ToString()).Entity,
                Pet.Create("Bob", "20/02/2001", "Dog", "Male").Entity,
                DateTime.Now.AddDays(1), 30);
        }

        private Tuple<string, string, int, string, string> CreateSUTForClinic()
        {
            return new Tuple<string, string, int, string, string>("Vet Clinic", "Address", 10, "email@gmail.com", "+40123456789");
        }

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

        public static Tuple<Vet, Pet, DateTime, int> CreateAppSUT()
        {
            var sutVet = CreateSUTForVet();
            var sutPet = CreateSUTForPet();
            return new Tuple<Vet, Pet, DateTime, int>(Vet.Create(sutVet.Item1, sutVet.Item2, sutVet.Item3, sutVet.Item4, sutVet.Item5, sutVet.Item6, sutVet.Item7).Entity, Pet.Create(sutPet.Item1, sutPet.Item2, sutPet.Item3, sutPet.Item4).Entity, DateTime.Now.AddDays(1), 30);
        }
    }
}
