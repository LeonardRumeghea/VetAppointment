using Microsoft.AspNetCore.Mvc;
using VetAppointment.API.Dtos;
using VetAppointment.API.Dtos.Create;
using VetAppointment.Application;
using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.API.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class TreatmentsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public TreatmentsController(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

        [HttpGet]
        public IActionResult Get()
        {
            var treatments = unitOfWork.TreatmentRepository
                .All()
                .Select ( t => new TreatmentDto { Id = t.Id, Description = t.Description } );

            return Ok(treatments);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateTreatmentDto treatmentDto)
        {
            var treat = Treatment.Create(treatmentDto.Description);
            if (treat.IsFailure)
            {
                return BadRequest(treat.Error);
            }

            unitOfWork.TreatmentRepository.Add(treat.Entity);
            unitOfWork.SaveChanges();

            var fullTreatment = new TreatmentDto()
            {
                Id = treat.Entity.Id,
                Description = treat.Entity.Description
            };

            return Created(nameof(Get), fullTreatment);
        }

        [HttpPost("{treatmentId:Guid}/prescribedDrugs")]
        public IActionResult AddDrugsToTreatment(Guid treatmentId, 
            [FromBody] List<PrescribedDrugDto> prescribedDrugDtos)
        {
            var treatment = unitOfWork.TreatmentRepository.Get(treatmentId);
            if (treatment == null)
            {
                return NotFound();
            }

            var drugs = prescribedDrugDtos
                .Select(d => PrescribedDrug.Create(d.Quantity, unitOfWork.DrugRepository.Get(d.DrugId)) ).ToList();
            if (drugs.Any(p => p.IsFailure))
            {
                return BadRequest();
            }

            var result = treatment.AppendDrugsToTreatment(drugs.Select(d => d.Entity).ToList());
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            drugs.ForEach(d => unitOfWork.PrescribedDrugRepository.Add(d.Entity));
            unitOfWork.SaveChanges();

            return NoContent();
        }

        [HttpPut("{treatmentId:Guid}")]
        public IActionResult UpdateTreatment(Guid treatmentId, [FromBody] CreateTreatmentDto treatmentDto)
        {
            var treatment = unitOfWork.TreatmentRepository.Get(treatmentId);
            if (treatment == null)
            {
                return NotFound();
            }

            var result = treatment.UpdateDescription(treatmentDto.Description);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            unitOfWork.TreatmentRepository.Update(treatment);
            unitOfWork.SaveChanges();

            return NoContent();
        }

        [HttpPut("{treatmentId:Guid}/prescribedDrug/{prescribedDrugId:Guid}")]
        public IActionResult UpdateDrugInTreatment(Guid treatmentId, Guid prescribedDrugId, 
            [FromBody] PrescribedDrugDto prescribedDrugDto)
        {
            var treatment = unitOfWork.TreatmentRepository.Get(treatmentId);
            if (treatment == null)
            {
                return NotFound();
            }

            var drugPrescribed = unitOfWork.PrescribedDrugRepository.Get(prescribedDrugId);
            if (drugPrescribed == null)
            {
                return NotFound();
            }

            var drug = unitOfWork.DrugRepository.Get(prescribedDrugDto.DrugId);
            if (drug == null)
            {
                return NotFound();
            }

            var result = drugPrescribed.Update(prescribedDrugDto.Quantity, drug);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            unitOfWork.PrescribedDrugRepository.Update(drugPrescribed);
            unitOfWork.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{treatmentId:Guid}/prescribedDrug/{prescribedDrugId:Guid}")]
        public IActionResult RemoveDrugFromTreatment(Guid treatmentId, Guid prescribedDrugId)
        {
            var treatment = unitOfWork.TreatmentRepository.Get(treatmentId);
            if (treatment == null)
            {
                return NotFound();
            }

            var drug = unitOfWork.PrescribedDrugRepository.Get(prescribedDrugId);
            if (drug == null)
            {
                return NotFound();
            }

            var result = treatment.RemoveDrugFromTreatment(drug);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            unitOfWork.PrescribedDrugRepository.Delete(drug);
            unitOfWork.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{treatmentId:Guid}")]
        public IActionResult Delete(Guid treatmentId)
        {
            var treatment = unitOfWork.TreatmentRepository.Get(treatmentId);
            if (treatment == null)
            {
                return NotFound();
            }

            unitOfWork.TreatmentRepository.Delete(treatment);
            unitOfWork.SaveChanges();

            return NoContent();
        }

    }
}
