using VetAppointment.UI.Pages.Services;

namespace VetAppointment.UI.Pages
{
    public partial class PetOwnersOverview : ComponentBase
    {
        [Inject]
        public IPetOwnerDataService PetOwnerDataService { get; set; }

        public List<PetOwner> PetOwners { get; set; } = default!;
        protected async override Task OnInitializedAsync()
        {
            PetOwners = (await PetOwnerDataService.GetAllPetOwners()).ToList();

            PetOwners.ForEach(p => { Console.WriteLine(p.Id + " " + p.Name); });
        }

        //protected async override Task OnInitializedAsync()
        //{
        //    Clinics = (await VetClinicDataService.GetAllClinics()).ToList();
        //}
    }
}