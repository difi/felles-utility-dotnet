using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.Felles.Utility.Tester.Validation
{
    [TestClass]
    public class XmlValidatorTests
    {
        [TestClass]
        public class ValidateMethod : XmlValidatorTests
        {
            [TestMethod]
            public void ValidateWithCorrectXmlShouldReturnTrueAndGiveNoValidationMessages()
            {
                var validTestRequest = new TestGenerator.ValidTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();

                var validateResult = validator.Validate(validTestRequest.Input());

                Assert.IsTrue(validateResult);
                Assert.AreEqual(0, validator.ValidationMessages.Count);
            }

            [TestMethod]
            public void ValidateWithInvalidContentShouldReturnFalseAndGiveValidationError()
            {
                var invalidTestRequest = new TestGenerator.InvalidContentTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();

                var validateResult = validator.Validate(invalidTestRequest.Input());

                Assert.IsTrue(invalidTestRequest.ExpectedValidationMessages.Contains(validator.ValidationMessages.ToString()));
                Assert.IsFalse(validateResult);
            }

            [TestMethod]
            public void ValidateWithInvalidSyntaxShouldReturnFalseAndGiveValidationError()
            {
                var invalidSyntaxTestRequest = new TestGenerator.InvalidSyntaxTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();

                var validateResult = validator.Validate(invalidSyntaxTestRequest.Input());

                Assert.IsTrue(invalidSyntaxTestRequest.ExpectedValidationMessages.Contains(validator.ValidationMessages.ToString()));
                Assert.IsFalse(validateResult);
            }

            [TestMethod]
            public void MultipleValidateShouldNotHoldValidationState()
            {
                XmlValidator validator = new XmlValidatorTestImplementation();

                var invalidTestRequest = new TestGenerator.InvalidContentTestCouple();
                var validateResult = validator.Validate(invalidTestRequest.Input());
                Assert.IsFalse(validateResult);
                Assert.IsTrue(invalidTestRequest.ExpectedValidationMessages.Contains(validator.ValidationMessages.ToString()));

                var validTestRequest = new TestGenerator.ValidTestCouple();
                validateResult = validator.Validate(validTestRequest.Input());
                Assert.IsTrue(validateResult);
                Assert.AreEqual(0, validator.ValidationMessages.Count);

                var invalidSyntaxTestRequest = new TestGenerator.InvalidSyntaxTestCouple();
                validateResult = validator.Validate(invalidSyntaxTestRequest.Input());
                Assert.IsFalse(validateResult);
                Assert.IsTrue(invalidSyntaxTestRequest.ExpectedValidationMessages.Contains(validator.ValidationMessages.ToString()));
            }
        }
    }
}