using Microsoft.AspNetCore.Mvc;
using System;
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

        public VetClinicsController(IRepository<VetClinic> vetClinicRepository, IRepository<Pet> petRepository,
            IRepository<Vet> vetRepository, IRepository<Appointment> appointmentRepository)
        {
            this.vetClinicRepository = vetClinicRepository;
            this.petRepository = petRepository;
            this.vetRepository = vetRepository;
            this.appointmentRepository = appointmentRepository;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateVetClinicDto vetClinicDto)
        {
            var vetClinic = VetClinic.Create(
                    vetClinicDto.Name,
                    vetClinicDto.Address,
                    vetClinicDto.NumberOfPlaces,
                    vetClinicDto.ContactEmail,
                    vetClinicDto.ContactPhone
                );

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
                RegistrationDate = vetClinic.Entity.RegistrationDate
            };

            vetClinicRepository.Add(vetClinic.Entity);
            vetClinicRepository.SaveChanges();

            return Created(nameof(GetAllVetClinics), fullClinic);
        }

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
                RegistrationDate = vet.RegistrationDate
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
            var vetClinic = VetClinic.Create(
                    vetClinicDto.Name,
                    vetClinicDto.Address,
                    vetClinicDto.NumberOfPlaces,
                    vetClinicDto.ContactEmail,
                    vetClinicDto.ContactPhone
                );

            if (vetClinic.IsFailure)
            {
                return BadRequest(vetClinic.Error);
            }

            var clinicToUpdate = vetClinicRepository.Get(vetClinicId);
            if (clinicToUpdate == null)
            {
                return NotFound();
            }
            clinicToUpdate = vetClinic.Entity;
            vetClinicRepository.Update(clinicToUpdate);
            vetClinicRepository.SaveChanges();
            return NoContent();
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
    }
}