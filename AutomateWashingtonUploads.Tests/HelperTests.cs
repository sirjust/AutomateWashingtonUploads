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
        public void ConvertDataToStringList_ShouldReturnListWithCountSameAsRows()
        {
            // this method may be currently untestable

            // arrange
            // var data = TestData.GetMockRawData();

            // act
            // var actual = Helper.ConvertDataToStringList(data);

            // assert
        }
    }
}
