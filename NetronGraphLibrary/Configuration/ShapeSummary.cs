using System;

namespace Netron.GraphLib.Configuration
{
	/// <summary>
	/// Collects info about a shape from the class attributes
	/// </summary>
	public class ShapeSummary : Summary
	{
		#region Fields
		/// <summary>
		/// the shape's category
		/// </summary>
		protected string mShapeCategory;
	
		private bool mIsInternal;
		#endregion

		#region Properties
	
		/// <summary>
		/// Gets or sets the shape's category
		/// </summary>
		public string ShapeCategory
		{
			get{return mShapeCategory;}
			set{mShapeCategory = value;}
		}
		/// <summary>
		/// Gets or sets whether the shape is only for internal use (creation can only occur by code)
		/// </summary>
		public bool IsInternal
		{
			get{return mIsInternal;}
			set{mIsInternal = value;}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public ShapeSummary()
		{
			
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="libraryPath"></param>
		/// <param name="mShapeKey"></param>
		/// <param name="mShapeName"></param>
		/// <param name="mShapeCategory"></param>
		/// <param name="reflectionName"></param>
		public ShapeSummary(string libraryPath, string mShapeKey, string mShapeName, string mShapeCategory, string reflectionName)
		{
			this.mLibPath=libraryPath;
			this.mKey = mShapeKey;
			this.mName = mShapeName;
			this.mShapeCategory = mShapeCategory;
			this.mReflectionName = reflectionName;
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="libraryPath"></param>
		/// <param name="mShapeKey"></param>
		/// <param name="mShapeName"></param>
		/// <param name="mShapeCategory"></param>
		/// <param name="reflectionName"></param>
		/// <param name="mDescription"></param>
		public ShapeSummary(string libraryPath, string mShapeKey, string mShapeName, string mShapeCategory, string reflectionName, string mDescription)
		{
			this.mLibPath=libraryPath;
			this.mKey = mShapeKey;
			this.mName = mShapeName;
			this.mShapeCategory = mShapeCategory;
			this.mReflectionName = reflectionName;
			this.mDescription = mDescription;
		}

		#endregion
	}
}
