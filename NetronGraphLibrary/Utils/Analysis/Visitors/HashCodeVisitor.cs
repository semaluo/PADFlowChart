using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Hashcode visitor
	/// </summary>
	public class HashCodeVisitor : AbstractVisitor
	{
		/// <summary>
		/// the hashcode
		/// </summary>
		private int result;

		/// <summary>
		/// Visits the given object
		/// </summary>
		/// <param name="obj"></param>
		public override void Visit(object obj)
		{
			result += obj.GetHashCode();
		}

		/// <summary>
		/// Returns the hashcode
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return result;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public HashCodeVisitor()
		{
			result = 0;
				
		}
	}

}
