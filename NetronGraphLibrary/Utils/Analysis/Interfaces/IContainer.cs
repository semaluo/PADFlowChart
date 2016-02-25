using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Container interface; the base interface for all data structures
	/// </summary>
	public interface IContainer: IComparable, IEnumerable
	{

		/// <summary>
		/// Gets the amount of elements
		/// </summary>
		int Count
		{
			get;
		}
		/// <summary>
		/// Gets whether the container is empty
		/// </summary>
		bool IsEmpty
		{
			get;
		}
		/// <summary>
		/// Gets whether the container is full
		/// </summary>
		bool IsFull
		{
			get;
		}
		/// <summary>
		/// Empties the container
		/// </summary>
		void Purge();
		/// <summary>
		/// Accepts an IVisitor
		/// </summary>
		/// <param name="visitor"></param>
		void Accept(IVisitor visitor);
	}
}
