using System;
using System.Threading.Tasks;
using IronRuby.Runtime;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace RgssSharp.Rgss
{
	[RubyClass("Autotile", Inherits = typeof(Object))]
	public class Autotile
	{

		public static readonly int[][] INDEX = { 
			new[] { 27,28,33,34 },   new[] { 5,28,33,34 },   new[] { 27,6,33,34 },  
			new[] { 5,6,33,34 },     new[] { 27,28,33,12 },  new[] { 5,28,33,12 },  
			new[] { 27,6,33,12 },    new[] { 5,6,33,12 },    new[] { 27,28,11,34 },  
			new[] { 5,28,11,34 },    new[] { 27,6,11,34 },   new[] { 5,6,11,34 },
			new[] { 27,28,11,12 },   new[] { 5,28,11,12 },   new[] { 27,6,11,12 },  
			new[] { 5,6,11,12 },     new[] { 25,26,31,32 },  new[] { 25,6,31,32 },  
			new[] { 25,26,31,12 },   new[] { 25,6,31,12 },   new[] { 15,16,21,22 },  
			new[] { 15,16,21,12 },   new[] { 15,16,11,22 },  new[] { 15,16,11,12 },
			new[] { 29,30,35,36 },   new[] { 29,30,11,36 },  new[] { 5,30,35,36 },  
			new[] { 5,30,11,36 },    new[] { 39,40,45,46 },  new[] { 5,40,45,46 },  
			new[] { 39,6,45,46 },    new[] { 5,6,45,46 },    new[] { 25,30,31,36 },  
			new[] { 15,16,45,46 },   new[] { 13,14,19,20 },  new[] { 13,14,19,12 },
			new[] { 17,18,23,24 },   new[] { 17,18,11,24 },  new[] { 41,42,47,48 }, 
			new[] { 5,42,47,48 },    new[] { 37,38,43,44 },  new[] { 37,6,43,44 },  
			new[] { 13,18,19,24 },   new[] { 13,14,43,44 },  new[] { 37,42,43,48 },  
			new[] { 17,18,47,48 },   new[] { 13,18,43,48 },  new[] { 13,18,43,48 }
		};

		public Texture2D this[int tileId, int frame]
		{
			get => Data[frame % FrameCount, tileId];
		}

		/// <summary>
		/// Gets a two-dimensional array of textures (<see cref="Texture2D"/>), where
		/// the elements within contain the cached tile at the specified x and y coordinate.
		/// </summary>
		public Texture2D[,] Data { get; private set; }

		/// <summary>
		/// Gets the number of frames this Autotile contains for animated tiles.
		/// <para>For non-animated Autotiles, this value is always one.</para> 
		/// </summary>
		/// <remarks>Supports up to 16 frames for animation.</remarks>
		public  int FrameCount { get; private set; }

		/// <summary>
		/// Gets a flag indicating if resources used by this object have been freed.
		/// </summary>
		public bool IsDisposed { get; private set; }
				
		public Autotile(Texture2D bitmap)
		{
			CreateIndex(bitmap);
		}

		public void Dispose()
		{
			if (IsDisposed)
				return;
			IsDisposed = true;
			var maxX = Data.GetLength(0);
			var maxY = Data.GetLength(1);
			for (var x = 0; x < maxX; x++)
			{
				for (var y = 0; y < maxY; y++)
				{
					Data[x, y].Dispose();
				}
			}
			Data = null;
		}

		private void CreateIndex(Texture2D srcBitmap)
		{
			// TODO: Definte benchmarking to be done here.
			// TODO: Use single dimension array and just calculate stride? Jow much more efficient? This
			// TODO: not being invoke very often, only on initial map load, so that much optimization needed?
			FrameCount = srcBitmap.Width / 96;
			Data = new Texture2D[FrameCount, 48];
			for (var frame = 0; frame < FrameCount; frame++)
			{
				using (var bitmap = new Texture2D(Graphics.Device, 256, 192))
				{
					for (var i = 0; i < 6; i++)
					{
						for (var j = 0; j < 8; j++)
						{
							Rectangle srcRect;
							uint[] data;
							foreach (var number in INDEX[8 * i + j])
							{
								var num = number - 1;
								var x = 16 * (num % 6);
								var y = 16 * (num / 6);
								srcRect = new Rectangle(x + (frame * 96), y, 16, 16);
								var destRect = new Rectangle(32 * j + x % 32, 32 * i + y % 32, 16, 16);
								data = new uint[16 * 16];
								srcBitmap.GetData(0, srcRect, data, 0, data.Length);
								bitmap.SetData(0, destRect, data, 0, data.Length);
							}
							var index = 8 * i + j;
							var sx = 32 * (index % 8);
							var sy = 32 * (index / 8);
							srcRect = new Rectangle(sx, sy, 32, 32);
							data = new uint[32 * 32];
							bitmap.GetData(0, srcRect, data, 0, data.Length);
							Data[frame, index] = new Texture2D(srcBitmap.GraphicsDevice, 32, 32);
							Data[frame, index].SetData(data);
						}
					}
				}
			}
		}
	}

}
