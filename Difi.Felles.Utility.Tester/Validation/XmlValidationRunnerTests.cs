using Difi.Felles.Utility.Validation;
using Xunit;

namespace Difi.Felles.Utility.Tester.Validation
{
    public class XmlValidationRunnerTests
    {
        public class ConstructorMethod : XmlValidationRunnerTests
        {
            [Fact]
            public void SimpleInitialization()
            {
                //Arrange
                var xmlSchemaSet = TestGenerator.XmlSchemaSet();

                //Act
                var validationRunner = new XmlValidationRunner(xmlSchemaSet);

                //Assert
                Assert.Equal(xmlSchemaSet, validationRunner.XmlSchemaSet);
            }
        }

        public class ValidateMethod : ValidationMessagesTests
        {
            [Fact]
            public void AddsValidationMessage()
            {
                //Arrange
                var validationRunner = new XmlValidationRunner(TestGenerator.XmlSchemaSet());
                var invalidTestCouple = new TestGenerator.InvalidContentTestCouple();

                //Act
                validationRunner.Validate(invalidTestCouple.Input(), GetType().GUID);

                //Assert
                Assert.Equal(1, validationRunner.ValidationMessages.Count);
            }
        }
    }
}