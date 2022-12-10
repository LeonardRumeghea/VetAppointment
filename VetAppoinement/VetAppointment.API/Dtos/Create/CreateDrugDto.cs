#nullable disable
namespace VetAppointment.API.Dtos.Create
{
    public class CreateDrugDto
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
