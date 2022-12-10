using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;

namespace VetAppointment.Infrastructure.Repositories.GenericRepositories
{
    public class AppointmentRepository : Repository<Appointment>
    {
        public AppointmentRepository(DatabaseContext context) : base(context) { }
    }
}
