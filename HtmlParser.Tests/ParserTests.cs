using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMyVision.HtmlParserTests
{
    [TestClass()]
    public class ParserTests
    {
        [TestMethod()]
        public void SimpleParseTest()
        {
            var hp = new NMyVision.HtmlParser();

            var result = hp.Parse("<div/>").First();

            Assert.AreEqual(result.Tag, "div");
        }

        [TestMethod()]
        public void AttributeTest()
        {
            var hp = new NMyVision.HtmlParser();

            var result = hp.Parse("<input type='text' required />").First();

            Assert.AreEqual(result.Tag, "input");

            Assert.IsTrue(result.Attributes.Contains("type"));

            Assert.AreEqual(result.Attributes["type"], "text");
        }
    }
}