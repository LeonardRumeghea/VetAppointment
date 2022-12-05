using Microsoft.AspNetCore.Mvc;
using VetAppointment.API.Dtos;
using VetAppointment.API.Dtos.Create;
using VetAppointment.Application;
using VetAppointment.Domain;

namespace VetAppointment.API.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class VetClinicsController : ControllerBase
    {
        private readonly IRepository<VetClinic> vetClinicRepository;
        private readonly IRepository<Pet> petRepository;
        private readonly IRepository<Vet> vetRepository;
        private readonly IRepository<Appointment> appointmentRepository;
        private readonly IRepository<MedicalHistory> medicalHistoryRepository;

        public VetClinicsController(IRepository<VetClinic> vetClinicRepository, IRepository<Pet> petRepository,
            IRepository<Vet> vetRepository, IRepository<Appointment> appointmentRepository,
            IRepository<MedicalHistory> medicalHistoryRepository)
        {
            this.vetClinicRepository = vetClinicRepository;
            this.petRepository = petRepository;
            this.vetRepository = vetRepository;
            this.appointmentRepository = appointmentRepository;
            this.medicalHistoryRepository = medicalHistoryRepository;
        
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateVetClinicDto vetClinicDto)
        {
            var history = MedicalHistory.Create();
            var vetClinic = VetClinic.Create(
                    vetClinicDto.Name,
                    vetClinicDto.Address,
                    vetClinicDto.NumberOfPlaces,
                    vetClinicDto.ContactEmail,
                    vetClinicDto.ContactPhone
                );
            history.Entity.AtachToClinic(vetClinic.Entity.Id);
            vetClinic.Entity.AttachMedicalHistory(history.Entity.Id);

            if (vetClinic.IsFailure)
            {
                return BadRequest(vetClinic.Error);
            }

            //vetClinicRepository.Add(vetClinic.Entity);
            var fullClinic = new VetClinicDto
            {
                Id = vetClinic.Entity.Id,
                Name = vetClinic.Entity.Name,
                Address = vetClinic.Entity.Address,
                NumberOfPlaces = vetClinic.Entity.NumberOfPlaces,
                ContactEmail = vetClinic.Entity.ContactEmail,
                ContactPhone = vetClinic.Entity.ContactPhone,
                RegistrationDate = vetClinic.Entity.RegistrationDate,
                MedicalHistoryId = history.Entity.Id
            };

            medicalHistoryRepository.Add(history.Entity);
            medicalHistoryRepository.SaveChanges();
            vetClinicRepository.Add(vetClinic.Entity);
            vetClinicRepository.SaveChanges();

            return Created(nameof(GetAllVetClinics), fullClinic);
        }

        // Gets - Clinic, Vets, Pets, Appointments, MedicalHistory (by vet, pet, clinic), Drug
        [HttpGet]
        public IActionResult GetAllVetClinics()
        {
            var vetClinics = vetClinicRepository.All().Select(vet => new VetClinicDto()
            {
                Id = vet.Id,
                Name = vet.Name,
                Address = vet.Address,
                NumberOfPlaces = vet.NumberOfPlaces,
                ContactEmail = vet.ContactEmail,
                ContactPhone = vet.ContactPhone,
                RegistrationDate = vet.RegistrationDate,
                MedicalHistoryId = vet.MedicalHistoryId
            });
            return Ok(vetClinics);
        }

        [HttpGet("{vetClinicId:guid}")]
        public IActionResult GetById(Guid vetClinicId)
        {
            var clinic = vetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }
            return Ok(clinic);
        }

        [HttpGet("{vetClinicId:guid}/vets")]
        public IActionResult GetVetsByClinicId(Guid vetClinicId)
        {
            var clinic = vetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var vets = clinic.Vets.Select(vet => new VetDto()
            {
                Id = vet.Id,
                Name = vet.Name,
                Surname = vet.Surname,
                Birthdate = vet.Birthdate.ToString(),
                Specialisation = vet.Specialisation.ToString(),
                Email = vet.Email,
                Gender = vet.Gender.ToString(),
                Phone = vet.Phone,
            });

            return Ok(vets);
        }

        [HttpGet("{vetClinicId:guid}/pets")]
        public IActionResult GetPetsByClinicId(Guid vetClinicId)
        {
            var clinic = vetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var pets = clinic.Pets.Select(pet => new PetDto()
            {
                Id = pet.Id,
                Name = pet.Name,
                Birthdate = pet.Birthdate.ToString(),
                Gender = pet.Gender.ToString(),
                Race = pet.Race.ToString(),
            });

            return Ok(pets);
        }

        [HttpGet("{vetClinicId:guid}/appointments")]
        public IActionResult GetAppointmentsByClinicId(Guid vetClinicId)
        {
            var clinic = vetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var medicalHistory = medicalHistoryRepository.Get(clinic.MedicalHistoryId);

            var appointments = medicalHistory.Appointments.Select(appointment => new AppointmentDto()
            {
                Id = appointment.Id,
                EstimatedDurationInMinutes = appointment.EstimatedDurationInMinutes,
                PetId = appointment.PetId,
                VetId = appointment.VetId,
                ScheduledDate = appointment.ScheduledDate,
                TreatmentId = appointment.TreatmentId
            });

            return Ok(appointments);
        }

        // Post - Vet, Pets, Appointment, Drug
        [HttpPost("{vetClinicId:guid}/pets")]
        public IActionResult RegisterPetsFamily(Guid vetClinicId, [FromBody] List<PetDto> petsDtos)
        {
            var clinic = vetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var pets = petsDtos.Select(p => Pet.Create(p.Name, p.Birthdate, p.Race, p.Gender)).ToList();
            if (pets.Any(p => p.IsFailure))
            {
                return BadRequest();
            }

            var result = clinic.RegisterPetsFamilyToClinic(pets.Select(p => p.Entity).ToList());
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            pets.ForEach(p => petRepository.Add(p.Entity));
            petRepository.SaveChanges();

            return NoContent();
        }

        [HttpPost("{vetClinicId:guid}/vet")]
        public IActionResult RegisterVet(Guid vetClinicId, [FromBody] VetDto vetDto)
        {
            var clinic = vetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var doctor = Vet.Create(vetDto.Name, vetDto.Surname, vetDto.Birthdate, vetDto.Gender, vetDto.Email, vetDto.Phone, vetDto.Specialisation);
            if (doctor.IsFailure)
            {
                return BadRequest();
            }

            var result = clinic.RegisterVetToClinic(doctor.Entity);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            vetClinicRepository.Update(clinic);
            vetRepository.Add(doctor.Entity);
            vetRepository.SaveChanges();

            return NoContent();
        }

        [HttpPut("{vetClinicId:guid}")]
        public IActionResult Update(Guid vetClinicId, [FromBody] VetClinicDto vetClinicDto)
        {
// Pentru front: Daca nu completeaza partea de "Number Of Places" atunci by default va fi 0.
// In caz ca "Number of Places" necompletat va fi inlocuit cu "Number of pleces" din clinica luata dupa ID
            if (vetClinicDto == null)
            {
                return BadRequest();
            }

            var clinic = vetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var result = clinic.Update(vetClinicDto.Name, vetClinicDto.Address, vetClinicDto.NumberOfPlaces, vetClinicDto.ContactEmail, vetClinicDto.ContactPhone);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            vetClinicRepository.Update(result.Entity);
            vetClinicRepository.SaveChanges();

            return NoContent();
        }

        [HttpPut("{vetClinicId:guid}/vet/{vetId:guid}")]
        public IActionResult UpdateVet(Guid vetClinicId, Guid vetId, [FromBody] VetDto vetDto)
        {
            var clinic = vetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var vet = vetRepository.Get(vetId);
            if (vet == null)
            {
                return NotFound();
            }

            var result = vet.Update(vetDto.Name, vetDto.Surname, vetDto.Birthdate, vetDto.Gender, vetDto.Email, vetDto.Phone, vetDto.Specialisation);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            vetRepository.Update(vet);
            vetRepository.SaveChanges();

            return Ok();
        }

        [HttpPut("{vetClinicId:guid}/vet/{petId:guid}")]
        public IActionResult UpdatePet(Guid vetClinicId, Guid petId, [FromBody] PetDto petDto)
        {
            var clinic = vetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var pet = petRepository.Get(petId);
            if (pet == null)
            {
                return NotFound();
            }

            var result = pet.Update(petDto.Name, petDto.Birthdate, petDto.Race, petDto.Gender);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            petRepository.Update(pet);
            petRepository.SaveChanges();

            return Ok();
        }
        
        [HttpDelete("{vetClinicId:guid}")]
        public IActionResult Delete(Guid vetClinicId)
        {
            var vetClinic = vetClinicRepository.Get(vetClinicId);
            if (vetClinic == null)
            {
                return NotFound();
            }
            vetClinicRepository.Delete(vetClinic);
            vetClinicRepository.SaveChanges();
            return Ok();
        }

        [HttpDelete("{vetClinicId:guid}/vet/{vetId:guid}")]
        public IActionResult DeleteVet(Guid vetClinicId, Guid vetId)
        {
            var clinic = vetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var vet = vetRepository.Get(vetId);
            if (vet == null)
            {
                return NotFound();
            }

            vetRepository.Delete(vet);
            vetRepository.SaveChanges();

            return Ok();
        }

        [HttpDelete("{vetClinicId:guid}/pet/{petId:guid}")]
        public IActionResult DeletePet(Guid vetClinicId, Guid petId)
        {
            var clinic = vetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var pet = petRepository.Get(petId);
            if (pet == null)
            {
                return NotFound();
            }

            petRepository.Delete(pet);
            petRepository.SaveChanges();

            return Ok();
        }
    }
}