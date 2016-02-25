using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// A visitor with methods PreVisit, PostVisit, and Visit.
	/// </summary>
	public interface IPrePostVisitor : IVisitor
	{
		/// <summary>
		/// The pre-visit method, before the actual Visit method
		/// </summary>
		/// <param name="obj">an object implementing the visiting pattern</param>
		void PreVisit(object obj);
		/// <summary>
		/// The post-visit method, after the actual Visit method
		/// </summary>
		/// <param name="obj">an object implementing the visiting pattern</param>
		void PostVisit(object obj);
		
	}

}
