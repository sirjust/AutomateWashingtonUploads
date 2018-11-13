using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AutomateWashingtonUploads
{
    public class ConvertDataToObjectList
    {
        public List<string> ConvertDataToStringList ()
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

    public class Completion
    {
        public string course;
        public string date;
        public string license;
        public string name;

        public List<Completion> listToCompletionList(List<string> stringList)
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

            for(int i = 0; i< dividedStrings.Count-1; i++)
            {
                Completion completion = new Completion();
                completion.course = dividedStrings[i][0];
                completion.date = dividedStrings[i][1];
                completion.license = dividedStrings[i][2];
                completion.name = dividedStrings[i][3];
                myCompletionList.Add(completion);
            }
            return myCompletionList;
        }

    }
}
