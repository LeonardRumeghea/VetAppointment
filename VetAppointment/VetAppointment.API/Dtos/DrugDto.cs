#nullable disable
namespace VetAppointment.API.Dtos
{
    public class DrugDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
