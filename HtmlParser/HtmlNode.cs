using System;
using System.Collections.Generic;
using System.Linq;

namespace NMyVision
{
	public partial class HtmlNode
	{
		private int startIndex;
		private int endIndex;
		private List<HtmlNode> children;

		public HtmlNodeType Type { get; private set; }

		public string Tag { get; set; }
		public string EndTag { get; set; }

		public bool SelfClosing { get; set; }
		
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

		/// <summary>
		/// Determine if children property has items.
		/// </summary>
		/// <returns>Returns true if children property has items, otherwise false.</returns>
		public bool HasChildren()
			=> this.Children?.Any() ?? false;

		/// <summary>
		/// Determine if attributes property has items.
		/// </summary>
		/// <returns>Returns true if attributes property has items, otherwise false.</returns>
		public bool HasAttributes()
			=> this.Attributes?.Any() ?? false;

	}
}
