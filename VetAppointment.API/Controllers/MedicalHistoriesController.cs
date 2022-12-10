using Microsoft.AspNetCore.Mvc;
using VetAppointment.API.Dtos;
using VetAppointment.API.Dtos.Create;
using VetAppointment.Application;
using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.API.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class MedicalHistoriesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public MedicalHistoriesController(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

        [HttpGet]
        public IActionResult Get()
        {
            var medicalHistories = unitOfWork.MedicalHistoryRepository
                .All()
                .Select( history => new MedicalHistoryDto { Id = history.Id, ClinicId = history.ClinicId } );

            return Ok(medicalHistories);
        }

        [HttpPost("{medicalHistoryId:Guid}/appointment")]
        public IActionResult Post(Guid medicalHistoryId, [FromBody] CreateAppointmentDto appointmentDto)
        {
            var medicalHistory = unitOfWork.MedicalHistoryRepository.Get(medicalHistoryId);
            if (medicalHistory == null)
            {
                return NotFound();
            }

            var pet = unitOfWork.PetRepository.Get(appointmentDto.PetId);
            if (pet == null)
            {
                return NotFound();
            }

            var vet = unitOfWork.VetRepository.Get(appointmentDto.VetId);
            if (vet == null)
            {
                return NotFound();
            }

            var appointment = Appointment.SettleAppointment(
                vet, 
                pet, 
                appointmentDto.ScheduledDate, 
                appointmentDto.EstimatedDurationInMinutes
            );
            if (appointment.IsFailure)
            {
                return BadRequest(appointment.Error);
            }

            var result = medicalHistory.RegisterAppointmentToHistory(appointment.Entity);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            unitOfWork.AppointmentRepository.Add(appointment.Entity);
            unitOfWork.SaveChanges();

            var fullAppointment = new AppointmentDto
            {
                Id = appointment.Entity.Id,
                ScheduledDate = appointment.Entity.ScheduledDate.ToString(),
                EstimatedDurationInMinutes = appointment.Entity.EstimatedDurationInMinutes,
                VetId = appointment.Entity.VetId,
                PetId = appointment.Entity.PetId,
                TreatmentId = appointment.Entity.TreatmentId,
                MedicalHistoryId = appointment.Entity.MedicalHistoryId
            };

            return Created(nameof(Post), fullAppointment);
        }
    }
}
