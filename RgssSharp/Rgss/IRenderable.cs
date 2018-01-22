using IronRuby.Runtime;
using System;
using IronRuby.Builtins;

namespace RgssSharp.Rgss
{
	/// <summary>
	/// Defines a generalized structure for renderable objects.
	/// </summary>
	/// <seealso cref="T:System.IComparable`1" />
	/// <seealso cref="T:System.IDisposable" />
	/// <seealso cref="T:RgssSharp.Rgss.Viewport" />
	/// <seealso cref="T:RgssSharp.Rgss.Sprite" />
	/// <seealso cref="T:RgssSharp.Rgss.Plane" />
	/// <seealso cref="T:RgssSharp.Rgss.Tilemap" />
	/// <seealso cref="T:RgssSharp.Rgss.Window" />
	[RubyModule("Renderable", DefineIn = typeof(RubyModule))]
	public interface IRenderable : IComparable<IRenderable>, IDisposable
	{
		/// <summary>
		/// Gets or sets the Z-coordinate for the sprite.
		/// </summary>
		/// <value>
		/// The Z-coordinate.
		/// </value>
		/// <remarks>
		/// <para>This value determines the "depth" of the sprite on the screen.</para>
		/// <para>Sprites with lower Z-coordiantes are drawn first, while sprites with higher values are drawn later, therefore overlapping them. </para>
		/// <para>All values are relative to the <see cref="Viewport"/> they are contained in, while a <see cref="Sprite"/> without a Viewport is ordered independently along with each Viewport.</para>
		/// </remarks>
		int Z { get; set; }

		/// <summary>
		/// Gets or sets the opacity.
		/// </summary>
		/// <value>
		/// The opacity.
		/// </value>
		int Opacity { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="IRenderable"/> is visible.
		/// </summary>
		/// <value>
		///   <c>true</c> if visible; otherwise, <c>false</c>.
		/// </value>
		bool Visible { get; set; }

		/// <summary>
		/// Renders this object to the screen. 
		/// </summary>
		void Draw();

		/// <summary>
		/// Adds this object to the queue to be rendered on the next frame. 
		/// <para>The object is not drawn if this method is not called each frame.</para>
		/// </summary>
		void Update();

		/// <summary>
		/// Determines whether this instance is disposed.
		/// </summary>
		/// <returns>
		///   <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>This method is <c>disposed?</c> when being invoked via Ruby.</remarks>
		[RubyMethod("disposed?")]
		bool IsDisposed();

		
	}
}
