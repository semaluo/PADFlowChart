using System;

namespace Netron.GraphLib.Maths
{
	/// <summary>
	/// Implementation of 1D and 2D noise functions
	/// </summary>
	public sealed class Noise
	{
		/// <summary>
		/// Returns a random number based on a given value.
		/// </summary>
		/// <param name="x">an integer </param>
		/// <returns></returns>
		public static float Noise1d(int x)
		{
			x = (x<<13) ^ x;
			float res = (float)( 1.0 - ( (x * (x * x * 15731 + 789221)
				+ 1376312589) & 0x7fffffff ) / 1073741824.0);
			return res;
		}
		/// <summary>
		/// Returns a random number based on two given numbers
		/// </summary>
		/// <param name="x">an integer</param>
		/// <param name="y">an integer</param>
		/// <returns></returns>
		private static float Noise2d(int x, int y)
		{
			int n;
			n = x + y * 57;
			n = (n<<13) ^ n;
			float res = (float)( 1.0 - ( (n * (n * n * 15731 + 789221)
				+ 1376312589) & 0x7fffffff ) / 1073741824.0);
			return res;
		}

		/// <summary>
		/// Private constructor
		/// 
		/// </summary>
		private Noise(){}
	}
}
