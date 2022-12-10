using Microsoft.AspNetCore.Mvc;
using VetAppointment.API.Dtos;
using VetAppointment.API.Dtos.Create;
using VetAppointment.Application;
using VetAppointment.Domain;

namespace VetAppointment.API.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class PetOwnersController : ControllerBase
    {
        private readonly IRepository<PetOwner> petOwnerRepository;
        private readonly IRepository<Pet> petRepository;

        public PetOwnersController(IRepository<PetOwner> petOwnerRepository, IRepository<Pet> petRepository)
        {
            this.petOwnerRepository = petOwnerRepository;
            this.petRepository = petRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var petOwners = petOwnerRepository
                .All()
                .Select(
                    owner => new PetOwnerDto
                    {
                        Id = owner.Id,
                        Name = owner.Name,
                        Surname = owner.Surname,
                        Birthdate = owner.Birthdate.ToString(),
                        Gender = owner.Gender.ToString(),
                        Address = owner.Address,
                        Email = owner.Email,
                        Phone = owner.Phone
                    });

            return Ok(petOwners);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreatePetOwnerDto petOwnerDto)
        {
            var petOwner = PetOwner.Create (
                    petOwnerDto.Name,
                    petOwnerDto.Surname,
                    petOwnerDto.Birthdate,
                    petOwnerDto.Gender,
                    petOwnerDto.Address,
                    petOwnerDto.Email,
                    petOwnerDto.Phone
                );

            if (petOwner.IsFailure)
            {
                return BadRequest(petOwner.Error);
            }

            petOwnerRepository.Add(petOwner.Entity);
            petOwnerRepository.SaveChanges();

            var fullPetOwner = new PetOwnerDto()
            {
                Name = petOwner.Entity.Name,
                Surname = petOwner.Entity.Surname,
                Address = petOwner.Entity.Address,
                Birthdate = petOwner.Entity.Birthdate.ToString(),
                Email = petOwner.Entity.Email,
                Gender = petOwner.Entity.Gender.ToString(),
                Phone = petOwner.Entity.Phone,
                Id = petOwner.Entity.Id
            };

            return Created(nameof(Get), fullPetOwner);

        }

        [HttpPost ("{ownerId:guid}/pets")]
        public IActionResult RegisterPetsToOwner(Guid ownerId, [FromBody] List<CreatePetDto> petsDtos)
        {
            var owner = petOwnerRepository.Get(ownerId);
            if (owner == null)
            {
                return NotFound();
            }

            var pets = petsDtos.Select(p => Pet.Create(p.Name, p.Birthdate, p.Race, p.Gender)).ToList();
            if(pets.Any(p => p.IsFailure))
            {
                return BadRequest();
            
            }

            var result = owner.RegisterPetsToOwner(pets.Select(p => p.Entity).ToList());

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            pets.ForEach(p => petRepository.Add(p.Entity));
            petOwnerRepository.SaveChanges();

            return Created(nameof(Get), owner);
        }
    }
}
