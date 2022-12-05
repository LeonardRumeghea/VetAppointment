using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.Infrastructure.Repositories.GenericRepositories
{
    public class PrescribedDrugRepository : Repository<PrescribedDrug>
    {
        public PrescribedDrugRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
