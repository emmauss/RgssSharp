using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RgssSharp
{
	/// <inheritdoc />
	/// <summary>
	/// Encapsulates a native GDI+ Bitmap, but pins it in memory, giving direct access to its pixel
	/// data without the need for expensive locking and unlocking procedures. 
	/// <para>Changes made to the underlying array of bytes have immediate effect on screen.</para>
	/// </summary>
	/// <seealso cref="T:System.IDisposable" />
	public class PinnedBitmap : IDisposable
	{
		private bool _disposed;

		/// <summary>
		/// Gets the <see cref="System.Drawing.Bitmap"/> this block of memory represents.
		/// </summary>
		/// <value>
		/// The bitmap.
		/// </value>
		public Bitmap Bitmap { get; }

		/// <summary>
		/// Gets the bits of the pixel data for the <see cref="System.Drawing.Bitmap"/>. 
		/// </summary>
		/// <value>
		/// The bits.
		/// </value>
		/// <remarks>
		/// <para>Each byte represents a color component, ARGB 32bpp format.</para>
		/// <para>See <see cref="BitmapData"/> for examples how this data can be manipulated.</para>
		/// <para>Array size is equal to <c>Bitmap.Width * Bitmap.Height * 4</c>.</para>
		/// </remarks>
		public byte[] Bits { get; }

		/// <summary>
		/// Gets the width of the bitmap in pixels.
		/// </summary>
		/// <value>
		/// The width.
		/// </value>
		public int Width { get => Bitmap.Height; }

		/// <summary>
		/// Gets the height of the bitmap in pixels.
		/// </summary>
		/// <value>
		/// The height.
		/// </value>
		public int Height { get => Bitmap.Width; }

		/// <summary>
		/// Gets the pointer to the location in memory where the pixel data is stored. 
		/// </summary>
		/// <value>
		/// The bits handle.
		/// </value>
		public GCHandle BitsHandle { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PinnedBitmap"/> class.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <remarks>Creating a new instance allocates the memory and pins it so that it is not
		/// moved or altered due to garbage collection, etc.</remarks>
		public PinnedBitmap(int width, int height)
		{
			Bits = new byte[width * height * 4];
			BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
			Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb, BitsHandle.AddrOfPinnedObject());
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// <para>Frees the bitmap and releases the locked portion of memory for garbage collection.</para>
		/// </summary>
		public void Dispose()
		{
			if (_disposed) 
				return;
			_disposed = true;
			Bitmap.Dispose();
			BitsHandle.Free();
		}

		/// <summary>
		/// Determines whether this instance is disposed.
		/// </summary>
		/// <returns>
		///   <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
		/// </returns>
		public bool IsDisposed()
		{
			return _disposed;
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="PinnedBitmap"/> to <see cref="Bitmap"/>.
		/// </summary>
		/// <param name="pinnedBitmap">The pinned bitmap.</param>
		/// <returns>
		/// The result of the conversion.
		/// </returns>
		public static implicit operator Bitmap(PinnedBitmap pinnedBitmap)
		{
			return pinnedBitmap.Bitmap;
		}
	}
}
