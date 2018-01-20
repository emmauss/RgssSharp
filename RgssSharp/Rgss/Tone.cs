namespace RgssSharp.Rgss
{
	public class Tone
	{
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
	}
}
