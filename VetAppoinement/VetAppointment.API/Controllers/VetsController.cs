using Microsoft.AspNetCore.Mvc;
using VetAppointment.API.Dtos;
using VetAppointment.API.Dtos.Create;
using VetAppointment.Application;
using VetAppointment.Domain;

namespace VetAppointment.API.Features.Vets
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class VetsController : ControllerBase
    {
        private readonly IRepository<Vet> vetRepository;

        public VetsController(IRepository<Vet> vetRepository) => this.vetRepository = vetRepository;

        [HttpGet]
        public IActionResult Get()
        {
            var vets = vetRepository
                .All()
                .Select(
                    v => new VetDto()
                    {
                        Id = v.Id,
                        Name = v.Name,
                        Surname = v.Surname,
                        Birthdate = v.Birthdate.ToString(),
                        Gender = v.Gender.ToString(),
                        Email = v.Email,
                        Phone = v.Phone,
                        Specialisation = v.Specialisation.ToString()
                    }
                );
            
            return Ok(vets);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateVetDto vetDto)
        {
            var vet = Vet.Create(
                    vetDto.Name,
                    vetDto.Surname,
                    vetDto.Birthdate,
                    vetDto.Gender,
                    vetDto.Email,
                    vetDto.Phone,
                    vetDto.Specialisation
                );

            if (vet.IsFailure)
            {
                return BadRequest(vet.Error);
            }

            vetRepository.Add(vet.Entity);
            vetRepository.SaveChanges();

            var fullVet = new VetDto()
            {
                Id = vet.Entity.Id,
                ClinicId = vet.Entity.ClinicId,
                Name = vet.Entity.Name,
                Surname = vet.Entity.Surname,
                Birthdate = vet.Entity.Birthdate.ToString(),
                Gender = vet.Entity.Gender.ToString(),
                Email = vet.Entity.Email,
                Phone = vet.Entity.Phone,
                Specialisation = vet.Entity.Specialisation.ToString()
            };

            return Created(nameof(Get), fullVet);
        }
    }
}