using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	///  IComparable version of the double datatype
	/// </summary>
	public class ComparableDouble : ComparableValue
	{
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="d"></param>
		public ComparableDouble(double d) : base(d)
		{
		}
		/// <summary>
		/// Operator overloading
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static explicit operator double(ComparableDouble c)
		{
			return (double)c.obj;
		}
		/// <summary>
		/// Returns the hashcode
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return (int)(BitConverter.DoubleToInt64Bits((double)obj) >> 20);
		}
	}


}
