using System.Collections.Generic;
using System.Xml.Schema;
using Difi.Felles.Utility.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.Felles.Utility.Tester.Validation
{
    [TestClass]
    public class ValidationMessagesTests
    {
        [TestMethod]
        public void AddErrorTest()
        {
            var messages = new ValidationMessages();
            const string expectedError = "Error";
            messages.Add(XmlSeverityType.Error, expectedError);

            Assert.AreEqual(1, messages.Count);
            Assert.AreSame(expectedError, messages.ToString());
            Assert.IsTrue(messages.HasErrors);
            Assert.IsFalse(messages.HasWarnings);
            Assert.IsNotNull(messages);
        }

        [TestMethod]
        public void AddWarningTest()
        {
            var messages = new ValidationMessages();
            const string expectedError = "Warning";
            messages.Add(XmlSeverityType.Warning, expectedError);

            Assert.AreEqual(1, messages.Count);
            Assert.AreSame(expectedError, messages.ToString());
            Assert.IsFalse(messages.HasErrors);
            Assert.IsTrue(messages.HasWarnings);
            Assert.IsNotNull(messages);
        }

        [TestMethod]
        public void CountTest()
        {
            var messages = new ValidationMessages();

            Assert.AreEqual(0, messages.Count);
            messages.Add(XmlSeverityType.Error, "Error");

            Assert.AreEqual(1, messages.Count);
        }

        [TestMethod]
        public void ListTest()
        {
            var messages = new ValidationMessages();
            const string expectedError = "Error";
            var expectedList = new List<string> {expectedError};

            messages.Add(XmlSeverityType.Error, expectedError);
            CollectionAssert.AreEqual(expectedList, messages);
        }

        [TestMethod]
        public void ToStringTest()
        {
            var messages = new ValidationMessages();
            const string expectedError = "Error";

            messages.Add(XmlSeverityType.Error, expectedError);

            Assert.AreSame(expectedError, messages.ToString());
        }

        [TestMethod]
        public void ResetTest()
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