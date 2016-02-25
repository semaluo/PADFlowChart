using System;

namespace Netron.GraphLib.Interfaces
{
	/// <summary>
	/// Describes a reporting utility
	/// </summary>
	public interface IReporter
	{
		/// <summary>
		/// Returns a report, the datatype depends on the actual reporting implementation
		/// </summary>
		/// <returns></returns>
		object Report();
	}
}
