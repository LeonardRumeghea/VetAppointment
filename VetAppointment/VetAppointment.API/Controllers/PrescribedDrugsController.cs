using Microsoft.AspNetCore.Mvc;
using VetAppointment.API.Dtos;
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
            var drugs = prescribedDrugRepository.All().Select
                (
                    d => new PrescribedDrugDto
                    {
                        Quantity = d.Quantity,
                        DrugId = d.DrugToPrescribeId,
                        TotalCost = d.TotalCost
                    }
                );

            return Ok(drugs);
        }

        [HttpPost]
        public IActionResult Create([FromBody] PrescribedDrugDto drugDto)
        {
            var baseDrug = drugRepository.Get(drugDto.DrugId);
            var drug = PrescribedDrug.Create(
                    drugDto.Quantity,
                    baseDrug
                );

            if (drug.IsFailure)
            {
                return BadRequest(drug.Error);
            }

            prescribedDrugRepository.Add(drug.Entity);
            prescribedDrugRepository.SaveChanges();

            return Created(nameof(Get), drug.Entity);
        }
    }
}
