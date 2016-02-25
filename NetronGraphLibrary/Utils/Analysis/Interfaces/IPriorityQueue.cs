using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Priority queue interface
	/// </summary>
	public interface IPriorityQueue: IContainer, IComparable, IEnumerable
	{

		/// <summary>
		/// Gets the min
		/// </summary>
		ComparableObject Min
		{
			get;
		}	
		/// <summary>
		/// Enqueues an object
		/// </summary>
		/// <param name="obj"></param>
		void Enqueue(ComparableObject obj);
		/// <summary>
		/// Dequeues an object
		/// </summary>
		/// <returns></returns>
		ComparableObject DequeueMin();
	}

}
