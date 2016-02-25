using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Abstract implementation of the IPrePostVisitor interface
	/// </summary>
	public abstract class AbstractPrePostVisitor: IPrePostVisitor
	{
		/// <summary>
		/// Gets whether the visiting process is done
		/// </summary>
		public virtual bool IsDone
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Performs the pre-visit
		/// </summary>
		/// <param name="obj"></param>
		public virtual void PreVisit(object obj)
		{
		}
		/// <summary>
		/// The actual visiting action
		/// </summary>
		/// <param name="obj"></param>
		public virtual void Visit(object obj)
		{
		}
		/// <summary>
		/// Performs a post-visit
		/// </summary>
		/// <param name="obj"></param>
		public virtual void PostVisit(object obj)
		{
		}
	}
}
