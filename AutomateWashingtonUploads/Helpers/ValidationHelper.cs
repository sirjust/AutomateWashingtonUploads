using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateWashingtonUploads.Helpers
{
    public static class ValidationHelper
    {
        public static string ChangeSecondToLastCharacter(string s)
        {
            char[] license = s.ToCharArray();
            license[10] = 'O';
            return new string(license);
        }

        public static bool IsLicenseTwelveCharacters(string license) => license.Length == 12;
    }
}