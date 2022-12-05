using Microsoft.AspNetCore.Mvc;
using VetAppointment.API.Dtos;
using VetAppointment.API.Dtos.Create;
using VetAppointment.Application;
using VetAppointment.Domain;


namespace VetAppointment.API.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IRepository<Appointment> appointmentRepository;
        private readonly IRepository<MedicalHistory> medicalHistoryRepository;
        private readonly IRepository<Treatment> treatmentRepository;
        private readonly IRepository<Pet> petRepository;
        private readonly IRepository<Vet> vetRepository;

        public AppointmentsController(IRepository<Appointment> appointmentRepository, 
            IRepository<Pet> petRepository, IRepository<Vet> vetRepository, IRepository<MedicalHistory> medicalHistoryRepository,
            IRepository<Treatment> treatmentRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.petRepository = petRepository;
            this.vetRepository = vetRepository;
            this.medicalHistoryRepository = medicalHistoryRepository;
            this.treatmentRepository = treatmentRepository;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateAppointmentDto appointmentDto)
        {
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
            var appointment = Appointment.SettleAppointment(
                    vet,
                    pet,
                    appointmentDto.ScheduledDate,
                    appointmentDto.EstimatedDurationInMinutes
                );

            var treatement = treatmentRepository.Get(appointmentDto.TreatmentId);
            if (treatement == null )
            {
                return NotFound();
            }

            var history = medicalHistoryRepository.Get(appointmentDto.MedicalHistoryId);
            if (history == null)
            {
                return NotFound();
            }

            history.RegisterAppointmentToHistory(appointment.Entity);
            medicalHistoryRepository.Update(history);
            medicalHistoryRepository.SaveChanges();

            appointment.Entity.AttachTreatmentToAppointment(treatement);
            appointment.Entity.AttachAppointmentToMedicalHistory(history);


            if (appointment.IsFailure)
            {
                return BadRequest(appointment.Error);
            }
            
            appointmentRepository.Add(appointment.Entity);
            appointmentRepository.SaveChanges();
            var fullAppointment = new AppointmentDto
            {
                Id = appointment.Entity.Id,
                VetId = appointment.Entity.VetId,
                PetId = appointment.Entity.PetId,
                ScheduledDate = appointment.Entity.ScheduledDate,
                EstimatedDurationInMinutes = appointment.Entity.EstimatedDurationInMinutes
            };

            return Created(nameof(GetAllAppointments), appointment);
        }

        [HttpGet]
        public IActionResult GetAllAppointments()
        {
            var appointments = appointmentRepository.All().Select(appointment => new AppointmentDto()
            {
                Id = appointment.Id,
                VetId = appointment.VetId,
                PetId = appointment.PetId,
                ScheduledDate = appointment.ScheduledDate,
                EstimatedDurationInMinutes = appointment.EstimatedDurationInMinutes,
                TreatmentId = appointment.TreatmentId,
                MedicalHistoryId = appointment.MedicalHistoryId
            });
            
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public IActionResult GetAppointmentById(Guid id)
        {
            var appointment = appointmentRepository.Get(id);
            if (appointment == null)
            {
                return NotFound();
            }

            var appointmentDto = new AppointmentDto
            {
                Id = appointment.Id,
                VetId = appointment.VetId,
                PetId = appointment.PetId,
                ScheduledDate = appointment.ScheduledDate,
                EstimatedDurationInMinutes = appointment.EstimatedDurationInMinutes,
                TreatmentId = appointment.TreatmentId,
                MedicalHistoryId = appointment.MedicalHistoryId
            };

            return Ok(appointmentDto);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAppointment(Guid id)
        {
            var appointment = appointmentRepository.Get(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointmentRepository.Delete(appointment);
            appointmentRepository.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAppointment(Guid id, [FromBody] AppointmentDto appointmentDto)
        {
            var appointment = appointmentRepository.Get(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Update(appointment.VetId, appointmentDto.PetId, appointmentDto.ScheduledDate,
                appointmentDto.EstimatedDurationInMinutes, appointmentDto.TreatmentId, appointmentDto.MedicalHistoryId);

            appointmentRepository.Update(appointment);
            appointmentRepository.SaveChanges();

            return NoContent();
        }
    }
}
