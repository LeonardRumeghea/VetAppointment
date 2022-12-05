#nullable disable
namespace VetAppointment.API.Dtos.Create
{
    public class CreateVetClinicDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int NumberOfPlaces { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
    }
}
