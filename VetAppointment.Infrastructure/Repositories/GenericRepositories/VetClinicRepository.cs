using Microsoft.EntityFrameworkCore;
using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.Infrastructure.Repositories.GenericRepositories
{
    public class VetClinicRepository : Repository<VetClinic>
    {
        private readonly DatabaseContext _databaseContext;
        public VetClinicRepository(DatabaseContext context) : base(context) 
        {
            _databaseContext = context;
        }

        public override VetClinic Get(Guid id) => _databaseContext.Set<VetClinic>()
            .Include(x => x.Pets)
            .Include(x => x.Vets)
            .SingleOrDefault(x => x.Id == id);
        
        public override IEnumerable<VetClinic> All() => _databaseContext.Set<VetClinic>()
            .Include(x => x.Pets)
            .Include(x => x.Vets)
            .ToList();

    }
}
