using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
/// <summary>
/// Queue interface
/// </summary>
	public interface IQueue: IContainer, IComparable, IEnumerable
	{
		/// <summary>
		/// Gets the head of the queue
		/// </summary>
		object Head
		{
			get;
		}
		/// <summary>
		/// Enqueues the given object
		/// </summary>
		/// <param name="obj"></param>
		void Enqueue(object obj);
		/// <summary>
		/// Dequeues the next object
		/// </summary>
		/// <returns></returns>
		object Dequeue();	
	}
}
