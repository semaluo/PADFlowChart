using System;

namespace Netron.GraphLib.Configuration
{
	/// <summary>
	/// Encapsulates the essential information of a custom connection
	/// </summary>
	public class ConnectionSummary : Summary
	{
		
		#region Fields
		
			
		

		#endregion

		#region Properties
	

	

		
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public ConnectionSummary(){}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="libraryPath"></param>
		/// <param name="name"></param>
		/// <param name="key"></param>
		/// <param name="reflectionName"></param>
		public ConnectionSummary(string libraryPath,  string name, string key,  string reflectionName):base(libraryPath,name,key, reflectionName)
		{}

		#endregion
	}
}
