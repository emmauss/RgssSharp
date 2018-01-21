using IronRuby.Runtime;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using SD = System.Drawing;


namespace RgssSharp.Rgss
{
	/// <inheritdoc />
	/// <summary>
	/// Defines a particular format for text, including font face, size, and style attributes.
	/// <para>The font class. <b>Font</b> is a property of the <see cref="T:RgssSharp.Rgss.Bitmap" /> class.</para>
	/// </summary>
	/// <seealso cref="T:RgssSharp.Rgss.Bitmap" />
	/// <seealso cref="T:System.IDisposable" />
	public class Font : IDisposable
	{
		#region Static

		/// <summary>
		/// The private font container for storing fonts loaded into memory.
		/// </summary>
		private static PrivateFontCollection _privateFonts;
		/// <summary>
		/// The instance of the internally used default <see cref="SD.Font"/>.
		/// </summary>
		private static SD.Font _default = new SD.Font("MS PGothic", 22, SD.FontStyle.Regular, SD.GraphicsUnit.Pixel);
		/// <summary>
		/// The level of tolerance for comparing float values of font sizes.
		/// </summary>
		private const float TOLERANCE = 0.01f;

		/// <summary>
		/// Gets or sets the default font family name.
		/// </summary>
		/// <value>
		/// The default font family name.
		/// </value>
		public static string DefaultName
		{
			get => _default.FontFamily.Name;
			set
			{
				if (String.Compare(_default.FontFamily.Name, value, StringComparison.InvariantCultureIgnoreCase) == 0) 
					return;
				_default.Dispose();
				_default = Create(value, _default.Size, _default.Style);
			}
		}

		/// <summary>
		/// Gets or sets the default font size, in pixels.
		/// </summary>
		/// <value>
		/// The default size.
		/// </value>
		public static float DefaultSize
		{
			get => _default.Size;
			set
			{
				if (Math.Abs(value - _default.Size) < TOLERANCE) 
					return;
				_default.Dispose();
				_default = Create(_default.FontFamily.Name, value, _default.Style);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether font is bold by default.
		/// </summary>
		/// <value>
		///   <c>true</c> if bold; otherwise, <c>false</c>.
		/// </value>
		public static bool DefaultBold
		{
			get => _default.Bold;
			set
			{
				if (value == _default.Bold)
					return;
				_default.Dispose();
				_default = Create(_default.FontFamily.Name, _default.Size, _default.Style ^ SD.FontStyle.Bold);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether font is italic by default.
		/// </summary>
		/// <value>
		///   <c>true</c> if italic; otherwise, <c>false</c>.
		/// </value>
		public static bool DefaultItalic 
		{
			get => _default.Italic;
			set
			{
				if (value == _default.Italic)
					return;
				_default.Dispose();
				_default = Create(_default.FontFamily.Name, _default.Size, _default.Style ^ SD.FontStyle.Italic);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether font is underline by default.
		/// </summary>
		/// <value>
		///   <c>true</c> if underline; otherwise, <c>false</c>.
		/// </value>
		public static bool DefaultUnderline 
		{
			get => _default.Underline;
			set
			{
				if (value == _default.Underline)
					return;
				_default.Dispose();
				_default = Create(_default.FontFamily.Name, _default.Size, _default.Style ^ SD.FontStyle.Underline);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether font is strikeout by default.
		/// </summary>
		/// <value>
		///   <c>true</c> if strikeout; otherwise, <c>false</c>.
		/// </value>
		public static bool DefaultStrikeout
		{
			get => _default.Strikeout;
			set
			{
				if (value == _default.Strikeout)
					return;
				_default.Dispose();
				_default = Create(_default.FontFamily.Name, _default.Size, _default.Style ^ SD.FontStyle.Strikeout);
			}
		}

		/// <summary>
		/// Gets or sets the default font color.
		/// </summary>
		/// <value>
		/// The default color.
		/// </value>
		public static Color DefaultColor { get; set; } = new Color(255, 255, 255);

		/// <summary>
		/// Loads the fonts found in the Font folder.
		/// </summary>
		/// <remarks>Supported formats are TrueType (*.ttf), OpenType (*.otf), and Raw Bitmap (*.fnt).</remarks>
		internal static void LoadFonts()
		{
			_privateFonts = new PrivateFontCollection();
			var list = new List<string>(Directory.GetFiles("Fonts", "*.ttf"));
			list.AddRange(Directory.GetFiles("Fonts", "*.otf"));
			list.AddRange(Directory.GetFiles("Fonts", "*.fnt"));
			foreach (var filename in list)
				AddFont(filename);
		}

		/// <summary>
		/// Loads a font from the specified file to be used by the application without installation.
		/// </summary>
		/// <param name="filename">The filename to load (*.ttf, *.otf, *.fnt).</param>
		public static void AddFont(string filename)
		{
			NativeMethods.AddFontResourceEx(filename, NativeMethods.FR_PRIVATE, IntPtr.Zero);
			using (var stream = File.OpenRead(filename))
			{
				var length = (int) stream.Length;
				var ptr = Marshal.AllocCoTaskMem(length);
				var data = new byte[stream.Length];
				stream.Read(data, 0, length);
				Marshal.Copy(data, 0, ptr, length);
				_privateFonts.AddMemoryFont(ptr, length);
				Marshal.FreeCoTaskMem(ptr);
			}
		}

		/// <summary>
		/// Creates the specified font.
		/// </summary>
		/// <param name="familyName">Name of the font family.</param>
		/// <param name="size">The size of the font.</param>
		/// <param name="style">The style.</param>
		/// <returns>A newly created <see cref="SD.Font"/>.</returns>
		private static SD.Font Create(string familyName, float size, SD.FontStyle style)
		{
			var family = _privateFonts.Families.First(f => f.Name == familyName) ?? new SD.FontFamily(familyName);
			return new SD.Font(family, size, style, SD.GraphicsUnit.Pixel);
		}

		/// <summary>
		/// Gets the font style based on flags.
		/// </summary>
		/// <param name="bold">if set to <c>true</c> [bold].</param>
		/// <param name="italic">if set to <c>true</c> [italic].</param>
		/// <param name="underline">if set to <c>true</c> [underline].</param>
		/// <param name="strikeout">if set to <c>true</c> [strikeout].</param>
		/// <returns>A <see cref="SD.FontStyle"/> enumeration with specified flags set.</returns>
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

		/// <summary>
		/// Checks if the specified font exists and is available to use. 
		/// </summary>
		/// <param name="familyName">Name of the font family.</param>
		/// <returns>
		/// <c>true</c> if font is installed or loaded into memory; otherwise, <c>false</c>.
		/// </returns>
		[RubyMethod("exists?", RubyMethodAttributes.PublicSingleton)]
		public static bool Exists(string familyName)
		{
			using (var testFont = Create(familyName, DefaultSize, SD.FontStyle.Regular))
				return String.Compare(testFont.FontFamily.Name, familyName, StringComparison.InvariantCultureIgnoreCase) == 0;
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="Font"/> to <see cref="SD.Font"/>.
		/// </summary>
		/// <param name="font">The font.</param>
		/// <returns>
		/// The result of the conversion.
		/// </returns>
		public static implicit operator SD.Font(Font font)
		{
			return font._font;
		}

		/// <summary>
		/// Performs an explicit conversion from <see cref="SD.Font"/> to <see cref="Font"/>.
		/// </summary>
		/// <param name="sysFont">The system font.</param>
		/// <returns>
		/// The result of the conversion.
		/// </returns>
		public static explicit operator Font(SD.Font sysFont)
		{
			return new Font(sysFont);
		}

		#endregion

		/// <summary>
		/// Internally, this class is a wrapper around a GDI <see cref="SD.Font"/>. 
		/// <para>This field is the internal font that is actually used for drawing.</para>
		/// </summary>
		private SD.Font _font;

		/// <summary>
		/// Gets or sets the font family name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name
		{
			get => _font.FontFamily.Name;
			set
			{
				if (String.Compare(_font.FontFamily.Name, value, StringComparison.InvariantCultureIgnoreCase) != 0)
					return;
				_font.Dispose();
				_font = Create(value, _font.Size, _font.Style);
			}
		}

		/// <summary>
		/// Gets or sets the font size.
		/// </summary>
		/// <value>
		/// The size.
		/// </value>
		public float Size
		{
			get => _font.Size;
			set
			{
				if (Math.Abs(_font.Size - value) < TOLERANCE)
					return;
				_font.Dispose();
				_font = Create(_font.FontFamily.Name, value, _font.Style);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Font"/> is bold.
		/// </summary>
		/// <value>
		///   <c>true</c> if bold; otherwise, <c>false</c>.
		/// </value>
		public bool Bold
		{
			get => _font.Bold;
			set
			{
				if (_font.Bold == value)
					return;
				_font.Dispose();
				_font = Create(_font.FontFamily.Name, _font.Size, _font.Style ^ SD.FontStyle.Bold);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Font"/> is italic.
		/// </summary>
		/// <value>
		///   <c>true</c> if italic; otherwise, <c>false</c>.
		/// </value>
		public bool Italic
		{
			get => _font.Italic;
			set
			{
				if (_font.Italic == value)
					return;
				_font.Dispose();
				_font = Create(_font.FontFamily.Name, _font.Size, _font.Style ^ SD.FontStyle.Italic);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Font"/> is underline.
		/// </summary>
		/// <value>
		///   <c>true</c> if underline; otherwise, <c>false</c>.
		/// </value>
		public bool Underline
		{
			get => _font.Underline;
			set
			{
				if (_font.Underline == value)
					return;
				_font.Dispose();
				_font = Create(_font.FontFamily.Name, _font.Size, _font.Style ^ SD.FontStyle.Underline);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Font"/> is strikeout.
		/// </summary>
		/// <value>
		///   <c>true</c> if strikeout; otherwise, <c>false</c>.
		/// </value>
		public bool Strikeout
		{
			get => _font.Strikeout;
			set
			{
				if (_font.Strikeout == value)
					return;
				_font.Dispose();
				_font = Create(_font.FontFamily.Name, _font.Size, _font.Style ^ SD.FontStyle.Strikeout);
			}
		}

		/// <summary>
		/// Gets or sets the color.
		/// </summary>
		/// <value>
		/// The color.
		/// </value>
		public Color Color { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Font"/> class.
		/// </summary>
		/// <param name="font">The <see cref="SD.Font"/> to create it from.</param>
		public Font(SD.Font font)
		{
			Color = DefaultColor;
			_font = font;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Font"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public Font(string name) : 
			this(name, DefaultSize, DefaultColor, DefaultBold, DefaultItalic, DefaultUnderline, DefaultStrikeout)
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Font"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="size">The size.</param>
		public Font(string name, float size) :
			this(name, size, DefaultColor, DefaultBold, DefaultItalic, DefaultUnderline, DefaultStrikeout)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Font"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="size">The size.</param>
		/// <param name="color">The color.</param>
		public Font(string name, float size, Color color) :
			this(name, size, color, DefaultBold, DefaultItalic, DefaultUnderline, DefaultStrikeout)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Font"/> class.
		/// </summary>
		/// <param name="name">The font family name.</param>
		/// <param name="size">The font size.</param>
		/// <param name="color">The color.</param>
		/// <param name="bold">if set to <c>true</c> [bold].</param>
		/// <param name="italic">if set to <c>true</c> [italic].</param>
		/// <param name="underline">if set to <c>true</c> [underline].</param>
		/// <param name="strikeout">if set to <c>true</c> [strikeout].</param>
		public Font(string name, float size, Color color, bool bold, bool italic, bool underline, bool strikeout)
		{
			Color = color;
			_font = new SD.Font(name, size, GetStyle(bold, italic, underline, strikeout), SD.GraphicsUnit.Pixel);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_font.Dispose();
		}
	}
}