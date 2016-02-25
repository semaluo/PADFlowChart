using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// IComparable version of the char datatype
	/// </summary>
	public class ComparableChar : ComparableValue
	{

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="c"></param>
		public ComparableChar(char c) : base(c)
		{
		}

		/// <summary>
		/// Operator overloading
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static explicit operator char(ComparableChar c)
		{
			return (char)c.obj;
		}

		/// <summary>
		/// Returns the hashcode
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return (char)obj;
		}
	}

}
