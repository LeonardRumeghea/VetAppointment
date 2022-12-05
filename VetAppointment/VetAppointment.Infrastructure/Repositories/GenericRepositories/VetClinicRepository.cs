using Microsoft.EntityFrameworkCore;
using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.Infrastructure.Repositories.GenericRepositories
{
    public class VetClinicRepository : Repository<VetClinic>
    {
        private readonly DatabaseContext context;
        public VetClinicRepository(DatabaseContext context) : base(context) 
        {
            this.context = context;
        }

        public override VetClinic Get(Guid id) 
            => context.Set<VetClinic>().Include(x => x.Vets).Include(x => x.Pets).FirstOrDefault(x => x.Id == id);

        public override IEnumerable<VetClinic> All()
            => context.Set<VetClinic>().Include(x => x.Vets).Include(x => x.Pets).ToList();
    }
}
