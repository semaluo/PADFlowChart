using System;
using System.Collections;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Stack interface
	/// </summary>
	public interface IStack: IContainer, IComparable, IEnumerable
	{

		/// <summary>
		/// Gets the top object of the stack
		/// </summary>
		object Top
		{
			get;
		}
		/// <summary>
		/// Pushes the given object on the stack
		/// </summary>
		/// <param name="obj"></param>
		void Push(object obj);
		/// <summary>
		/// Pops  the next object from the stack
		/// </summary>
		/// <returns></returns>
		object Pop();
	}

}
