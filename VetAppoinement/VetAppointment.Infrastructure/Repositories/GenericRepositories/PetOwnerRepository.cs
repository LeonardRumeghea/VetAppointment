using Microsoft.EntityFrameworkCore;
using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.Infrastructure.Repositories.GenericRepositories
{
    public class PetOwnerRepository : Repository<PetOwner>
    {
        private readonly DatabaseContext _databaseContext;
        public PetOwnerRepository(DatabaseContext context) : base(context) 
        {
            _databaseContext = context;
        }

        public override PetOwner Get(Guid id) => _databaseContext.Set<PetOwner>()
            .Include(x => x.Pets)
            .SingleOrDefault(x => x.Id == id);

        public override IEnumerable<PetOwner> All() => _databaseContext.Set<PetOwner>()
            .Include(x => x.Pets)
            .ToList();
    }
}
