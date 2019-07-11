using System;
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
    }
}
