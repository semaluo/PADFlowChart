using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Base class for a PrePostVisitor
	/// </summary>
	public class InOrder : AbstractPrePostVisitor
	{
		/// <summary>
		/// the visitor
		/// </summary>
		protected IVisitor visitor;
		/// <summary>
		/// Gets whether the visiting is done
		/// </summary>
		public override bool IsDone
		{
			get
			{
				return visitor.IsDone;
			}
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="visitor"></param>
		public InOrder(IVisitor visitor)
		{
			this.visitor = visitor;
		}
		/// <summary>
		/// Visits the given object
		/// </summary>
		/// <param name="obj"></param>
		public override void Visit(object obj)
		{
			visitor.Visit(obj);
		}
	}
}
