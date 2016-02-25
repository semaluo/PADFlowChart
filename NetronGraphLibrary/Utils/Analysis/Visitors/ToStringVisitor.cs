using System;
using System.Text;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Visitor taking the ToString method on the visited objects
	/// </summary>
	public class ToStringVisitor : AbstractVisitor
	{
		#region Fields
		/// <summary>
		/// the builder
		/// </summary>
		private StringBuilder builder;
		/// <summary>
		/// comma bit
		/// </summary>
		private bool comma;
		#endregion

		#region Methods
		/// <summary>
		/// Visits the given object
		/// </summary>
		/// <param name="obj"></param>
		public override void Visit(object obj)
		{
			if (comma)
			{
				builder.Append(",\n ");
			}
			builder.Append(obj);
			comma = true;
		}
		/// <summary>
		/// Returns the content of the string-builder
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return builder.ToString();
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public ToStringVisitor()
		{
			builder = new StringBuilder();
			comma = false;
			
		}
		#endregion
	}

}
