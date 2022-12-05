using Microsoft.AspNetCore.Mvc;
using VetAppointment.API.Dtos;
using VetAppointment.Application;
using VetAppointment.Domain;

namespace VetAppointment.API.Features.Vets
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class VetsController : ControllerBase
    {
        private readonly IRepository<Vet> vetRepository;

        public VetsController(IRepository<Vet> vetRepository)
        {
            this.vetRepository = vetRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var vets = vetRepository.All().Select
            (
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
        public IActionResult Create([FromBody] VetDto vetDto)
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

            return Created(nameof(Get), vet.Entity);
        }
    }
}