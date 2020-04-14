using System;

namespace NMyVision.HtmlParser
{
    public class HtmlAttribute : ICloneable
	{
		/// <summary>
		/// The name for this attribute.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The value for this attribute.
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// Construct a new Attribute.  The name, delim, and value
		/// properties can be specified here.
		/// </summary>
		/// <param name="name">The name of this attribute.</param>
		/// <param name="value">The value of this attribute.</param>
		/// </param>
		public HtmlAttribute(string name, string value)
		{
			this.Name = name;
			this.Value = value;
		}

		#region ICloneable Members
		public virtual object Clone()
		{
			return new HtmlAttribute(Name, Value);
		}
		#endregion
	}
}
