using System;
using System.Collections.Generic;

namespace NMyVision
{
	public class HtmlAttributeCollection : Dictionary<string, string>
	{
		internal void Add(KeyValuePair<string, string> kv)
		{
			var key = kv.Key;

			if (!this.ContainsKey(key))
			{
				this.Add(key, kv.Value);
			}
		}

		public bool Contains(string key)
			=> this.ContainsKey(key);
		
	}
}
