using System;
using IronRuby.Builtins;
using XnaColor = Microsoft.Xna.Framework.Color;
using SD = System.Drawing;

namespace RgssSharp.Rgss
{

	public struct Color
	{
		public static readonly Color WHITE = new Color(255, 255, 255);
		public static readonly Color BLACK = new Color(0, 0, 0);
		public static readonly Color CLEAR = new Color(0, 0, 0, 0);


		private XnaColor _color;
		private float _red, _green, _blue, _alpha;

		public float Red
		{
			get => _red;
			set
			{
				_red = value.Clamp(0.0f, 255.0f);
				_color.R = (byte)Math.Round(_red);
			}
		}

		public float Green
		{
			get => _green;
			set
			{
				_green = value.Clamp(0.0f, 255.0f);
				_color.G = (byte)Math.Round(_green);
			}
		}

		public float Blue
		{
			get => _blue;
			set
			{
				_blue = value.Clamp(0.0f, 255.0f);
				_color.B = (byte)Math.Round(_blue);
			}
		}

		public float Alpha
		{
			get => _alpha;
			set
			{
				_alpha = value.Clamp(0.0f, 255.0f);
				_color.A = Convert.ToByte(_alpha);
			}
		}

		public uint PackedValue { get => _color.PackedValue; }

		public Color(float red, float green, float blue, float alpha = 255.0f)
		{
			_red = _green = _blue = _alpha = 0.0f;
			_color = new XnaColor();
			Set(red, green, blue, alpha);
		}

		public void Set(float red, float green, float blue, float alpha = 255.0f)
		{
			Red = red;
			Green = green;
			Blue = blue;
			Alpha = alpha;
		}

		public static string ToHtml(Color color)
		{
			var sysColor = SD.Color.FromArgb(color._color.A, color._color.R, color._color.G, color._color.B);
			return SD.ColorTranslator.ToHtml(sysColor);
		}

		public static Color FromHtml(string htmlColor)
		{
			if (!htmlColor.StartsWith("#", StringComparison.InvariantCulture))
				htmlColor = "#" + htmlColor;
			var color = SD.ColorTranslator.FromHtml(htmlColor);
			return new Color(color.R, color.G, color.B, color.A);
		}

		public MutableString _dump()
		{
			var array = new RubyArray(new[] { Red, Green, Blue, Alpha });
			return Ruby.Pack(array, "d4");
		}

		public static Color _load(MutableString io)
		{
			dynamic data = Ruby.Unpack(io, "d4");
			return new Color((float)data[0], (float)data[1], (float)data[2], (float)data[3]);
		}

		public static implicit operator XnaColor(Color color)
		{
			return color._color;
		}

		public static implicit operator SD.Color(Color color)
		{
			return SD.Color.FromArgb(color._color.A, color._color.R, color._color.G, color._color.B);
		}

		public static explicit operator Color(XnaColor color)
		{
			return new Color(color.R, color.B, color.G, color.A);
		}
		
	}
}
