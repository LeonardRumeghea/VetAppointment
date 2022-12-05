using Microsoft.AspNetCore.Mvc;
using VetAppointment.API.Dtos;
using VetAppointment.Application;
using VetAppointment.Domain;

namespace VetAppointment.API.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IRepository<Pet> petRepository;

        public PetsController (IRepository<Pet> petRepository)
        {
            this.petRepository = petRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var pets = petRepository.All().Select
                (
                    p => new PetDto
                    {
                        Name = p.Name,
                        Birthdate = p.Birthdate.ToString(),
                        Race = p.Race.ToString(),
                        Gender = p.Gender.ToString()
                    }
                );
            
            return Ok(pets);
        }

        [HttpPost]
        public IActionResult Create([FromBody] PetDto petDto)
        {
            var pet = Pet.Create(
                    petDto.Name,
                    petDto.Birthdate,
                    petDto.Race,
                    petDto.Gender
                );

            if (pet.IsFailure)
            {
                return BadRequest(pet.Error);
            }

            petRepository.Add(pet.Entity);
            petRepository.SaveChanges();

            return Created(nameof(Get), pet.Entity);
        }
    } 
}
