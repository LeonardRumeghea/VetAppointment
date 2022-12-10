#nullable disable
using VetAppointment.API.Dtos.Create;

namespace VetAppointment.API.Dtos
{
    public class DrugDto : CreateDrugDto
    {
        public Guid Id { get; set; }
    }
}
