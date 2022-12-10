using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.Infrastructure.Repositories.GenericRepositories
{
    public class PetRepository : Repository<Pet>
    {
        public PetRepository(DatabaseContext context) : base(context) { }
    }
}
