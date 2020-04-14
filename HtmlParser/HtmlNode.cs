using System.Collections.Generic;

namespace NMyVision.HtmlParser
{
    public class HtmlNode
	{
		private int startIndex;
		private int endIndex;
		public string Tag;
		public string EndTag;

		public bool SelfClosing;
		public readonly HtmlNodeType Type;
		public string Content = null;
		public List<HtmlAttribute> Attributes;
		public List<HtmlNode> Children;

		public string OuterHTML { get; private set; }

		public HtmlNode(HtmlNodeType type)
		{
			this.Type = type;
		}

		// public override string ToString() => $"{Type.ToString().ToUpper()} { Tag ?? "#text" } [ {Content} ]";

		public void SetEndIndex(int index, string source)
		{
			endIndex = index;
			OuterHTML = source.Substring(startIndex, endIndex - startIndex);
		}

		internal static HtmlNode CreateWhiteSpace(string content = "")
			=> new HtmlNode(HtmlNodeType.Whitespace) { Content = content };

		internal static HtmlNode CreateComment()
			=> new HtmlNode(HtmlNodeType.Comment) { };

		internal static HtmlNode CreateElement(int index)
			=> new HtmlNode(HtmlNodeType.Element) { startIndex = index, Children = new List<HtmlNode>(), Attributes = new List<HtmlAttribute>() };

		internal static HtmlNode CreateElement(string tagName)
			=> new HtmlNode(HtmlNodeType.Element) { Tag = tagName, Children = new List<HtmlNode>(), Attributes = new List<HtmlAttribute>() };

		internal static HtmlNode CreateText(string content = "")
			=> new HtmlNode(HtmlNodeType.Text) { Content = content };

	}
}
