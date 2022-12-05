using VetAppointment.Domain.Helpers;

#nullable disable
namespace VetAppointment.Domain
{
    public class Appointment
    {
        public Guid Id { get; private set; }
        public Guid VetId { get; private set; }
        public Guid PetId { get; private set; }
        public DateTime ScheduledDate { get; private set; }
        public int EstimatedDurationInMinutes { get; private set; }
        public Guid TreatmentId { get; private set; }
        public Guid MedicalHistoryId { get; private set; }

        public static Result<Appointment> SettleAppointment(Vet vet, Pet pet, DateTime date, int duration)
        {

            Guid vetId = vet.Id;
            Guid petId = pet.Id;

            if (duration <= 0)
            {
                return Result<Appointment>.Failure($"Duration cannot be less than 0");
            }

            if (date < DateTime.Now)
            {
                return Result<Appointment>.Failure($"Date cannot be in the past");
            }

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                VetId = vetId,
                PetId = petId,
                ScheduledDate = date,
                EstimatedDurationInMinutes = duration
            };

            return Result<Appointment>.Success(appointment);
        }

        public void AttachTreatmentToAppointment(Treatment treatment)
        {
            TreatmentId = treatment.Id;
        }

        public void AttachAppointmentToMedicalHistory(MedicalHistory history)
        {
            MedicalHistoryId = history.Id;
        }
    
        public Result Update(Guid vetId, Guid petId, DateTime scheduledDate, int estimatedDurationInMinutes,
            Guid treatmentId, Guid medicalHistoryId)
        {
            if (estimatedDurationInMinutes <= 0)
            {
                return Result.Failure($"Duration cannot be less than 0");
            }

            if (scheduledDate < DateTime.Now)
            {
                return Result.Failure($"Date cannot be in the past");
            }

            VetId = vetId;
            PetId = petId;
            ScheduledDate = scheduledDate;
            EstimatedDurationInMinutes = estimatedDurationInMinutes;
            TreatmentId = treatmentId;
            MedicalHistoryId = medicalHistoryId;

            return Result.Success();
        }
    }
}
