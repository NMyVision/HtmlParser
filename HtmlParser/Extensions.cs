using System;
using System.Collections.Generic;
using System.Linq;

namespace NMyVision
{
	public static partial class Extensions
	{
		public static string Peek(this Queue<char> queue, int take) => new String(queue.Take(take).ToArray());
	}
}
