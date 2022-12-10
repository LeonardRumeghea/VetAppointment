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

        public PetsController(IRepository<Pet> petRepository) => this.petRepository = petRepository;

        [HttpGet]
        public IActionResult Get()
        {
            var pets = petRepository
                .All()
                .Select(
                    p => new PetDto
                    {
                        Id= p.Id,
                        Name = p.Name,
                        Birthdate = p.Birthdate.ToString(),
                        Race = p.Race.ToString(),
                        Gender = p.Gender.ToString()
                    }
                );
            
            return Ok(pets);
        }
    } 
}
