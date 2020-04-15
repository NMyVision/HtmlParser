using System;
using System.Collections.Generic;

namespace NMyVision
{
	public partial class HtmlNode
	{
		private int startIndex;
		private int endIndex;
		private List<HtmlNode> children;

		public string Tag { get; set; }
		public string EndTag { get; set; }

		public bool SelfClosing { get; set; }

		public readonly HtmlNodeType Type ;
		
		public string Content { get; set; }
		
		public HtmlAttributeCollection Attributes { get; set; }
		
		public IEnumerable<HtmlNode> Children
		{
			get { return children; }
		}

		public string OuterHTML { get; private set; }

		public HtmlNode(HtmlNodeType type)
		{
			this.Type = type;
		}

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
			=> new HtmlNode(HtmlNodeType.Element) { startIndex = index, children = new List<HtmlNode>(), Attributes = new HtmlAttributeCollection() };

		internal static HtmlNode CreateElement(string tagName)
			=> new HtmlNode(HtmlNodeType.Element) { Tag = tagName, children = new List<HtmlNode>(), Attributes = new HtmlAttributeCollection() };

		internal static HtmlNode CreateText(string content = "")
			=> new HtmlNode(HtmlNodeType.Text) { Content = content };

		internal void AddChildren(IEnumerable<HtmlNode> enumerable)
			=> this.children.AddRange(enumerable);
		
	}
}
