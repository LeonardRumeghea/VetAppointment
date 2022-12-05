using VetAppointment.Domain.Helpers;

#nullable disable
namespace VetAppointment.Domain
{
    public class VetClinic
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public int NumberOfPlaces { get; private set; }
        public string ContactEmail { get; private set; }
        public string ContactPhone { get; private set; }
        public DateTime RegistrationDate { get; private set; }
        public List<Pet> Pets { get; private set; }
        public List<Vet> Vets { get; private set; }
        public Guid MedicalHistoryId { get; private set; }

        public ClinicOwner Owner;

        public static Result<VetClinic> Create(string name, string address, int numberOfPlaces, string contactEmail,
            string contactPhone)
        {
            var minimumNumberOfPlaces = 0;
            if (numberOfPlaces <= 0)
            {
                return Result<VetClinic>.Failure($"The number of " +
                    $"places for the shelter needs to be greater than " +
                    $"'{minimumNumberOfPlaces}'");
            }

            if (!Validations.IsValidEmail(contactEmail))
            {
                return Result<VetClinic>.Failure($"Email {contactEmail} is not valid");
            }

            if (!Validations.IsValidPhoneNumber(contactPhone))
            {
                return Result<VetClinic>.Failure($"Phone number {contactPhone} is not valid");
            }

            var vetClinic = new VetClinic
            {
                Id = Guid.NewGuid(),
                Name = name,
                Address = address,
                ContactEmail = contactEmail,
                ContactPhone = contactPhone,
                RegistrationDate = DateTime.Now,
                NumberOfPlaces = numberOfPlaces,
                Pets = new List<Pet>(),
                Vets = new List<Vet>(),
            };

            return Result<VetClinic>.Success(vetClinic);
        }

        public void AttachMedicalHistory(Guid medicalHistoryId)
        {
            MedicalHistoryId = medicalHistoryId;
        }

        public Result RegisterPetsFamilyToClinic(List<Pet> pets)
        {
            if (!pets.Any())
            {
                return Result.Failure("Register at least a pet to the clinic");
            }

            if (pets.Count > GetAvailableNumberOfPlaces())
            {
                return Result.Failure($"The newly added pets number '{pets.Count}' " +
                    $"exceed the available number of places '{GetAvailableNumberOfPlaces()}'");
            }

            pets.ForEach(pet =>
            {
                pet.RegisterPetToClinic(this);
                Pets.Add(pet);
            });

            return Result.Success();
        }

        public Result RegisterVetToClinic(Vet vet)
        {
            vet.RegisterVetToClinic(this);
            Vets.Add(vet);

            return Result.Success();
        }

        public void ConnectToOwner(ClinicOwner owner) => Owner = owner;

        public int GetAvailableNumberOfPlaces() => NumberOfPlaces - Pets.Count;

        public Result<VetClinic> Update(string name, string address, int numberOfPlaces, string contactEmail, string contactPhone)
        {
            if (name != "" && name != null)
            {
                this.Name = name;
            }

            if (address != "" && address != null)
            {
                this.Address = address;
            }

            if ( numberOfPlaces <= 0)
            {
                return Result<VetClinic>.Failure($"The number of " +
                    $"places for the shelter needs to be greater than " +
                    $"'{0}'");
            }
            this.NumberOfPlaces = numberOfPlaces;

            if (contactEmail != null && contactEmail != "" && !Validations.IsValidEmail(contactEmail))
            {
                return Result<VetClinic>.Failure($"Email {contactEmail} is not valid");
            }
            this.ContactEmail = contactEmail;

            if (contactPhone != null && contactPhone != "" && !Validations.IsValidPhoneNumber(contactPhone))
            {
                return Result<VetClinic>.Failure($"Phone number {contactPhone} is not valid");
            }
            this.ContactPhone = contactPhone;


            return Result<VetClinic>.Success(this);
        }
    }
}
