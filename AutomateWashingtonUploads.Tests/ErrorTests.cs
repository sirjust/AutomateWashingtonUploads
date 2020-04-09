using AutomateWashingtonUploads.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomateWashingtonUploads.Tests
{
    [TestClass]
    public class ErrorTests
    {
        IErrorHelper _helper;
        [TestInitialize]
        public void Setup()
        {
            _helper = new ErrorHelper();
        }

        [TestMethod]
        public void HasInvalidLicense_Returns_True_When_License_Invalid()
        {
            // Arrange
            var text = "No Licenses Found.";

            // Act
            var actual = _helper.HasInvalidLicense(text);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void CourseOutOfDateRange_Returns_True_When_CourseDate_Invalid()
        {
            // Arrange
            var text = "The Completion Date is out of the class range.";

            // Act
            var actual = _helper.CourseOutOfDateRange(text);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void HasAlreadyUsedCourse_Returns_True_When_Tradesman_Already_Used_Course()
        {
            // Arrange
            var text = "has already used this class for this renewal.";

            // Act
            var actual = _helper.HasAlreadyUsedCourse(text);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void LienseAlreadyOnRoster_Returns_True_When_License_OnRoster()
        {
            // Arrange
            var text = "is already on this roster.";

            // Act
            var actual = _helper.LienseAlreadyOnRoster(text);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void CourseNumberNotFound_Returns_True_When_No_Course_Number()
        {
            // Arrange
            var text = "Invalid course identifier specified";

            // Act
            var actual = _helper.CourseNumberNotFound(text);

            // Assert
            Assert.IsTrue(actual);
        }
    }
}
