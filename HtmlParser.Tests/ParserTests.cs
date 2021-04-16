﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var hp = new HtmlParser();

            var result = hp.Parse("<div/>").First();

            Assert.AreEqual(result.Tag, "div");
        }

        [TestMethod()]
        public void AttributeTest()
        {
            var hp = new HtmlParser();

            var result = hp.Parse("<input type='text' required />").First();

            Assert.AreEqual(result.Tag, "input");

            Assert.IsTrue(result.Attributes.Contains("type"));

            Assert.AreEqual(result.Attributes["type"], "text");
        }


        [TestMethod()]
        public void MultipleRunsTest()
        {
            string markup = @"
                <svg viewBox='0 0 64 64' xmlns='http://www.w3.org/2000/svg' fill-rule='evenodd' clip-rule='evenodd' stroke-linejoin='round' stroke-miterlimit='2'>
                  <path d='M10 10h6.022l29.242 44h-5.802L22.586 28.584V54H10V10zm44 44h-6.023L18.735 10h5.802l16.876 25.416V10H54v44z' fill='#4664a9' fill-rule='nonzero'/>
                </svg>";
         
            var parser = new HtmlParser();

            var el = parser.Parse(markup).First();

            parser.Parse(markup).First().Children.First();

        }
    }
}