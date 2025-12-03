using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.Business.Methods;
using System;

namespace Org.Business.Tests
{
    [TestClass]
    public class SubscribersBALTests
    {
        [TestMethod]
        public void TestGetContentForProduct()
        {
            long productId = 25; // Replace with a valid product ID
            
            SubscribersBAL subscribersBAL = new SubscribersBAL();

            // Act
            string content = subscribersBAL.GetContentForProduct(productId);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(content), "Content should not be empty");
        }
    }
}
