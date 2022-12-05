namespace VetAppointment.UI.Pages.Services
{
    public interface IAppointmentDataService
    {
        Task<Appointment> AddAppointment(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAllAppointments();
    }
}