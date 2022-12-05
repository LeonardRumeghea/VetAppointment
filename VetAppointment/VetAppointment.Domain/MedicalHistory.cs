using VetAppointment.Domain.Helpers;

namespace VetAppointment.Domain
{
    #nullable disable
    public class MedicalHistory
    {
        public Guid Id { get; private set; }
        public List<Appointment> Appointments { get; private set; }
        public Guid ClinicId { get; private set; }

        public static Result<MedicalHistory> Create()
        {
            var medicalHistory = new MedicalHistory
            {
                Appointments = new List<Appointment>()
            };

            return Result<MedicalHistory>.Success(medicalHistory);
        }

        
        public Result RegisterAppointmentToHistory(Appointment appointment)
        {
            if (appointment == null)
            {
                return Result.Failure("Appointment does not exist");
            }

            appointment.AttachAppointmentToMedicalHistory(this);
            Appointments.Add(appointment);

            return Result.Success();
        }
    }
}
