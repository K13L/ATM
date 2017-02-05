using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ATM.Controllers;
using System.Web.Mvc;

namespace ATM.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ContactReturnsContactView()
        {
            var homeController = new HomeController();
            var result = homeController.Contact() as ViewResult;
            Assert.AreEqual("About", result.ViewName);
        }

        //[TestMethod]
        //public void ContactFormSaysMessageRecieved()
        //{
        //    var homeController = new HomeController();
        //    var result = homeController.Contact("I love your bank.") as ViewResult;
        //    Assert.IsNotNull(result.ViewBag.TheMessage);

        //}
    }
}
