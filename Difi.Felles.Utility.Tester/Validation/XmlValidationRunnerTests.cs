using Difi.Felles.Utility.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.Felles.Utility.Tester.Validation
{
    [TestClass]
    public class XmlValidationRunnerTests
    {
        [TestClass]
        public class ConstructorMethod : XmlValidationRunnerTests
        {
            [TestMethod]
            public void SimpleInitialization()
            {
                //Arrange
                var xmlSchemaSet = TestGenerator.XmlSchemaSet();

                //Act
                var validationRunner = new XmlValidationRunner(xmlSchemaSet);

                //Assert
                Assert.AreEqual(xmlSchemaSet, validationRunner.XmlSchemaSet);
            }
        }

        [TestClass]
        public class ValidateMethod : ValidationMessagesTests
        {
            [TestMethod]
            public void AddsValidationMessage()
            {
                //Arrange
                var validationRunner = new XmlValidationRunner(TestGenerator.XmlSchemaSet());
                var invalidTestCouple = new TestGenerator.InvalidContentTestCouple();

                //Act
                validationRunner.Validate(invalidTestCouple.Input());

                //Assert
                Assert.AreEqual(1, validationRunner.ValidationMessages.Count);
            }
        }
    }
}