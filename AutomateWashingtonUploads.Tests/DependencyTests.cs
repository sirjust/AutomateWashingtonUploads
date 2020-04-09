using AutomateWashingtonUploads.Dependency;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace AutomateWashingtonUploads.Tests
{
    [TestClass]
    public class DependencyTests
    {
        [TestMethod]
        [Ignore]
        public void Kernel_Can_Instantiate_IUploader()
        {
            // Arrange
            var kernel = new StandardKernel(new DependencyContainer());

            // Act
            var uploader = kernel.Get<IUploader>();

            // Assert
            Assert.IsNotNull(uploader);
        }
    }
}
