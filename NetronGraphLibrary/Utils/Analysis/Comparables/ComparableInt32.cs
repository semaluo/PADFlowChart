using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	///  IComparable version of the Int32 datatype
	/// </summary>
	public class ComparableInt32 : ComparableValue
	{

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="i"></param>
		public ComparableInt32(int i) : base(i)
		{
		}

		/// <summary>
		/// Operator overload
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static explicit operator int(ComparableInt32 c)
		{
			return (int)c.obj;
		}

		/// <summary>
		/// Returns the hashcode
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return (int)obj;
		}
	}

}
