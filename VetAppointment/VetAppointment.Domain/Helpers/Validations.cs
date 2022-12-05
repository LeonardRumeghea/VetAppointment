namespace VetAppointment.Domain.Helpers
{
    public class Validations
    {
        public static bool IsValidEmail(string email)
        {
            email = email.Trim();

            if (email.EndsWith("."))
            {
                return false;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (phoneNumber == null)
            {
                return false;
            }

            phoneNumber = phoneNumber.Trim();

            if (phoneNumber.StartsWith("+"))
            {
                phoneNumber = phoneNumber[1..];
                return phoneNumber.Length == 11 && phoneNumber.All(char.IsDigit) && phoneNumber.StartsWith("40");
            }

            return false;
        }
    }
}