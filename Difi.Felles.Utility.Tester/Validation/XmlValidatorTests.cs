using System.Collections.Generic;
using Xunit;

namespace Difi.Felles.Utility.Tester.Validation
{
    
    public class XmlValidatorTests
    {
        
        public class ValidateMethod : XmlValidatorTests
        {
            [Fact]
            public void ValidateReturnsBoolAndOutsString()
            {
                var invalidContentTestCouple = new TestGenerator.InvalidContentTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();
                string validationMessage;
                
                var status = validator.Validate(invalidContentTestCouple.Input(), out validationMessage);

                Assert.True(invalidContentTestCouple.ExpectedValidationMessages.Contains(validationMessage));
                Assert.False(status);
            }

            [Fact]
            public void ValidateReturnsBoolAndOutsListOfStrings()
            {
                var invalidContentTestCouple = new TestGenerator.InvalidContentTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();
                List<string> validationMessages;
                
                var status = validator.Validate(invalidContentTestCouple.Input(), out validationMessages);

                Assert.True(invalidContentTestCouple.ExpectedValidationMessages.Contains(validationMessages.ToString()));
                Assert.False(status);
            }

            [Fact]
            public void ValidateReturnsBool()
            {
                var invalidContentTestCouple = new TestGenerator.InvalidContentTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();
                
                var status = validator.Validate(invalidContentTestCouple.Input());

                Assert.False(status);
            }

            [Fact]
            public void ValidateWithCorrectXmlShouldReturnTrueAndGiveNoValidationMessages()
            {
                var validTestCouple = new TestGenerator.ValidTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();
                List<string> messagesList;

                var validateResult = validator.Validate(validTestCouple.Input(), out messagesList);

                Assert.True(validateResult);
                Assert.Equal(0, messagesList.Count);
            }

            [Fact]
            public void ValidateWithInvalidContentShouldReturnFalseAndGiveValidationError()
            {
                var invalidContentTestCouple = new TestGenerator.InvalidContentTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();
                List<string> messagesList;
                var validateResult = validator.Validate(invalidContentTestCouple.Input(), out messagesList);

                Assert.True(invalidContentTestCouple.ExpectedValidationMessages.Contains(messagesList.ToString()));
                Assert.False(validateResult);
            }

            [Fact]
            public void ValidateWithInvalidSyntaxShouldReturnFalseAndGiveValidationError()
            {
                var invalidSyntaxTestRequest = new TestGenerator.InvalidSyntaxTestCouple();
                XmlValidator validator = new XmlValidatorTestImplementation();
                List<string> messagesList;

                var validateResult = validator.Validate(invalidSyntaxTestRequest.Input(), out messagesList);

                Assert.True(invalidSyntaxTestRequest.ExpectedValidationMessages.Contains(messagesList.ToString()));
                Assert.False(validateResult);
            }

            [Fact]
            public void MultipleValidateShouldNotHoldValidationState()
            {
                XmlValidator validator = new XmlValidatorTestImplementation();

                var invalidContentTestCouple = new TestGenerator.InvalidContentTestCouple();
                List<string> messagesList;
                var validateResult = validator.Validate(invalidContentTestCouple.Input(), out messagesList);
                Assert.False(validateResult);
                Assert.True(invalidContentTestCouple.ExpectedValidationMessages.Contains(messagesList.ToString()));

                var validTestCouple = new TestGenerator.ValidTestCouple();
                validateResult = validator.Validate(validTestCouple.Input(), out messagesList);
                Assert.True(validateResult);
                Assert.Equal(0, messagesList.Count);

                var invalidSyntaxTestCouple = new TestGenerator.InvalidSyntaxTestCouple();
                validateResult = validator.Validate(invalidSyntaxTestCouple.Input(), out messagesList);
                Assert.False(validateResult);
                Assert.True(invalidSyntaxTestCouple.ExpectedValidationMessages.Contains(messagesList.ToString()));
            }
        }
    }
}