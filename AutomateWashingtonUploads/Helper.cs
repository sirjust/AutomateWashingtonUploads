using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateWashingtonUploads
{
    public static class Helper
    {

        public static List<Completion> ListToCompletionList(List<string> stringList)
        {
            List<string[]> dividedStrings = new List<string[]>();
            List<Completion> myCompletionList = new List<Completion>();
            foreach (string rawCompletion in stringList)
            {
                string[] itemArray = rawCompletion.Split('|');
                for (int i = 0; i < itemArray.Length; i++)
                {
                    itemArray[i] = itemArray[i].Trim();
                }
                dividedStrings.Add(itemArray);
            }

            for (int i = 0; i < dividedStrings.Count - 1; i++)
            {
                Completion completion = new Completion();
                completion.Course = dividedStrings[i][0];
                completion.Date = dividedStrings[i][1];
                completion.License = dividedStrings[i][2];
                completion.Name = dividedStrings[i][3];
                myCompletionList.Add(completion);
            }
            return myCompletionList;
        }

        public static List<string> ConvertDataToStringList()
        {
            string s = "1";
            List<string> result = new List<string> { };
            while (!string.IsNullOrEmpty(s))
            {
                s = Console.ReadLine();
                result.Add(s);
            }
            return result;
        }

        public static string ChangeSecondToLastCharacter(string s)
        {
            char[] license = s.ToCharArray();
            license[10] = 'O';
            return new string(license);
        }

        public static bool IsLicenseTwelveCharacters(string license)
        {
            if (license.Length == 12) return true;
            return false;
        }
    }
}
