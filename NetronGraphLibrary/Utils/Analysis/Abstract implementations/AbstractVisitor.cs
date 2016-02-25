using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Abstract implementation of the IVisitor interface
	/// </summary>
	public abstract class AbstractVisitor: IVisitor
	{
		/// <summary>
		/// Gets whether the visiting is done
		/// </summary>
		public virtual bool IsDone
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Performs the actual visit to the object
		/// </summary>
		/// <param name="obj"></param>
		public virtual void Visit(object obj)
		{
		}
	}

}
