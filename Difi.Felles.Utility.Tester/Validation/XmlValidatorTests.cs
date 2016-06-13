using System.Collections.Generic;
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
            public void ValidateReturnsBoolAndOutsString()
            {
                var invalidContentTestCouple = new TestGenerator.InvalidContentTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();
                string validationMessage;
                
                var status = validator.Validate(invalidContentTestCouple.Input(), out validationMessage);

                Assert.IsTrue(invalidContentTestCouple.ExpectedValidationMessages.Contains(validationMessage));
                Assert.IsFalse(status);
            }

            [TestMethod]
            public void ValidateReturnsBoolAndOutsListOfStrings()
            {
                var invalidContentTestCouple = new TestGenerator.InvalidContentTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();
                List<string> validationMessages;
                
                var status = validator.Validate(invalidContentTestCouple.Input(), out validationMessages);

                Assert.IsTrue(invalidContentTestCouple.ExpectedValidationMessages.Contains(validationMessages.ToString()));
                Assert.IsFalse(status);
            }

            [TestMethod]
            public void ValidateReturnsBool()
            {
                var invalidContentTestCouple = new TestGenerator.InvalidContentTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();
                
                var status = validator.Validate(invalidContentTestCouple.Input());

                Assert.IsFalse(status);
            }

            [TestMethod]
            public void ValidateWithCorrectXmlShouldReturnTrueAndGiveNoValidationMessages()
            {
                var validTestCouple = new TestGenerator.ValidTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();
                List<string> messagesList;

                var validateResult = validator.Validate(validTestCouple.Input(), out messagesList);

                Assert.IsTrue(validateResult);
                Assert.AreEqual(0, messagesList.Count);
            }

            [TestMethod]
            public void ValidateWithInvalidContentShouldReturnFalseAndGiveValidationError()
            {
                var invalidContentTestCouple = new TestGenerator.InvalidContentTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();
                List<string> messagesList;
                var validateResult = validator.Validate(invalidContentTestCouple.Input(), out messagesList);

                Assert.IsTrue(invalidContentTestCouple.ExpectedValidationMessages.Contains(messagesList.ToString()));
                Assert.IsFalse(validateResult);
            }

            [TestMethod]
            public void ValidateWithInvalidSyntaxShouldReturnFalseAndGiveValidationError()
            {
                var invalidSyntaxTestRequest = new TestGenerator.InvalidSyntaxTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();
                List<string> messagesList;

                var validateResult = validator.Validate(invalidSyntaxTestRequest.Input(), out messagesList);

                Assert.IsTrue(invalidSyntaxTestRequest.ExpectedValidationMessages.Contains(messagesList.ToString()));
                Assert.IsFalse(validateResult);
            }

            [TestMethod]
            public void MultipleValidateShouldNotHoldValidationState()
            {
                XmlValidator validator = new XmlValidatorTestImplementation();

                var invalidContentTestCouple = new TestGenerator.InvalidContentTestCouple();
                List<string> messagesList;
                var validateResult = validator.Validate(invalidContentTestCouple.Input(), out messagesList);
                Assert.IsFalse(validateResult);
                Assert.IsTrue(invalidContentTestCouple.ExpectedValidationMessages.Contains(messagesList.ToString()));

                var validTestCouple = new TestGenerator.ValidTestCouple();
                validateResult = validator.Validate(validTestCouple.Input(), out messagesList);
                Assert.IsTrue(validateResult);
                Assert.AreEqual(0, messagesList.Count);

                var invalidSyntaxTestCouple = new TestGenerator.InvalidSyntaxTestCouple();
                validateResult = validator.Validate(invalidSyntaxTestCouple.Input(), out messagesList);
                Assert.IsFalse(validateResult);
                Assert.IsTrue(invalidSyntaxTestCouple.ExpectedValidationMessages.Contains(messagesList.ToString()));
            }
        }
    }
}