using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

            Assert.IsFalse(result.Attributes.Any());

            Assert.IsFalse(result.HasAttributes());

            Assert.IsFalse(result.HasChildren());
        }

        [TestMethod()]
        public void SimpleParse2Test()
        {
            var hp = new HtmlParser();

            var result = hp.Parse("<div />").First();

            Assert.AreEqual(result.Tag, "div");

            Assert.IsFalse(result.Attributes.Any());

            Assert.IsFalse(result.HasAttributes());

            Assert.IsFalse(result.HasChildren());
        }

        [TestMethod()]
        public void AttributeTest()
        {
            var hp = new HtmlParser();

            var result = hp.Parse("<input type='text' required />").First();

            Assert.AreEqual(result.Tag, "input");

            Assert.IsTrue(result.Attributes.Contains("type"));

            Assert.AreEqual(result.Attributes["type"], "text");


            Assert.IsTrue(result.HasAttributes());

            Assert.IsFalse(result.HasChildren());
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

            Assert.IsTrue(el.HasChildren());

        }

        [TestMethod()]
        public void LargeHtml()
        {
            var html = @"<html lang=""en"">
    <head>
        <title>Pug</title>
        <script type=""text/javascript""> if (foo) bar(1 + 5) </script>
    </head>
    <body>
        <h1>Pug - node template engine</h1>
        <div id=""container"" class=""col"">
            <p>You are amazing</p>
            <p>Pug is a terse and simple templating language.</p>
        </div>
    </body>
</html>";

            var el = new HtmlParser().Parse(html).First();

            Assert.AreEqual(html, el.OuterHTML);
        }
        [TestMethod()]
        public void EmptyAttribute()
        {
            var html = "<h1 class=''  data-text='base'>Base</h1>";

            var el = new HtmlParser().Parse(html).First();

            Assert.AreEqual(html, el.OuterHTML);

            Assert.AreEqual("h1", el.EndTag);

            Assert.AreEqual(2, el.Attributes.Count);

        }

        [TestMethod()]
        public void EmptyAttributeNoValue()
        {
            var html = "<h1 required>Base</h1>";

            var el = new HtmlParser().Parse(html).First();

            Assert.AreEqual(html, el.OuterHTML);

            Assert.AreEqual("h1", el.EndTag);

            Assert.AreEqual(1, el.Attributes.Count);
        }

        [TestMethod()]
        public void TemplateStringAttribute()
        {
            var html = "<input data-json=` { \"very - long\": \"piece of\" }` />";

            var el = new HtmlParser().Parse(html).First();

            Assert.AreEqual(html, el.OuterHTML);

            Assert.AreEqual("input", el.Tag);

            Assert.AreEqual(1, el.Attributes.Count);

        }
    }
}