using Microsoft.AspNetCore.Mvc;
using VetAppointment.API.Dtos;
using VetAppointment.API.Dtos.Create;
using VetAppointment.Application;
using VetAppointment.Domain;

namespace VetAppointment.API.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class TreatmentsController : ControllerBase
    {
        private readonly IRepository<Treatment> treatmentRepository;
        private readonly IRepository<PrescribedDrug> prescribedDrugRepository;
        private readonly IRepository<Drug> drugRepository;

        public TreatmentsController(IRepository<Treatment> treatmentRepository, 
            IRepository<PrescribedDrug> prescribedDrugRepository, IRepository<Drug> drugRepository)
        {
            this.treatmentRepository = treatmentRepository;
            this.prescribedDrugRepository = prescribedDrugRepository;
            this.drugRepository = drugRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var treatments = treatmentRepository.All().Select
                (
                    treat => new TreatmentDto
                    {
                        Id = treat.Id,
                        Description = treat.Description
                    }
                );

            return Ok(treatments);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateTreatmentDto treatmentDto)
        {
            var treat = Treatment.Create(
                    treatmentDto.Description
                );

            if (treat.IsFailure)
            {
                return BadRequest(treat.Error);
            }

            treatmentRepository.Add(treat.Entity);
            treatmentRepository.SaveChanges();

            return Created(nameof(Get), treat);
        }

        [HttpPost("{treatmentId:Guid}/drug")]
        public IActionResult AddDrugsToTreatment(Guid treatmentId, [FromBody] List<PrescribedDrugDto> drugDtos)
        {
            var treatment = treatmentRepository.Get(treatmentId);
            if (treatment == null)
            {
                return NotFound();
            }

            var drugs = drugDtos.Select(d => PrescribedDrug.Create(d.Quantity, drugRepository.Get(d.DrugId))).ToList();
            if (drugs.Any(p => p.IsFailure))
            {
                return BadRequest();
            }

            var result = treatment.AppendDrugsToTreatment(drugs.Select(d => d.Entity).ToList());
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            drugs.ForEach(d => prescribedDrugRepository.Add(d.Entity));
            drugRepository.SaveChanges();

            return NoContent();
        }
    }
}
