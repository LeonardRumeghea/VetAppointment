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
    public class VetClinicsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public VetClinicsController(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

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

            if (vetClinic == null)
            {
                return BadRequest();
            }

            history.Entity.AtachToClinic(vetClinic.Entity.Id);
            vetClinic.Entity.AttachMedicalHistory(history.Entity.Id);

            if (vetClinic.IsFailure)
            {
                return BadRequest(vetClinic.Error);
            }

            unitOfWork.MedicalHistoryRepository.Add(history.Entity);
            unitOfWork.SaveChanges();
            
            unitOfWork.VetClinicRepository.Add(vetClinic.Entity);
            unitOfWork.SaveChanges();

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

            return Created(nameof(GetAllVetClinics), fullClinic);
        }

        // Gets - Clinic, Vets, Pets, Appointments, MedicalHistory (by vet, pet, clinic), Drug
        [HttpGet]
        public IActionResult GetAllVetClinics()
        {
            var vetClinics = unitOfWork.VetClinicRepository
                .All()
                .Select(
                    vet => new VetClinicDto()
                    {
                        Id = vet.Id,
                        Name = vet.Name,
                        Address = vet.Address,
                        NumberOfPlaces = vet.NumberOfPlaces,
                        ContactEmail = vet.ContactEmail,
                        ContactPhone = vet.ContactPhone,
                        RegistrationDate = vet.RegistrationDate,
                        MedicalHistoryId = vet.MedicalHistoryId
                    }
                );
            
            return Ok(vetClinics);
        }

        [HttpGet("{vetClinicId:guid}")]
        public IActionResult GetById(Guid vetClinicId)
        {
            var clinic = unitOfWork.VetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }
            
            return Ok(clinic);
        }

        [HttpGet("{vetClinicId:guid}/vets")]
        public IActionResult GetVetsByClinicId(Guid vetClinicId)
        {
            var clinic = unitOfWork.VetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var vets = clinic.Vets
                .Select(vet => new VetDto()
                {
                    Id = vet.Id,
                    ClinicId = vet.ClinicId,
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
            var clinic = unitOfWork.VetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var pets = clinic.Pets
                .Select(pet => new PetDto()
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
            var clinic = unitOfWork.VetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var medicalHistory = unitOfWork.MedicalHistoryRepository.Get(clinic.MedicalHistoryId);

            var appointments = medicalHistory.Appointments
                .Select(
                    appointment => new AppointmentDto()
                    {
                        Id = appointment.Id,
                        EstimatedDurationInMinutes = appointment.EstimatedDurationInMinutes,
                        PetId = appointment.PetId,
                        VetId = appointment.VetId,
                        ScheduledDate = appointment.ScheduledDate.ToString(),
                        TreatmentId = appointment.TreatmentId
                    }
                );

            return Ok(appointments);
        }

        // Post - Vet, Pets, Appointment, Drug
        [HttpPost("{vetClinicId:guid}/pets")]
        public IActionResult RegisterPetsFamily(Guid vetClinicId, [FromBody] List<CreatePetDto> petsDtos)
        {
            var clinic = unitOfWork.VetClinicRepository.Get(vetClinicId);
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

            pets.ForEach(p => unitOfWork.PetRepository.Add(p.Entity));
            unitOfWork.SaveChanges();


            var createdPets = pets.Select(
                pet => new PetDto()
                {
                    Name = pet.Entity.Name,
                    Birthdate = pet.Entity.Birthdate.ToString(),
                    Gender = pet.Entity.Gender.ToString(),
                    Race = pet.Entity.Race.ToString(),
                    Id = pet.Entity.Id,
                });

            return Created(nameof(GetPetsByClinicId), createdPets);
        }

        [HttpPost("{vetClinicId:guid}/vet")]
        public IActionResult RegisterVet(Guid vetClinicId, [FromBody] CreateVetDto vetDto)
        {
            var clinic = unitOfWork.VetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var doctor = Vet.Create(vetDto.Name, vetDto.Surname, vetDto.Birthdate, vetDto.Gender, vetDto.Email, 
                vetDto.Phone, vetDto.Specialisation);
            if (doctor.IsFailure)
            {
                return BadRequest();
            }

            var result = clinic.RegisterVetToClinic(doctor.Entity);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            unitOfWork.VetClinicRepository.Update(clinic);
            unitOfWork.VetRepository.Add(doctor.Entity);
            unitOfWork.SaveChanges();

            var createVet = new VetDto()
            {
                Name = doctor.Entity.Name,
                Surname = doctor.Entity.Surname,
                Specialisation = doctor.Entity.Specialisation.ToString(),
                Birthdate = doctor.Entity.Birthdate.ToString(),
                Gender = doctor.Entity.Gender.ToString(),
                Email = doctor.Entity.Email,
                Phone = doctor.Entity.Phone,
                Id = doctor.Entity.Id,
            };

            return Created(nameof(RegisterVet), createVet);
        }

        [HttpPost("{vetClinicId:guid}/appointment")]
        public IActionResult RegisterAppointment(Guid vetClinicId, [FromBody] CreateAppointmentDto appointmentDto)
        {
            var clinic = unitOfWork.VetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var medicalHistory = unitOfWork.MedicalHistoryRepository.Get(clinic.MedicalHistoryId);

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

            var appointment = Appointment.SettleAppointment(vet, pet, appointmentDto.ScheduledDate,
                appointmentDto.EstimatedDurationInMinutes);
            if (appointment.IsFailure)
            {
                return BadRequest();
            }

            var result = medicalHistory.RegisterAppointmentToHistory(appointment.Entity);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            unitOfWork.MedicalHistoryRepository.Update(medicalHistory);
            unitOfWork.AppointmentRepository.Add(appointment.Entity);
            unitOfWork.SaveChanges();

            var createdAppointment = new AppointmentDto()
            {
                Id = appointment.Entity.Id,
                EstimatedDurationInMinutes = appointment.Entity.EstimatedDurationInMinutes,
                PetId = appointment.Entity.PetId,
                VetId = appointment.Entity.VetId,
                ScheduledDate = appointment.Entity.ScheduledDate.ToString(),
                TreatmentId = appointment.Entity.TreatmentId
            };

            return Created(nameof(RegisterAppointment), createdAppointment);
        }

        [HttpPut("{vetClinicId:guid}")]
        public IActionResult Update(Guid vetClinicId, [FromBody] VetClinicDto vetClinicDto)
        {
            var clinic = unitOfWork.VetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var result = clinic.Update(vetClinicDto.Name, vetClinicDto.Address, vetClinicDto.NumberOfPlaces, 
                vetClinicDto.ContactEmail, vetClinicDto.ContactPhone);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            unitOfWork.VetClinicRepository.Update(result.Entity);
            unitOfWork.SaveChanges();

            return NoContent();
        }

        [HttpPut("{vetClinicId:guid}/vet/{vetId:guid}")]
        public IActionResult UpdateVet(Guid vetClinicId, Guid vetId, [FromBody] VetDto vetDto)
        {
            var clinic = unitOfWork.VetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var vet = unitOfWork.VetRepository.Get(vetId);
            if (vet == null)
            {
                return NotFound();
            }

            var result = vet.Update(vetDto.Name, vetDto.Surname, vetDto.Birthdate, vetDto.Gender, vetDto.Email, 
                vetDto.Phone, vetDto.Specialisation);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            
            unitOfWork.VetRepository.Update(vet);
            unitOfWork.SaveChanges();

            return NoContent();
        }

        [HttpPut("{vetClinicId:guid}/pet/{petId:guid}")]
        public IActionResult UpdatePet(Guid vetClinicId, Guid petId, [FromBody] PetDto petDto)
        {
            var clinic = unitOfWork.VetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var pet = unitOfWork.PetRepository.Get(petId);
            if (pet == null)
            {
                return NotFound();
            }

            var result = pet.Update(petDto.Name, petDto.Birthdate, petDto.Race, petDto.Gender);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            unitOfWork.PetRepository.Update(pet);
            unitOfWork.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{vetClinicId:guid}")]
        public IActionResult Delete(Guid vetClinicId)
        {
            var vetClinic = unitOfWork.VetClinicRepository.Get(vetClinicId);
            if (vetClinic == null)
            {
                return NotFound();
            }

            var medicalHistorys = unitOfWork.MedicalHistoryRepository.All().Where(m => m.ClinicId == vetClinicId);
            if (medicalHistorys != null)
            {
                foreach (var item in medicalHistorys)
                {
                    unitOfWork.MedicalHistoryRepository.Delete(item);
                }
            }

            var vets = unitOfWork.VetRepository.All().Where(v => v.ClinicId == vetClinicId);
            if (vets != null)
            {
                foreach (var item in vets)
                {
                    unitOfWork.VetRepository.Delete(item);
                }
            }

            var pets = unitOfWork.PetRepository.All().Where(p => p.ClinicId == vetClinicId);
            if (pets != null)
            {
                foreach (var item in pets)
                {
                    unitOfWork.PetRepository.Delete(item);
                }
            }

            unitOfWork.VetClinicRepository.Delete(vetClinic);
            unitOfWork.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{vetClinicId:guid}/vet/{vetId:guid}")]
        public IActionResult DeleteVet(Guid vetClinicId, Guid vetId)
        {
            var clinic = unitOfWork.VetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var vet = unitOfWork.VetRepository.Get(vetId);
            if (vet == null)
            {
                return NotFound();
            }

            unitOfWork.VetRepository.Delete(vet);
            unitOfWork.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{vetClinicId:guid}/pet/{petId:guid}")]
        public IActionResult DeletePet(Guid vetClinicId, Guid petId)
        {
            var clinic = unitOfWork.VetClinicRepository.Get(vetClinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var pet = unitOfWork.PetRepository.Get(petId);
            if (pet == null)
            {
                return NotFound();
            }

            unitOfWork.PetRepository.Delete(pet);
            unitOfWork.SaveChanges();

            return NoContent();
        }
    }
}