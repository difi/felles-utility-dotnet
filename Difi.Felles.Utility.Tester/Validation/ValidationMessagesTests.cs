using System.Xml.Schema;
using Difi.Felles.Utility.Validation;
using Xunit;

namespace Difi.Felles.Utility.Tester.Validation
{
    public class ValidationMessagesTests
    {
        public class AddErrorMethod : ValidationMessagesTests
        {
            [Fact]
            public void ErrorMessageIsAdded()
            {
                var messages = new ValidationMessages();
                const string expectedError = "ErrorMessage";
                messages.Add(XmlSeverityType.Error, expectedError);

                Assert.Equal(1, messages.Count);
                Assert.Same(expectedError, messages.ToString());
                Assert.True(messages.HasErrors);
                Assert.False(messages.HasWarnings);
                Assert.NotNull(messages);
            }
        }

        public class AddWarningMethod : ValidationMessagesTests
        {
            [Fact]
            public void WarningMessageIsAdded()
            {
                var messages = new ValidationMessages();
                const string expectedError = "WarningMessage";
                messages.Add(XmlSeverityType.Warning, expectedError);

                Assert.Equal(1, messages.Count);
                Assert.Same(expectedError, messages.ToString());
                Assert.False(messages.HasErrors);
                Assert.True(messages.HasWarnings);
                Assert.NotNull(messages);
            }
        }

        public class ToStringMethod : ValidationMessagesTests
        {
            [Fact]
            public void OutputsCorrectly()
            {
                var messages = new ValidationMessages();
                const string expectedError = "Error";

                messages.Add(XmlSeverityType.Error, expectedError);

                Assert.Same(expectedError, messages.ToString());
            }
        }
    }
}