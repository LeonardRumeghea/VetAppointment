using Microsoft.EntityFrameworkCore;
using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.Infrastructure.Repositories.GenericRepositories
{
    public class MedicalHistoryRepository : Repository<MedicalHistory>
    {
        private readonly DatabaseContext _databaseContext;
        public MedicalHistoryRepository(DatabaseContext context) : base(context)
        {
            _databaseContext = context;
        }

        public override MedicalHistory Get(Guid id) => _databaseContext.Set<MedicalHistory>()
            .Include(x => x.Appointments)
            .SingleOrDefault(x => x.Id == id);

        public override IEnumerable<MedicalHistory> All() => _databaseContext.Set<MedicalHistory>()
            .Include(x => x.Appointments)
            .ToList();
    }
}
