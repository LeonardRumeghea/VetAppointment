using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.Infrastructure.Repositories.GenericRepositories
{
    public class MedicalHistoryRepository : Repository<MedicalHistory>
    {
        public MedicalHistoryRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
