using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronRuby;
using IronRuby.Builtins;

namespace RgssSharp.Rgss
{
	/// <summary>
	/// The multidimensional array class. 
	/// Each element takes up 2 signed bytes (<see langword="short"/>), ranging from -32,768 to 32,767.
	/// </summary>
	[Serializable]
	public class Table
	{

		#region Public Properties

		public short[] data { get; set; }

		/// <summary>
		/// Accesses the array's elements. Pulls the same number of arguments as 
		/// there are dimensions in the created array. Returns nil if the specified 
		/// element does not exist.
		/// </summary>
		/// <param name="x">Index on the x-axis</param>
		/// <returns>Value at the specified position</returns>
		public short this[int x]
		{
			get { return GetData(x, 0, 0); }
			set { SetData(x, 0, 0, value); }
		}

		/// <summary>
		/// Accesses the array's elements. Pulls the same number of arguments as 
		/// there are dimensions in the created array. Returns nil if the specified 
		/// element does not exist.
		/// </summary>
		/// <param name="x">Index on the x-axis</param>
		/// <param name="y">Index on the y-axis</param>
		/// <returns>Value at the specified position</returns>
		public short this[int x, int y]
		{
			get { return GetData(x, y, 0); }
			set { SetData(x, y, 0, value); }
		}

		/// <summary>
		/// Accesses the array's elements. Pulls the same number of arguments as 
		/// there are dimensions in the created array. Returns nil if the specified 
		/// element does not exist.
		/// </summary>
		/// <param name="x">Index on the x-axis</param>
		/// <param name="y">Index on the y-axis</param>
		/// <param name="z">Index on the z-axis</param>
		/// <returns>Value at the specified position</returns>
		public short this[int x, int y, int z]
		{
			get { return GetData(x, y, z); }
			set { SetData(x, y, z, value); }
		}

		/// <summary>
		/// Gets the size of the Table on the x-axis
		/// </summary>
		public int xsize { get; set; }

		/// <summary>
		/// Gets the size of the Table on the y-axis
		/// </summary>
		public int ysize { get; set; }

		/// <summary>
		/// Gets the size of the Table on the z-axis
		/// </summary>
		public int zsize { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a Table object. Specifies the size of each dimension in the multidimensional array. 
		/// 1-, 2-, and 3-dimensional arrays are possible. Arrays with no parameters are also permitted.
		/// </summary>
		public Table() : this(1, 1, 1) { }

		/// <summary>
		/// Creates a Table object. Specifies the size of each dimension in the multidimensional array. 
		/// 1-, 2-, and 3-dimensional arrays are possible. Arrays with no parameters are also permitted.
		/// </summary>
		/// <param name="xSize">The size of the Table on the X-axis.</param>
		public Table(int xSize)
			: this(xSize, 1, 1)
		{
		}

		/// <summary>
		/// Creates a Table object. Specifies the size of each dimension in the multidimensional array. 
		/// 1-, 2-, and 3-dimensional arrays are possible. Arrays with no parameters are also permitted.
		/// </summary>
		/// <param name="xSize">The size of the Table on the X-axis.</param>
		/// <param name="ySize">The size of the Table on the Y-axis.</param>
		public Table(int xSize, int ySize)
			: this(xSize, ySize, 1)
		{
		}

		/// <summary>
		/// Creates a Table object. Specifies the size of each dimension in the multidimensional array. 
		/// 1-, 2-, and 3-dimensional arrays are possible. Arrays with no parameters are also permitted.
		/// </summary>
		/// <param name="xSize">The size of the Table on the X-axis.</param>
		/// <param name="ySize">The size of the Table on the Y-axis.</param>
		/// <param name="zSize">The size of the Table on the Z-axis.</param>
		public Table(int xSize, int ySize, int zSize)
		{
			data = new short[0];
			xSize = xSize.Clamp(0, xSize);
			ySize = ySize.Clamp(0, ySize);
			zSize = zSize.Clamp(0, zSize);
			resize(xSize, ySize, zSize);
		}

		#endregion

		#region Resizing

		/// <summary>
		/// Resizes the table to the specified dimensions
		/// </summary>
		/// <param name="xSize">Size of the Table on the x-axis</param>
		public void resize(int xSize)
		{
			resize(xSize, 1, 1);
		}

		/// <summary>
		/// Resizes the table to the specified dimensions
		/// </summary>
		/// <param name="xSize">Size of the Table on the x-axis</param>
		/// <param name="ySize">Size of the Table on the y-axis</param>
		public void resize(int xSize, int ySize)
		{
			resize(xSize, ySize, 1);
		}

		/// <summary>
		/// Resizes the table to the specified dimensions
		/// </summary>
		/// <param name="xSize">Size of the Table on the x-axis</param>
		/// <param name="ySize">Size of the Table on the y-axis</param>
		/// <param name="zSize">Size of the Table on the z-axis</param>
		public void resize(int xSize, int ySize, int zSize)
		{
			int oldXSize = xsize;
			int oldYSize = ysize;
			int copyXSize = Math.Min(xsize, xSize);
			int copyYSize = Math.Min(ysize, ySize);
			int copyZSize = Math.Min(zsize, zSize);
			int copySize = copyXSize * copyYSize * copyZSize;
			xsize = Math.Max(xSize, 0);
			ysize = Math.Max(ySize, 0);
			zsize = Math.Max(zSize, 0);
			var newData = new short[xSize * ySize * zSize];
			if (copySize > 0)
			{
				for (int x = 0; x < copyXSize; x++)
				{
					for (int y = 0; y < copyYSize; y++)
					{
						for (int z = 0; z < copyZSize; z++)
						{
							newData[x + xsize * (y + ysize * z)] =
								data[x + oldXSize * (y + oldYSize * z)];
						}
					}
				}
			}
			data = newData;
		}

		#endregion

		#region Get/Set Data

		private short GetData(int x, int y, int z)
		{
			if (xsize == 0 || ysize == 0 || zsize == 0)
				return 0;
			x = x.Clamp(0, xsize - 1);
			y = y.Clamp(0, ysize - 1);
			z = z.Clamp(0, zsize - 1);
			return data[x + xsize * (y + ysize * z)];
		}

		private void SetData(int x, int y, int z, short value)
		{
			if (!x.IsBetween(0, xsize - 1) || !y.IsBetween(0, ysize - 1) || !z.IsBetween(0, zsize - 1))
				return;
			data[x + xsize * (y + ysize * z)] = value;
		}

		#endregion

		#region Dump/Load

		public MutableString _dump()
		{
			var array = new RubyArray(new[] { 3, xsize, ysize, zsize, xsize * ysize * zsize });
			array.AddRange(data);
			return Ruby.Pack(array, "LLLLLS*");
		}

		public static Table _load(MutableString io)
		{
			dynamic data = Ruby.Unpack(io, "LLLLLS*");
			var table = new Table(data[1], data[2], data[3]);
			for (var i = 5; i < data.Count; i++)
				table.data[i - 5] = (short)data[i];
			return table;
		}

		#endregion
	}
}
