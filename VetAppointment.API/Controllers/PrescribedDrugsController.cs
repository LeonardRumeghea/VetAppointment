using Microsoft.AspNetCore.Mvc;
using VetAppointment.API.Dtos;
using VetAppointment.API.Dtos.Create;
using VetAppointment.Application;
using VetAppointment.Domain;

namespace VetAppointment.API.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class PrescribedDrugsController : ControllerBase
    {
        private readonly IRepository<PrescribedDrug> prescribedDrugRepository;
        private readonly IRepository<Drug> drugRepository;

        public PrescribedDrugsController(IRepository<PrescribedDrug> prescribedDrugRepository,
            IRepository<Drug> drugRepository)
        {
            this.prescribedDrugRepository = prescribedDrugRepository;
            this.drugRepository = drugRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var drugs = prescribedDrugRepository
                .All()
                .Select(
                    d => new PrescribedDrugDto
                    {
                        Id = d.Id,
                        Quantity = d.Quantity,
                        DrugId = d.DrugToPrescribeId,
                        TotalCost = d.TotalCost
                    }
                );

            return Ok(drugs);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreatePrescribedDrugDto drugDto)
        {
            var drug = drugRepository.Get(drugDto.DrugId);
            if (drug == null)
            {
                return NotFound();
            }

            var prescribedDrug = PrescribedDrug.Create(drugDto.Quantity, drug);
            if (prescribedDrug.IsFailure)
            {
                return BadRequest(prescribedDrug.Error);
            }

            prescribedDrugRepository.Add(prescribedDrug.Entity);
            prescribedDrugRepository.SaveChanges();

            var fullPrescribedDrug = new PrescribedDrugDto
            {
                Id = prescribedDrug.Entity.Id,
                Quantity = prescribedDrug.Entity.Quantity,
                DrugId = prescribedDrug.Entity.DrugToPrescribeId,
                TotalCost = prescribedDrug.Entity.TotalCost
            };

            return Created(nameof(Get), fullPrescribedDrug);
        }
    }
}
