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
        private readonly IRepository<Pet> petRepository;
        private readonly IRepository<Vet> vetRepository;

        public AppointmentsController(IRepository<Appointment> appointmentRepository, 
            IRepository<Pet> petRepository, IRepository<Vet> vetRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.petRepository = petRepository;
            this.vetRepository = vetRepository;
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

            if (appointment.IsFailure)
            {
                return BadRequest(appointment.Error);
            }
            
            appointmentRepository.Add(appointment.Entity);
            appointmentRepository.SaveChanges();
            //var fullAppointment = new AppointmentDto
            //{
            //    Id = appointment.Entity.Id,
            //    VetId = appointment.Entity.VetId,
            //    PetId = appointment.Entity.PetId,
            //    ScheduledDate = appointment.Entity.ScheduledDate,
            //    EstimatedDurationInMinutes = appointment.Entity.EstimatedDurationInMinutes
            //};

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
                EstimatedDurationInMinutes = appointment.EstimatedDurationInMinutes
            });
            
            return Ok(appointments);
        }
    }
}
