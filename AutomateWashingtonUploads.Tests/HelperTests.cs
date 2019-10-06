using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomateWashingtonUploads.Tests
{
    [TestClass]
    public class HelperTests
    {
        [TestMethod]
        public void ChangeSecondToLastCharacter_ShouldChangeCharacterToO()
        {
            // arrange
            var expected = "WONG*JK864O5";

            // act
            var result = Helper.ChangeSecondToLastCharacter("WONG*JK86405");

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ListToCompletionList_ShouldReturnListWithCorrectElements()
        {
            // arrange
            Completion expected = new Completion();
            expected.Course = "WA2016-240";
            expected.Name = "Test1, Test1";
            expected.License = "OOOOOOOOOOOO";
            expected.Date = "2019-03-19";

            // act
            var rawCompletions = TestData.GetMockCompletions();
            var actual = Helper.ListToCompletionList(rawCompletions)[0];
            // assert
            Assert.AreEqual(expected.Course, actual.Course);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.License, actual.License);
            Assert.AreEqual(expected.Date, actual.Date);
        }
    }
}
