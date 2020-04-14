using System;
using System.Collections.Generic;
using System.Linq;

namespace NMyVision.HtmlParser
{
    public class Parser
	{
		int index = 0;

		readonly string source;
		readonly Queue<char> q;
		readonly ParserOptions options;

		public Parser(string source, ParserOptions options = null)
		{
			this.options = options ?? ParserOptions.Default;
			this.source = source;
			this.q = new Queue<char>(source);
		}

		public IEnumerable<HtmlNode> Parse(HtmlNode parent = null)
		{
			var list = new List<HtmlNode>();
			HtmlNode node = null;
			while (q.Any())
			{
				var c = q.Peek();

				if (q.Peek(2) == "</")
				{
					Dequeue(2);
					if (parent != null)
						parent.EndTag = GetUpTo('>');
					else if (node != null)
						node.EndTag = GetUpTo('>');
					else
						GetUpTo('>');
					Dequeue();
					return list;
				}
				else if (q.Peek(4) == "<!--")
				{
					var comment = GetComment();
					node = HtmlNode.CreateComment();
					node.Content = comment;
					list.Add(node);

				}
				else if (q.Peek(2) == "<!")
				{
					node = CreateDocType();
					list.Add(node);
				}
				else if (c == '<')
				{
					node = HtmlNode.CreateElement(index);

					Dequeue();
					node.Tag = GetTagName();

					if (q.Peek() == ' ' || q.Peek() == '\r')

						node.Attributes = GetAttributes();

					SkipSpace();

					if (!q.Any())
					{
						node.SetEndIndex(index, source);
						list.Add(node);
						break;
					}

					// instantly closed
					if (q.Peek() == '>' && (q.Peek(3) == "></"))
					{
						Dequeue(3);
						node.EndTag = GetUpTo('>');
						Dequeue();

						node.SetEndIndex(index, source);
						list.Add(node);
					}
					// self closing element
					else if (q.Peek(2) == "/>")
					{
						Dequeue(2);
						node.SelfClosing = true;
						node.SetEndIndex(index, source);
						list.Add(node);
					}
					// self closing tags that don't have '/>' ie: <br>
					else if (q.Peek() == '>' && options.SelfClosingTags.Contains(node.Tag))
					{
						Dequeue();
						node.SelfClosing = true;
						list.Add(node);

					}
					else if (node.Tag.Equals("script", StringComparison.OrdinalIgnoreCase))
					{
						Dequeue();
						node.Content = GetUpTo("</script");
						node.EndTag = "script";
						list.Add(node);
					}
				}
				else if (c == '>')
				{
					Dequeue();
					node.Children.AddRange(Parse(node));
					node.SetEndIndex(index, source);
					list.Add(node);
				}
				else
				{
					var text = string.Empty;

					while (q.Any())
					{
						text += Dequeue();
						if (q.Any() && q.Peek() == '<') break;

					}
					if (text.Trim().Length == 0)
					{
						if (options.IgnoreWhitespace == false)
							list.Add(HtmlNode.CreateWhiteSpace(text));

					}
					else
						list.Add(HtmlNode.CreateText(text));
				}

			}

			return list;
		}


		HtmlNode CreateDocType()
		{
			Dequeue(2);
			var text = GetUpTo('>');
			var type = HtmlNodeType.DocType;
			Dequeue();

			return new HtmlNode(type)
			{
				Content = text
			};
		}


		List<HtmlAttribute> GetAttributes()
		{
			var attrs = new List<HtmlAttribute>();

			if (!q.Any()) return attrs;

			if (q.Peek() == '/') return attrs;

			SkipSpace();

			do
			{
				attrs.Add(GetAttribute());
				SkipSpace();

			} while (q.Any() && q.Peek() != '>' && q.Peek(2) != "/>");

			return attrs;

			HtmlAttribute GetAttribute()
			{
				var name = GetUpTo('=', ' ', '>');

				if (q.Peek() == ' ') return new HtmlAttribute(name, null);

				Dequeue();

				if (q.Any())
				{
					// attr=value is valid so check for the scenerio
					if (q.Peek() == '\'' || q.Peek() == '"')
					{
						var del = Dequeue();
						var value = GetUpTo(del);
						Dequeue();
						return new HtmlAttribute(name, value);
					}
					else
					{
						var value = GetUpTo(' ', '>', '<', '\'', '"', '=', '`');
						return new HtmlAttribute(name, value);
					}
				}

				return new HtmlAttribute(name, null);
			}
		}

		string GetTagName() => GetUpTo(' ', '>', '/', '\r', '\n');

		string GetUpTo(params char[] chars)
			 => GetUpTo(() => chars.Contains(q.Peek()));

		string GetUpTo(string text)
			=> GetUpTo(() => this.q.Peek(text.Length) == text);

		string GetComment()
		{
			this.Dequeue(4);
			var text = GetUpTo("-->");
			this.Dequeue(3);
			return text;
		}

		string GetUpTo(Func<bool> fn)
		{
			var text = string.Empty;
			while (q.Any())
			{
				text += Dequeue();
				if (!q.Any()) return text;
				if (fn())
					break;
			}
			return text;
		}

		void SkipSpace()
		{
			while (q.Any() && (q.Peek() == ' ' || q.Peek() == '\r' || q.Peek() == '\n'))
				Dequeue();
		}

		char Dequeue()
		{
			index++;
			return q.Dequeue();
		}

		string Dequeue(int length)
		{
			string text = string.Empty;
			while (length-- > 0)
			{
				index++;
				text += q.Dequeue();
			}
			return text;
		}
	}
}
