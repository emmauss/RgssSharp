using System;
using IronRuby.Builtins;
using IronRuby.Runtime;

namespace RgssSharp.Rgss
{
	[RubyClass("Tone", Inherits = typeof(Object))]
	[Serializable]
	public class Tone
	{
		public static readonly Tone NONE = new Tone(0.0f, 0.0f, 0.0f);

		private float _red, _green, _blue, _gray;

		public float Red
		{
			get => _red;
			set => _red = value.Clamp(-255.0f, 255.0f);
		}

		public float Green
		{
			get => _green;
			set => _green = value.Clamp(-255.0f, 255.0f);
		}

		public float Blue
		{
			get => _blue;
			set => _blue = value.Clamp(-255.0f, 255.0f);
		}

		public float Gray
		{
			get => _gray;
			set => _gray = value.Clamp(0, 255.0f);
		}

		public Tone(float red, float green, float blue, float gray = 0.0f)
		{
			_red = _green = _blue = _gray = 0.0f;
			Set(red, green, blue, gray);
		}

		public void Set(float red, float green, float blue, float gray = 0.0f)
		{
			Red = red;
			Green = green;
			Blue = blue;
			Gray = gray;
		}

		public MutableString _dump()
		{
			var array = new RubyArray(new[] { Red, Green, Blue, Gray });
			return Ruby.Pack(array, "d4");
		}

		public static Tone _load(MutableString io)
		{
			dynamic data = Ruby.Unpack(io, "d4");
			return new Tone((float)data[0], (float)data[1], (float)data[2], (float)data[3]);
		}
	}
}
