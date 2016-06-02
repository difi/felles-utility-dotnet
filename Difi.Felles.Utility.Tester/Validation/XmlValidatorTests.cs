using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.Felles.Utility.Tester.Validation
{
    [TestClass]
    public class XmlValidatorTests
    {
        [TestMethod]
        public void ValidateWithCorrectXmlShouldReturnTrueAndGiveNoWarnings()
        {
            XmlValidator validator = new TestGenerator.XmlValidatorImpl();

            var validateResult = validator.Validate(TestGenerator.ValidTestRequest());

            Assert.IsTrue(validateResult);
            Assert.AreEqual(0, validator.ValidationMessages.Count());
        }

        [TestMethod]
        public void ValidateWithInvalidXmlShouldReturnFalseAndGiveValidationWarning()
        {
            var expectedWarningList = TestGenerator.ExpectedWarningList();
            XmlValidator validator = new TestGenerator.XmlValidatorImpl();

            var validateResult = validator.Validate(TestGenerator.TestRequestWithInvalidSsn());

            Assert.IsTrue(expectedWarningList.Contains(validator.ValidationMessages.ToString()));
            Assert.IsFalse(validateResult);
        }

        [TestMethod]
        public void MultipleValidateShouldNotHoldValidationState()
        {
            var expectedWarningList = TestGenerator.ExpectedWarningList();
            XmlValidator validator = new TestGenerator.XmlValidatorImpl();

            var validateResult = validator.Validate(TestGenerator.TestRequestWithInvalidSsn());
            Assert.IsFalse(validateResult);
            Assert.IsTrue(expectedWarningList.Contains(validator.ValidationMessages.ToString()));

            validateResult = validator.Validate(TestGenerator.ValidTestRequest());
            Assert.IsTrue(validateResult);
            Assert.AreEqual(0, validator.ValidationMessages.Count());

            validateResult = validator.Validate(TestGenerator.TestRequestWithInvalidSyntax());
            Assert.IsFalse(validateResult);
            Assert.IsFalse(expectedWarningList.Contains(validator.ValidationMessages.ToString()));
        }
    }
}