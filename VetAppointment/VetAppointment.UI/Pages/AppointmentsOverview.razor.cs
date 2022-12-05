using VetAppointment.UI.Pages.Services;

namespace VetAppointment.UI.Pages
{
    public partial class AppointmentsOverview : ComponentBase
    {
        [Inject]
        public IAppointmentDataService AppointmentDataService { get; set; }

        public List<Appointment> Appointments { get; set; } = default!;
        protected async override Task OnInitializedAsync()
        {
            Appointments = (await AppointmentDataService.GetAllAppointments()).ToList();
        }
    }
}
