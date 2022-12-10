using System.ComponentModel.DataAnnotations;

namespace VetAppointment.Shared.Domain
{
    public class VetClinic
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Enter a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter an address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Enter number of places")]
        [Range(1, 999999999, ErrorMessage = "Enter a valid number of places")]
        public int NumberOfPlaces { get; set; }

        [Required(ErrorMessage = "Enter an email")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Enter a valid email")]
        public string ContactEmail { get; set; }

        [Required(ErrorMessage = "Enter a phone number")]
        [RegularExpression(@"^\+40[0-9]{9,}$", ErrorMessage = "Enter a valid phone number")]
        public string ContactPhone { get; set; }
    }
}
