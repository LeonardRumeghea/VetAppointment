using VetAppointment.UI.Pages.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<IPetDataService, PetDataService>
    (
        client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    );
builder.Services.AddHttpClient<IVetClinicDataService, VetClinicDataService>
    (
        client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    );
builder.Services.AddHttpClient < IPetOwnerDataService, PetOwnerDataService>
    (
        client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    );
builder.Services.AddHttpClient<IAppointmentDataService, AppointmentDataService>
    (
        client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    );

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
