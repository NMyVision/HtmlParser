using System.Collections.Generic;

namespace NMyVision.HtmlParser
{
	/// <summary>
	/// Options for the HtmlParser
	/// </summary>
	public class ParserOptions
	{
		/// <summary>
		/// Ignore whitespace when parsing. This should only affect whitespace outside elements.
		/// </summary>
		public bool IgnoreWhitespace { get; set; }

		/// <summary>
		/// These tags can be parsed as self closing tags without the '/' example <hr> instead of <hr />.
		/// </summary>
		public IEnumerable<string> SelfClosingTags { get; set; } = new[] { "area", "base", "br", "col", "embed", "hr", "img", "input", "keygen", "link", "menuitem", "meta", "param", "source", "track", "wbr" };

		/// <summary>
		/// Create the default instance fo ParseOptions object.
		/// </summary>
		public static ParserOptions Default => new ParserOptions { IgnoreWhitespace = true };
	}
}
