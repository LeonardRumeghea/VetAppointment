using Microsoft.EntityFrameworkCore;
using VetAppointment.Application;
using VetAppointment.Domain;
using VetAppointment.Infrastructure.Data;
using VetAppointment.Infrastructure.Repositories.GenericRepositories;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<IDatabaseContext, DatabaseContext>();
builder.Services.AddDbContext<DatabaseContext>(
	options => options.UseSqlite("Data Source = VetAppointmentTest.db"));

//builder.Services.AddDbContext<DatabaseContext>(
//	options => options.UseSqlite(
//		builder.Configuration.GetConnectionString("TestConnection"), 
//		b => b.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName)
//		)
//	);

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IRepository<Appointment>, AppointmentRepository>();
builder.Services.AddScoped<IRepository<PetOwner>, PetOwnerRepository>();
builder.Services.AddScoped<IRepository<Pet>, PetRepository>();
builder.Services.AddScoped<IRepository<VetClinic>, VetClinicRepository>();
builder.Services.AddScoped<IRepository<Vet>, VetRepository>();
builder.Services.AddScoped<IRepository<Drug>, DrugRepository>();
builder.Services.AddScoped<IRepository<MedicalHistory>, MedicalHistoryRepository>();
builder.Services.AddScoped<IRepository<PrescribedDrug>, PrescribedDrugRepository>();
builder.Services.AddScoped<IRepository<Treatment>, TreatmentRepository>();

builder.Services.AddCors(options =>
{
	options.AddPolicy("clinicsCors", policy =>
	{
		policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
	});
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("clinicsCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
