using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateWashingtonUploads
{
    public static class PlumbingCourses
    {
        public static string[] WAPlumbingCourses = {
            "WA2018-130",
            "WA2018-398",
            "WA2016-355",
            "WA2016-696",
            "WA2016-695",
            "WA2016-509",
            "WA2017-654",
            "WA2017-380",
            "WA2016-286",
            "WA2016-215",
            "WA2016-587",
            "WA2016-356",
            "WA2016-491",
            "WA2016-284",
            "WA2017-004",
            "WA2017-116",
            "WA2017-652",
            "WA2016-285",
            "WA2016-240",
            "WA2018-184",
            "WA2018-185",
            "WA2016-346",
            "WA2016-741",
            "WA2016-740",
            "WA2016-323",
            "WA2017-611",
            "WA2019-159",
            "WA2019-158",
            "WA2019-156",
            "WA2019-157",
            "WA2019-155"
        };

        public static Dictionary<string, string> Old_New_Courses = new Dictionary<string, string>{
            {"WA2016-323", "WA2019-159" },
            {"WA2016-1", "WA2019-158" }, // electrical review
            {"WA2016-2", "WA2019-156" }, // 2015 UPC Chapters 2 - 6 Questions
            {"WA2016-3", "WA2019-157" }, // 2015 UPC Chapters 7, 8, 9 Questions
            {"WA2016-4", "WA2019-155" } // 2015 UPC Chapter 6 Waer Systems Part 1
        };
    }
}
