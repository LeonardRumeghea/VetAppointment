using Microsoft.EntityFrameworkCore;
using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.Infrastructure.Repositories.GenericRepositories
{
    public class MedicalHistoryRepository : Repository<MedicalHistory>
    {
        private readonly DatabaseContext context;
        public MedicalHistoryRepository(DatabaseContext context) : base(context)
        {
            this.context = context;
        }

        public override MedicalHistory Get(Guid id)
            => context.Set<MedicalHistory>().Include(x => x.Appointments).FirstOrDefault(x => x.Id == id);
    }
}
