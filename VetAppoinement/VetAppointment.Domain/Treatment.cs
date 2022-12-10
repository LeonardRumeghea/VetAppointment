using VetAppointment.Domain.Helpers;

namespace VetAppointment.Domain
{
    #nullable disable
    public class Treatment
    {
        public Guid Id { get; private set; }
        public string Description { get; private set; }
        public List<PrescribedDrug> PrescribedDrugs { get; private set; }

        public static Result<Treatment> Create(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return Result<Treatment>.Failure($"Description {description} of treatment is not valid");
            }

            var treatment = new Treatment
            {
                Id = Guid.NewGuid(),
                Description = description,
                PrescribedDrugs = new List<PrescribedDrug>()
            };

            return Result<Treatment>.Success(treatment);
        }

        public double CalculatePriceOfTreatment()
        {
            double totalPrice = 0;
            foreach (var drug in PrescribedDrugs)
            {
                totalPrice += drug.TotalCost;
            }
            return totalPrice;
        }

        public Result AppendDrugsToTreatment(List<PrescribedDrug> drugs)
        {
            if (!drugs.Any())
            {
                return Result.Failure("Register at least a drug to the treatment");
            }

            drugs.ForEach(drug => PrescribedDrugs.Add(drug));

            return Result.Success();
        }

        public Result RemoveDrugFromTreatment(PrescribedDrug prescribedDrug)
        {
            var result = this.PrescribedDrugs.Remove(prescribedDrug);

            if (result == false)
            {
                return Result.Failure("Not found");
            }

            return Result.Success();
        }

        public Result UpdateDescription(string description)
        {
            if (description == null)
            {
                return Result.Failure("Description can not be null");
            }

            if (this.Description.Equals(description))
            {
                return Result.Failure("Update Error");
            }

            this.Description = description;
            return Result.Success();
        }
    }
}
