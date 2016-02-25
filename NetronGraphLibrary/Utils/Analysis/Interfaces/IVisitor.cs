using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Interface of a visitor
	/// </summary>
	public interface IVisitor
	{
		/// <summary>
		/// The actual action to perform on visited objects
		/// </summary>
		/// <param name="obj"></param>
		void Visit(object obj);
		/// <summary>
		/// Whether the visiting process is done
		/// </summary>
		bool IsDone { get; }
	}
}
