using System.Collections.Generic;

namespace AutomateWashingtonUploads.Tests
{
    public static class TestData
    {
        public static List<string> GetMockCompletions()
        {
            return new List<string> {
                "WA2016-240 | 2019-03-19 | OOOOOOOOOOOO | Test1, Test1",
                "WA2016-286 | 2019-03-19 | OOOOOOOOOOOO | Test2, Test2",
                "WA2016-323 | 2019-03-19 | OOOOOOOOOOOO | Test3, Test3",
                "WA2016-323 | 2019-03-19 | OOOOOOOOOOOO | Test4, Test4",
                "WA2016-491 | 2019-03-19 | OOOOOOOOOOOO | Test5, Test5",
                "WA2016-509 | 2019-03-19 | OOOOOOOOOOOO | Test6, Test6"
            };
        }
    }
}
