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
                var validTestCouple = new TestGenerator.ValidTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();

                var validateResult = validator.Validate(validTestCouple.Input());

                Assert.IsTrue(validateResult);
                Assert.AreEqual(0, validator.ValidationMessages.Count);
            }

            [TestMethod]
            public void ValidateWithInvalidContentShouldReturnFalseAndGiveValidationError()
            {
                var invalidContentTestCouple = new TestGenerator.InvalidContentTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();

                var validateResult = validator.Validate(invalidContentTestCouple.Input());

                Assert.IsTrue(invalidContentTestCouple.ExpectedValidationMessages.Contains(validator.ValidationMessages.ToString()));
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

                var invalidContentTestCouple = new TestGenerator.InvalidContentTestCouple();
                var validateResult = validator.Validate(invalidContentTestCouple.Input());
                Assert.IsFalse(validateResult);
                Assert.IsTrue(invalidContentTestCouple.ExpectedValidationMessages.Contains(validator.ValidationMessages.ToString()));

                var validTestCouple = new TestGenerator.ValidTestCouple();
                validateResult = validator.Validate(validTestCouple.Input());
                Assert.IsTrue(validateResult);
                Assert.AreEqual(0, validator.ValidationMessages.Count);

                var invalidSyntaxTestCouple = new TestGenerator.InvalidSyntaxTestCouple();
                validateResult = validator.Validate(invalidSyntaxTestCouple.Input());
                Assert.IsFalse(validateResult);
                Assert.IsTrue(invalidSyntaxTestCouple.ExpectedValidationMessages.Contains(validator.ValidationMessages.ToString()));
            }
        }
    }
}