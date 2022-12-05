using Microsoft.AspNetCore.Mvc;
using VetAppointment.API.Dtos;
using VetAppointment.API.Dtos.Create;
using VetAppointment.Application;
using VetAppointment.Domain;

namespace VetAppointment.API.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class MedicalHistoriesController : ControllerBase
    {
// UnitOfWork
        private readonly IRepository<MedicalHistory> medicalHistoryRepository;
        private readonly IRepository<Pet> petRepository;
        private readonly IRepository<Vet> vetRepository;
        private readonly IRepository<Appointment> appointmentRepository;

        public MedicalHistoriesController(IRepository<MedicalHistory> medicalHistoryRepository,
            IRepository<Pet> petRepository, IRepository<Vet> vetRepository, IRepository<Appointment> appointmentRepository)
        {
            this.medicalHistoryRepository = medicalHistoryRepository;
            this.petRepository = petRepository;
            this.vetRepository = vetRepository;
            this.appointmentRepository = appointmentRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var medicalHistories = medicalHistoryRepository.All().Select(history => new MedicalHistoryDto
            {
                Id = history.Id
            });
            
            return Ok(medicalHistories);
        }

        [HttpPost("{medicalHistoryId:Guid}/appointment")]
        public IActionResult Post(Guid medicalHistoryId, [FromBody] AppointmentDto appointmentDto)
        {
            var medicalHistory = medicalHistoryRepository.Get(medicalHistoryId);
            if (medicalHistory == null)
            {
                return NotFound();
            }

            var pet = petRepository.Get(appointmentDto.PetId);
            if (pet == null)
            {
                return NotFound();
            }

            var vet = vetRepository.Get(appointmentDto.VetId);
            if (vet == null)
            {
                return NotFound();
            }

            var appointment = Appointment.SettleAppointment(vet, pet, appointmentDto.ScheduledDate, appointmentDto.EstimatedDurationInMinutes);
            if (appointment.IsFailure)
            {
                return BadRequest(appointment.Error);
            }

            var result = medicalHistory.RegisterAppointmentToHistory(appointment.Entity);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            appointmentRepository.Add(appointment.Entity);
            appointmentRepository.SaveChanges();

            return Ok();
        }

        //[HttpPost("{vetClinicId:guid}/appointment")]
        //public IActionResult RegisterAppointment(Guid historyId, [FromBody] AppointmentDto appDto)
        //{
        //    MedicalHistory medHistory = medicalHistoryRepository.Get(historyId);
        //    if (medHistory == null)
        //    {
        //        return NotFound();
        //    }
        //    var pet = petRepository.Get(appDto.PetId);
        //    if (pet == null)
        //    {
        //        return NotFound();
        //    }
        //    var vet = vetRepository.Get(appDto.VetId);
        //    if (vet == null)
        //    {
        //        return NotFound();
        //    }

        //    var appointment = Appointment.SettleAppointment(vet, pet, appDto.ScheduledDate, appDto.EstimatedDurationInMinutes);
        //    if (appointment.IsFailure)
        //    {
        //        return BadRequest();
        //    }

        //    var result = medHistory.RegisterAppointmentToHistory(appointment.Entity);
        //    if (result.IsFailure)
        //    {
        //        return BadRequest(result.Error);
        //    }

        //    medicalHistoryRepository.Update(medHistory);
        //    appointmentRepository.Add(appointment.Entity);
        //    appointmentRepository.SaveChanges();

        //    return NoContent();
        //}
    }
}
