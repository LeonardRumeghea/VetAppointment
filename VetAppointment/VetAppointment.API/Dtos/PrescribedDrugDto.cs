namespace VetAppointment.API.Dtos
{
    public class PrescribedDrugDto
    {
        public double Quantity { get; set; }
        public double TotalCost { get; set; } 
        public Guid DrugId { get; set; }
            
    }
}
