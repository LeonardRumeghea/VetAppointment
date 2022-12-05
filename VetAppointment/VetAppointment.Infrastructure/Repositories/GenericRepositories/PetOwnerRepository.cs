using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.Infrastructure.Repositories.GenericRepositories
{
    public class PetOwnerRepository : Repository<PetOwner>
    {
        public PetOwnerRepository(DatabaseContext context) : base(context) { }
    }
}
