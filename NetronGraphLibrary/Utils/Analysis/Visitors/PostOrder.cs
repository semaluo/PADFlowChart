using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Adapts a given IVisitor to a post-visitor
	/// </summary>
	public class PostOrder : AbstractPrePostVisitor
	{
		/// <summary>
		/// the inner visitor
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
		public PostOrder(IVisitor visitor)
		{
			this.visitor = visitor;
		}

		/// <summary>
		/// The post-visit method
		/// </summary>
		/// <param name="obj"></param>
		public override void PostVisit(object obj)
		{
			visitor.Visit(obj);
		}
	}
}
