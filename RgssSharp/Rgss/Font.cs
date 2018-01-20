using System;
using IronRuby.Runtime;
using SD = System.Drawing;

namespace RgssSharp.Rgss
{
	public class Font : IDisposable
	{

		#region Static

		[RubyMethod("exists?", RubyMethodAttributes.PublicSingleton)]
		public static bool Exists(string fontName)
		{
			using (var testFont = new SD.Font(fontName, DefaultSize))
				return String.Compare(testFont.Name, fontName, StringComparison.InvariantCultureIgnoreCase) == 0;
		}

		public static implicit operator SD.Font(Font font)
		{
			return font._internalFont;
		}

		private static SD.FontStyle GetStyle(bool bold, bool italic, bool underline, bool strikeout)
		{
			var style = SD.FontStyle.Regular;
			if (bold)
				style |= SD.FontStyle.Bold;
			if (italic)
				style |= SD.FontStyle.Italic;
			if (underline)
				style |= SD.FontStyle.Underline;
			if (strikeout)
				style |= SD.FontStyle.Strikeout;
			return style;
		}


		public static string DefaultName { get; set; } = "Calibri";

		public static int DefaultSize { get; set; } = 16;

		public static bool DefaultBold { get; set; } = false;

		public static bool DefaultItalic { get; set; } = false;

		public static bool DefaultUnderline { get; set; } = false;

		public static bool DefaultStrikeout { get; set; } = false;

		public static Color DefaultColor { get; set; } = new Color(255, 255, 255);

		#endregion

		private SD.Font _internalFont;
		private string _name;
		private int _size;
		private bool _bold;
		private bool _italic;
		private bool _underline;
		private bool _strikeout;

		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				RecreateFont();
			}
		}

		public int Size
		{
			get => _size;
			set
			{
				_size = value;
				RecreateFont();
			}
		}

		public bool Bold
		{
			get => _bold;
			set
			{
				_bold = value;
				RecreateFont();
			}
		}

		public bool Italic
		{
			get => _italic;
			set
			{
				_italic = value;
				RecreateFont();
			}
		}

		public bool Underline
		{
			get => _underline;
			set
			{
				_underline = value;
				RecreateFont();
			}
		}

		public bool Strikeout
		{
			get => _strikeout;
			set
			{
				_strikeout = value;
				RecreateFont();
			}
		}

		public Color Color { get; set; }

		public Font(string name) : 
			this(name, DefaultSize, DefaultColor, DefaultBold, DefaultItalic, DefaultUnderline, DefaultStrikeout)
		{
			
		}

		public Font(string name, int size) :
			this(name, size, DefaultColor, DefaultBold, DefaultItalic, DefaultUnderline, DefaultStrikeout)
		{

		}

		public Font(string name, int size, Color color) :
			this(name, size, color, DefaultBold, DefaultItalic, DefaultUnderline, DefaultStrikeout)
		{

		}

		public Font(string name, int size, Color color, bool bold, bool italic, bool underline, bool strikeout)
		{
			_name = name;
			_size = size;
			_bold = bold;
			_italic = italic;
			_underline = underline;
			_strikeout = strikeout;
			Color = color;
			_internalFont = new SD.Font(name, size, GetStyle(bold, italic, underline, strikeout));
		}

		private void RecreateFont()
		{
			_internalFont.Dispose();
			_internalFont = new SD.Font(_name, _size, GetStyle(_bold, _italic, _underline, _strikeout));
		}

		public void Dispose()
		{
			_internalFont.Dispose();
		}
	}
}