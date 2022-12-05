using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.Infrastructure.Repositories.GenericRepositories
{
    public class DrugRepository : Repository<Drug>
    {
        public DrugRepository(DatabaseContext context) : base(context) { }
    }
}
