using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.Infrastructure.Repositories.GenericRepositories
{
    public class TreatmentRepository : Repository<Treatment>
    {
        public TreatmentRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
