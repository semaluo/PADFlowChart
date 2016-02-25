using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Utility class related to interval checks
	/// </summary>
	public class Bounds
	{

		/// <summary>
		/// Checks if the given integer is in the (closed) interval given.
		/// Returns null if alright, otherwise throws an IndexOutOfRangeException exception.
		/// </summary>
		/// <param name="i">an integer</param>
		/// <param name="rangeBase">the lower interval value</param>
		/// <param name="length">the length of the interval, i.e. the uppder interval value is (rangeBase+length-1)</param>
		/// 
		public static void Check(int i, int rangeBase, int length)
		{
			if (i < rangeBase || i > rangeBase + length - 1)
			{
				throw new IndexOutOfRangeException();
			}
			else
			{
				return;
			}
		}
	}
}
