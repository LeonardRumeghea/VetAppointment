using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.Infrastructure.Repositories.GenericRepositories
{
    public class VetRepository : Repository<Vet>
    {
        public VetRepository(DatabaseContext context) : base(context) { }
    }

}
