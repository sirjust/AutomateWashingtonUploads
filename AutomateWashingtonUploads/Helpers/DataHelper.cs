using System;
using System.Collections.Generic;

namespace AutomateWashingtonUploads.Helpers
{
    public static class DataHelper
    {
        public static IEnumerable<Completion> ListToCompletionList(IEnumerable<string> stringList)
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
                Completion completion = new Completion
                {
                    Course = dividedStrings[i][0],
                    Date = dividedStrings[i][1],
                    License = dividedStrings[i][2],
                    Name = dividedStrings[i][3]
                };
                myCompletionList.Add(completion);
            }
            return myCompletionList;
        }

        public static IEnumerable<string> ConvertDataToStringList()
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
    }
}
