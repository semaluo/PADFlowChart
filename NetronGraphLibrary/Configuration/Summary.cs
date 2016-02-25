using System;

namespace Netron.GraphLib.Configuration
{
	/// <summary>
	/// Summarizes the essential elements to reflect a graph element
	/// </summary>
	public class Summary
	{
		#region Fields
		/// <summary>
		/// the description
		/// </summary>
		protected string mDescription = "No description available.";
		/// <summary>
		/// the fully qualified name to reflect the object
		/// </summary>
		protected string mReflectionName;

		/// <summary>
		/// the unique key of the object
		/// </summary>
		protected string mKey;
		/// <summary>
		/// the name of the object
		/// </summary>
		protected string mName;
		/// <summary>
		/// the name of the dll where the lib is to be found
		/// </summary>
		protected string mLibPath;
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the library path
		/// </summary>
		public string LibPath
		{
			get{return mLibPath;}
			set{mLibPath = value;}
		}
	
		/// <summary>
		/// Gets or sets the key of the summary
		/// </summary>
		public string Key
		{
			get{return mKey;}
			set{mKey = value;}
		}
		/// <summary>
		/// Gets or sets the name of the library
		/// </summary>
		public string Name
		{
			get{return mName;}
			set{mName = value;}
		}
		/// <summary>
		/// Gets or sets the summary
		/// </summary>
		public string Description
		{
			get{return mDescription;}
			set{mDescription = value;}
		}

		/// <summary>
		/// Gets or sets the fully qualified namespace to reflect
		/// </summary>
		public string ReflectionName
		{
			get{return mReflectionName;}
			set{mReflectionName = value;}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public Summary()
		{
			
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="libraryPath"></param>
		/// <param name="name"></param>
		/// <param name="key"></param>
		/// <param name="reflectionName"></param>
		public Summary(string libraryPath,  string name, string key,  string reflectionName)
		{
			this.mLibPath=libraryPath;			
			this.mName = name;			
			this.mKey = key;
			this.mReflectionName = reflectionName;
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="libraryPath"></param>
		/// <param name="name"></param>
		/// <param name="key"></param>
		/// <param name="reflectionName"></param>
		/// <param name="description"></param>
		public Summary(string libraryPath,  string name,  string key, string reflectionName, string description)
		{
			this.mLibPath=libraryPath;			
			this.mName = name;			
			this.mKey = key;
			this.mReflectionName = reflectionName;
			this.mDescription = description;
		}
		#endregion


	}
}
