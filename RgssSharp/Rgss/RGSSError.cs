using System;
using IronRuby.Runtime;

namespace RgssSharp.Rgss
{
	[RubyClass("RGSSError", Inherits = typeof(RubyExceptions))]
	public class RgssError : Exception
	{
		public RgssError(string message) : base(message)
		{

		}
	}
}