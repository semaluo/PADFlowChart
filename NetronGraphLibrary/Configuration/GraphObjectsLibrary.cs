using System;
using Netron.GraphLib.Configuration;
namespace Netron.GraphLib.Configuration
{
	/// <summary>
	/// Collects shape and lib info of an assembly containing custom shapes and/or connections
	/// </summary>
	public class GraphObjectsLibrary
	{
		#region Fields
		/// <summary>
		/// the path to the lib
		/// </summary>
		protected string mPath;
		/// <summary>
		/// the summary collection for the custom shapes
		/// </summary>
		protected ShapeSummaryCollection shapeSummmaries;
		/// <summary>
		/// the summary collection for the custom connections
		/// </summary>
		protected ConnectionSummaryCollection conSummaries;
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public GraphObjectsLibrary()
		{
			shapeSummmaries = new ShapeSummaryCollection();
			conSummaries = new ConnectionSummaryCollection();
		}
		/// <summary>
		/// Constructor with the library-path
		/// </summary>
		/// <param name="libPath"></param>
		public GraphObjectsLibrary(string libPath):this()
		{
			this.mPath = libPath;
		}
		#endregion	
 
		#region Properties
		/// <summary>
		/// Gets or sets the mPath of the library
		/// </summary>
		public string Path
		{
			get{return mPath;}
			set{mPath = value;}
		}

		/// <summary>
		/// Gets or sets the shape summaries
		/// </summary>
		public ShapeSummaryCollection ShapeSummaries
		{
			get{return shapeSummmaries;}
			set{shapeSummmaries =value;}
		}
		/// <summary>
		/// Gets or sets the connection summaries
		/// </summary>
		public ConnectionSummaryCollection ConnectionSummaries
		{
			get{return conSummaries;}
			set{conSummaries = value;}
		}
		#endregion

	}
}
