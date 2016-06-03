using System.Collections.Generic;
using System.Xml.Schema;
using Difi.Felles.Utility.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.Felles.Utility.Tester.Validation
{
    [TestClass]
    public class ValidationMessagesTests
    {
        [TestClass]
        public class AddErrorMethod : ValidationMessagesTests
        {
            [TestMethod]
            public void ErrorMessageIsAdded()
            {
                var messages = new ValidationMessages();
                const string expectedError = "ErrorMessage";
                messages.Add(XmlSeverityType.Error, expectedError);

                Assert.AreEqual(1, messages.Count);
                Assert.AreSame(expectedError, messages.ToString());
                Assert.IsTrue(messages.HasErrors);
                Assert.IsFalse(messages.HasWarnings);
                Assert.IsNotNull(messages);
            }
        }

        [TestClass]
        public class AddWarningMethod : ValidationMessagesTests
        {
            [TestMethod]
            public void WarningMessageIsAdded()
            {
                var messages = new ValidationMessages();
                const string expectedError = "WarningMessage";
                messages.Add(XmlSeverityType.Warning, expectedError);

                Assert.AreEqual(1, messages.Count);
                Assert.AreSame(expectedError, messages.ToString());
                Assert.IsFalse(messages.HasErrors);
                Assert.IsTrue(messages.HasWarnings);
                Assert.IsNotNull(messages);
            }
        }

        [TestClass]
        public class ToStringMethod : ValidationMessagesTests
        {
            [TestMethod]
            public void OutputsCorrectly()
            {
                var messages = new ValidationMessages();
                const string expectedError = "Error";

                messages.Add(XmlSeverityType.Error, expectedError);

                Assert.AreSame(expectedError, messages.ToString());
            }
        }

        [TestClass]
        public class ResetMethod : ValidationMessagesTests
        {
            [TestMethod]
            public void ResetsState()
            {
                var messages = new ValidationMessages();
                const string expectedError = "Error";
                var expectedList = new List<string> {expectedError};
                messages.Add(XmlSeverityType.Error, expectedError);
                CollectionAssert.AreEqual(expectedList, messages);

                messages.Reset();

                CollectionAssert.AreEqual(new List<string>(), messages);
                Assert.AreEqual("", messages.ToString());
                Assert.IsFalse(messages.HasErrors);
                Assert.IsFalse(messages.HasWarnings);
                Assert.AreEqual(0, messages.Count);
            }
        }
    }
}