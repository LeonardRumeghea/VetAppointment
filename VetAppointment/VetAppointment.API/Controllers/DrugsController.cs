using Microsoft.AspNetCore.Mvc;
using VetAppointment.API.Dtos;
using VetAppointment.Application;
using VetAppointment.Domain;
using VetAppointment.Infrastructure.Repositories.GenericRepositories;

namespace VetAppointment.API.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class DrugsController : ControllerBase
    {
        private readonly IRepository<Drug> drugRepository;

        public DrugsController(IRepository<Drug> drugRepository)
        {
            this.drugRepository = drugRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var drugs = drugRepository.All().Select
                (
                    d => new DrugDto
                    {
                        Name = d.Name,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice
                    }
                );

            return Ok(drugs);
        }

        [HttpPost]
        public IActionResult Create([FromBody] DrugDto drugDto)
        {
            var drug = Drug.Create(
                    drugDto.Name,
                    drugDto.Quantity,
                    drugDto.UnitPrice
                );

            if (drug.IsFailure)
            {
                return BadRequest(drug.Error);
            }

            drugRepository.Add(drug.Entity);
            drugRepository.SaveChanges();

            return Created(nameof(Get), drug.Entity);
        }
    }

}
