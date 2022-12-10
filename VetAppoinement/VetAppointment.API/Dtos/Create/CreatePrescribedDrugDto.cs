namespace VetAppointment.API.Dtos.Create
{
    public class CreatePrescribedDrugDto
    {
        public double Quantity { get; set; }
        public double TotalCost { get; set; }
        public Guid DrugId { get; set; }
    }
}
