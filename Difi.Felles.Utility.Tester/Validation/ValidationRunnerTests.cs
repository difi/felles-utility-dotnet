using Difi.Felles.Utility.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.Felles.Utility.Tester.Validation
{
    [TestClass]
    public class ValidationRunnerTests
    {
        [TestClass]
        public class ConstructorMethod : ValidationRunnerTests
        {
            [TestMethod]
            public void SimpleInitialization()
            {
                //Arrange
                var validationRunner = new ValidationRunner(TestGenerator.XmlSchemaSet());
                
            }
        }

        [TestClass]
        public class ValidateMethod : ValidationMessagesTests
        {
            [TestMethod]
            public void AddsValidationMessage()
            {
                //Arrange
                var validationRunner = new ValidationRunner(TestGenerator.XmlSchemaSet());
                var invalidTestCouple = new TestGenerator.InvalidContentTestCouple();

                //Act
                validationRunner.Validate(invalidTestCouple.Input());

                //Assert
                Assert.AreEqual(1, validationRunner.ValidationMessages.Count);
            }
        }
    }
}