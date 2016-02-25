using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Decorates an IEnumerator implementation to an IEnumerable implementation
	/// </summary>
	public class Enumerable: IEnumerable
	{
		#region Field
		/// <summary>
		/// the enumerator of the Enumerable
		/// </summary>
		private IEnumerator enumerator;
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="enumerator"></param>
		public Enumerable(IEnumerator enumerator)
		{
			this.enumerator = enumerator;
		}
		#endregion

		#region Method
		/// <summary>
		/// Returns the IEnumerator enumerator
		/// </summary>
		/// <returns></returns>
		public  virtual IEnumerator GetEnumerator()
		{
			return enumerator;
		}
		#endregion
	}
}
