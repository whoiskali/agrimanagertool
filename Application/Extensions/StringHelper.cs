using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    internal static class StringHelper
    {
        public static string GenerateLink(int userId)
        {
            var random = new Random(userId);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var a = new string(Enumerable.Repeat(chars, 20)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            var b = new string(Enumerable.Repeat(chars, 20)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            var z = a + userId.ToString() + b;
            return z;
        }

        public static int GetAge(this DateTime? birthdate)
        {
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - birthdate?.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (birthdate?.Date > today.AddYears(-age??0)) age--;
            return age??0;
        }
    }
}
