using System;

namespace RgssSharp
{
	public static class Extensions
	{
		public static bool IsBetween<T>(this T value, T lowerBound, T upperBound) where T :
			struct, 
			IComparable, 
			IComparable<T>, 
			IConvertible, 
			IEquatable<T>, 
			IFormattable
		{
			return value.CompareTo(lowerBound) >= 0 && value.CompareTo(upperBound) <= 0;
		}


		public static T Clamp<T>(this T value, T min, T max) where T : 
			struct, 
			IComparable, 
			IComparable<T>, 
			IConvertible, 
			IEquatable<T>, 
			IFormattable
		{
			if (value.CompareTo(min) < 0) 
				return min;
			return value.CompareTo(max) > 0 ? max : value;
		}
	}
}