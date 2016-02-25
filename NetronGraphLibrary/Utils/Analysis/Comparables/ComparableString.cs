using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	///  IComparable version of the string datatype
	/// </summary>
	public class ComparableString : ComparableValue
	{
		/// <summary>
		/// shift constant
		/// </summary>
		private const int shift = 6;
		/// <summary>
		/// mask constant
		/// </summary>
		private const int mask = -67108864;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="c"></param>
		public ComparableString(string c) : base(c)
		{
		}
		/// <summary>
		/// Operator overloading
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static explicit operator string(ComparableString c)
		{
			return (String)c.obj;
		}
		/// <summary>
		/// Returns the hashcode
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			int i = 0;
			string str = (String)obj;
			for (int j = 0; j < str.Length; j++)
			{
				i = i & -67108864 ^ i << 6 ^ str[j];
			}
			return i;
		}
	}

}
