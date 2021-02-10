using AutomateWashingtonUploads.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace AutomateWashingtonUploads.Tests
{
    [TestClass]
    public class HelperTests
    {
        IValidationHelper _helper;

        [TestInitialize]
        public void Setup()
        {
            var logger = new Mock<ILogger>().Object;
            _helper = new ValidationHelper(logger);
        }

        [TestMethod]
        public void ChangeSecondToLastCharacter_ShouldChangeCharacterToO()
        {
            // arrange
            var expected = "ABCD*EF864O5";

            // act
            var result = _helper.ChangeSecondToLastCharacter("ABCD*EF86405");

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CheckForZero_ShouldReturnSameLicense_WhenLicenseCorrect()
        {
            // arrange
            var original = "ABCDEFGHIOOO";

            // act
            var actual = _helper.CheckForZero(original);

            // assert
            Assert.AreEqual(original, actual);
        }

        [TestMethod]
        public void CheckForZero_ReturnsOriginal_WhenLicenseIncorrectLength()
        {
            // arrange
            var original = "ABCDEF";

            // act
            var actual = _helper.CheckForZero(original);

            // assert
            Assert.AreEqual(original, actual);
        }

        [TestMethod]
        public void IsLicenseTwelveCharacters_ShouldReturnFalseIfWrongAmount()
        {
            // arrange
            var expected = false;

            // act
            var result1 = ValidationHelper.IsLicenseTwelveCharacters("1111111111111111");
            var result2 = ValidationHelper.IsLicenseTwelveCharacters("1111");

            // assert
            Assert.AreEqual(expected, result1);
            Assert.AreEqual(expected, result2);
        }

        [TestMethod]
        public void IsLicenseTwelveCharacters_ShouldReturnTrueIfCorrectLength()
        {
            // arrange
            var expected = true;

            // act
            var result1 = ValidationHelper.IsLicenseTwelveCharacters("111111111111");
            var result2 = ValidationHelper.IsLicenseTwelveCharacters("000000000000");

            // assert
            Assert.AreEqual(expected, result1);
            Assert.AreEqual(expected, result2);
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
            var actual = DataHelper.ListToCompletionList(rawCompletions).FirstOrDefault();
            // assert
            Assert.AreEqual(expected.Course, actual.Course);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.License, actual.License);
            Assert.AreEqual(expected.Date, actual.Date);
        }
    }
}
