using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.Infrastructure.Repositories.GenericRepositories
{
    public class VetClinicRepository : Repository<VetClinic>
    {
        public VetClinicRepository(DatabaseContext context) : base(context) { }
    }
}
