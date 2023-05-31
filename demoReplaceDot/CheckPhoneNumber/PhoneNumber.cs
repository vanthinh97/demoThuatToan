using System.Text.RegularExpressions;

namespace CheckPhoneNumber
{
    public class PhoneNumber
    {
        // Regular expression used to validate a phone number.
        public const string motif = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

        public static bool IsPhoneNbr(string number)
        {
            if (number != null) return Regex.IsMatch(number, motif);
            else return false;
        }
    }
}
