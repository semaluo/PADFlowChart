using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Turns a visitor in a pre-visitor which will be perofmed before the actual visit
	/// </summary>
	public class PreOrderVisitor : AbstractPrePostVisitor
	{
		#region Fields
		/// <summary>
		/// the IVisitor 
		/// </summary>
		protected IVisitor visitor;
		#endregion

		#region Properties
		/// <summary>
		/// Gets whether the visit is done
		/// </summary>
		public override bool IsDone
		{
			get
			{
				return visitor.IsDone;
			}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="visitor"></param>
		public PreOrderVisitor(IVisitor visitor)
		{
			this.visitor = visitor;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Uses the IVisitor.Visit as a pre-visit
		/// </summary>
		/// <param name="obj"></param>
		public override void PreVisit(object obj)
		{
			visitor.Visit(obj);
		}
		#endregion
	}
}
