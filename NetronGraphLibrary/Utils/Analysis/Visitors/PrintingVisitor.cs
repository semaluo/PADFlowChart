using System;
using System.Diagnostics;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Printing visitor; prints to the Console and the Trace
	/// </summary>
	public class PrintingVisitor : AbstractVisitor
	{
		/// <summary>
		/// the comma bit
		/// </summary>
		private bool comma;

		/// <summary>
		/// The actual visiting action
		/// </summary>
		/// <param name="obj"></param>
		public override void Visit(object obj)
		{
			if (comma)
			{
				Trace.WriteLine(",");
				Console.Write(", ");
			}
			Console.Write(obj);
			Trace.WriteLine(obj);
			comma = true;
		}

		/// <summary>
		/// Finishing text
		/// </summary>
		public void Finish()
		{
			Console.WriteLine("\nFinish of the printing visitor.");
			Trace.WriteLine("\nFinish of the printing visitor.");
			comma = false;
		}
		/// <summary>
		/// Constructor
		/// </summary>
		public PrintingVisitor()
		{
			comma = false;

		}
	}




}
