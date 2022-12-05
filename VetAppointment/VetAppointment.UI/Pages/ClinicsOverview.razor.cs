using Microsoft.AspNetCore.Components;
using VetAppointment.Shared.Domain;
using VetAppointment.UI.Pages.Services;

namespace VetAppointment.UI.Pages
{
    public partial class ClinicsOverview : ComponentBase
    {
        [Inject]
        public IVetClinicDataService VetClinicDataService { get; set; }
        public List<VetClinic> Clinics { get; set; } = default!;
        public VetClinic Clinic { get; set; } = default!;
        public VetClinic ClinicToGet { get; set; } = default!;

        public Vet Vet { get; set; } = default!;

        public string Id { get; set; } = default!;
        public bool IsCreating { get; set; } = false;
        public bool IsRetrieving { get; set; } = false;

        public bool IsVetBeingAdded { get; set; } = false;

        protected async override Task OnInitializedAsync()
        {
            Clinics = (await VetClinicDataService.GetAllClinics()).ToList();
        }

        protected async Task CreateClinic()
        {
            if (Clinic.Name != null && Clinic.ContactEmail != null && Clinic.ContactPhone != null
                && Clinic.Address != null && Clinic.NumberOfPlaces != 0)
                if (IsCreating)
                {
                    Clinic = await VetClinicDataService.AddClinic(Clinic);
                }
        }

        protected void StartCreateClinic()
        {
            IsCreating = !IsCreating;
            if (IsCreating)
            {
                Clinic = new();
            }
            else
            {
                Clinic = default!;
            }
        }

        protected async Task AddVetToClinic()
        {
            if (IsVetBeingAdded)
            {
                await VetClinicDataService.AddVetToClinic(Guid.Parse(Vet.ClinicId), Vet);
            }
        }

        protected void StartAddVetToClinic()
        {
            IsVetBeingAdded = !IsVetBeingAdded;
            if (IsVetBeingAdded)
            {
                Vet = new();
            }
            else
            {
                Vet = default!;
            }
        }

        protected async Task GetById()
        {
            if (IsRetrieving)
            {
                ClinicToGet = await VetClinicDataService.GetClinicById(Guid.Parse(Id));
            }
        }

        protected void ShowCreate()
        {
            Clinic = new();
            IsCreating = true;
        }

        protected void StartRetrieving()
        {
            /*IsRetrieving = !IsRetrieving;
            if (IsCreating)
            {
                Id = "";
            }
            else
            {
                Id = default!;
            }*/
            IsRetrieving = true;
            Id = "";
        }
    }
}