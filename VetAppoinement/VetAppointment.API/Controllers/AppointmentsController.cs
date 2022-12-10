using Microsoft.AspNetCore.Mvc;
using VetAppointment.API.Dtos;
using VetAppointment.API.Dtos.Create;
using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.API.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public AppointmentsController(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

        [HttpPost]
        public IActionResult Create([FromBody] CreateAppointmentDto appointmentDto)
        {
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

            var treatement = unitOfWork.TreatmentRepository.Get(appointmentDto.TreatmentId);
            if (treatement == null )
            {
                return NotFound();
            }

            var history = unitOfWork.MedicalHistoryRepository.Get(appointmentDto.MedicalHistoryId);
            if (history == null)
            {
                return NotFound();
            }

            history.RegisterAppointmentToHistory(appointment.Entity);
            unitOfWork.MedicalHistoryRepository.Update(history);
            unitOfWork.SaveChanges();

            appointment.Entity.AttachTreatmentToAppointment(treatement);
            appointment.Entity.AttachAppointmentToMedicalHistory(history);


            if (appointment.IsFailure)
            {
                return BadRequest(appointment.Error);
            }

            unitOfWork.AppointmentRepository.Add(appointment.Entity);
            unitOfWork.SaveChanges();
            var fullAppointment = new AppointmentDto
            {
                Id = appointment.Entity.Id,
                VetId = appointment.Entity.VetId,
                PetId = appointment.Entity.PetId,
                ScheduledDate = appointment.Entity.ScheduledDate.ToString(),
                EstimatedDurationInMinutes = appointment.Entity.EstimatedDurationInMinutes
            };

            return Created(nameof(GetAllAppointments), fullAppointment);
        }

        [HttpGet]
        public IActionResult GetAllAppointments()
        {
            var appointments = unitOfWork.AppointmentRepository.All().Select(appointment => new AppointmentDto()
            {
                Id = appointment.Id,
                VetId = appointment.VetId,
                PetId = appointment.PetId,
                ScheduledDate = appointment.ScheduledDate.ToString(),
                EstimatedDurationInMinutes = appointment.EstimatedDurationInMinutes,
                TreatmentId = appointment.TreatmentId,
                MedicalHistoryId = appointment.MedicalHistoryId
            });
            
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public IActionResult GetAppointmentById(Guid id)
        {
            var appointment = unitOfWork.AppointmentRepository.Get(id);
            if (appointment == null)
            {
                return NotFound();
            }

            var appointmentDto = new AppointmentDto
            {
                Id = appointment.Id,
                VetId = appointment.VetId,
                PetId = appointment.PetId,
                ScheduledDate = appointment.ScheduledDate.ToString(),
                EstimatedDurationInMinutes = appointment.EstimatedDurationInMinutes,
                TreatmentId = appointment.TreatmentId,
                MedicalHistoryId = appointment.MedicalHistoryId
            };

            return Ok(appointmentDto);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAppointment(Guid id)
        {
            var appointment = unitOfWork.AppointmentRepository.Get(id);
            if (appointment == null)
            {
                return NotFound();
            }

            unitOfWork.AppointmentRepository.Delete(appointment);
            unitOfWork.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAppointment(Guid id, [FromBody] AppointmentDto appointmentDto)
        {
            var appointment = unitOfWork.AppointmentRepository.Get(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Update(appointment.VetId, appointmentDto.PetId, appointmentDto.ScheduledDate,
                appointmentDto.EstimatedDurationInMinutes, appointmentDto.TreatmentId, appointmentDto.MedicalHistoryId);

            unitOfWork.AppointmentRepository.Update(appointment);
            unitOfWork.SaveChanges();

            return NoContent();
        }
    }
}
