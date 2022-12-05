using Microsoft.AspNetCore.Components;
using VetAppointment.Shared.Domain;
using VetAppointment.UI.Pages.Services;

namespace VetAppointment.UI.Pages
{
    public partial class PetsOverview : ComponentBase
    {
        [Inject]
        public IPetDataService PetDataService { get; set; }

        public List<Pet> Pets { get; set; } = default!;
        protected async override Task OnInitializedAsync()
        {
            Pets = (await PetDataService.GetAllPets()).ToList();
        }
    }
}