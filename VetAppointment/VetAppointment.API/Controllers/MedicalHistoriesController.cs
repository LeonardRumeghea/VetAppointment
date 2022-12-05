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

        [HttpPost]
        public IActionResult Create([FromBody] CreateMedicalHistoryDto medHistoryDto)
        {
            var medHistory = MedicalHistory.Create(
                );

            if (medHistory.IsFailure)
            {
                return BadRequest(medHistory.Error);
            }

            medicalHistoryRepository.Add(medHistory.Entity);
            var fullMedHistory = new MedicalHistoryDto
            {
                Id = medHistory.Entity.Id,
            };

            medicalHistoryRepository.Add(medHistory.Entity);
            medicalHistoryRepository.SaveChanges();

            return Created(nameof(Get), fullMedHistory);
        }

        [HttpPost("{vetClinicId:guid}/appointment")]
        public IActionResult RegisterAppointment(Guid historyId, [FromBody] AppointmentDto appDto)
        {
            MedicalHistory medHistory = medicalHistoryRepository.Get(historyId);
            if (medHistory == null)
            {
                return NotFound();
            }
            var pet = petRepository.Get(appDto.PetId);
            if (pet == null)
            {
                return NotFound();
            }
            var vet = vetRepository.Get(appDto.VetId);
            if (vet == null)
            {
                return NotFound();
            }

            var appointment = Appointment.SettleAppointment(vet, pet, appDto.ScheduledDate, appDto.EstimatedDurationInMinutes);
            if (appointment.IsFailure)
            {
                return BadRequest();
            }

            var result = medHistory.RegisterAppointmentToHistory(appointment.Entity);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            medicalHistoryRepository.Update(medHistory);
            appointmentRepository.Add(appointment.Entity);
            appointmentRepository.SaveChanges();

            return NoContent();
        }
    }
}
