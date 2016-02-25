using System;
using System.Collections;
namespace Netron.GraphLib.Attributes
{
	/// <summary>
	/// Abstract base class for the attributes related to serialization
	/// </summary>
	public abstract class NetronGraphAttribute : System.Attribute
	{
		#region Fields
		/// <summary>
		/// the key of the shape, usually a GUID
		/// </summary>
		protected string mKey;
		/// <summary>
		/// the name of the shape
		/// </summary>
		protected string mName;
		/// <summary>
		/// the full name of the shape to reflect
		/// </summary>
		/// 
		protected string mReflectionName;
		/// <summary>
		/// a description
		/// </summary>
		protected string mDescription = "No mDescription available.";
		/// <summary>
		/// whether the shape is only accessible via code or internally
		/// </summary>
		protected bool mIsInternal;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the full name to reflect the shape
		/// </summary>
		public string ReflectionName
		{
			get{return mReflectionName;}
			set{mReflectionName = value;}
		}
		/// <summary>
		/// Gets a mDescription of the shape
		/// </summary>
		public string Description
		{
			get{return mDescription;}
		}
		/// <summary>
		/// Gets the unique identifier of the shape
		/// </summary>
		public string Key
		{
			get{return mKey;}
		}
		/// <summary>
		/// Gets the shape name
		/// </summary>
		public string Name
		{
			get{return mName;}
		}
		/// <summary>
		/// Gets whether the entity is available via the interface or false if only via code
		/// </summary>
		public bool IsInternal
		{
			get{return mIsInternal;}
		}
		#endregion
	}
	/// <summary>
	/// Attribute to tag a class as a Netron graph shape
	/// </summary>
	[Serializable, AttributeUsage(AttributeTargets.Class)] public  class NetronGraphShapeAttribute : NetronGraphAttribute
	{
		#region Fields
	
		/// <summary>
		/// the cateogry under which it will stay
		/// </summary>
		protected string mShapeCategory;		

		#endregion

		#region Properties
		
		
		/// <summary>
		/// Gets the category of the shape under which it will reside in a viewer
		/// </summary>
		public string ShapeCategory
		{
			get{return mShapeCategory;}
			
		}	
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor, marks a class as a shape-class for the Netron graph library
		/// </summary>
		/// <param name="mShapeName"></param>
		/// <param name="mShapeKey"></param>
		/// <param name="mShapeCategory"></param>
		/// <param name="reflectionName"></param>
		public NetronGraphShapeAttribute(string mShapeName, string mShapeKey, string mShapeCategory, string reflectionName)
		{
			this.mName = mShapeName;
			this.mKey = mShapeKey;
			this.mShapeCategory=mShapeCategory;
			this.mReflectionName = reflectionName;
		}
		/// <summary>
		/// Constructor, marks a class as a shape-class for the Netron graph library
		/// </summary>
		/// <param name="mShapeName"></param>
		/// <param name="mShapeKey"></param>
		/// <param name="mShapeCategory"></param>
		/// <param name="reflectionName"></param>
		/// <param name="mDescription"></param>
		public NetronGraphShapeAttribute(string mShapeName, string mShapeKey, string mShapeCategory, string reflectionName, string mDescription)
		{
			this.mName = mShapeName;
			this.mKey = mShapeKey;
			this.mShapeCategory=mShapeCategory;
			this.mReflectionName = reflectionName;
			this.mDescription = mDescription;
		}
		/// <summary>
		/// Constructor, marks a class as a shape-class for the Netron graph library
		/// </summary>
		/// <param name="mShapeName"></param>
		/// <param name="mShapeKey"></param>
		/// <param name="mShapeCategory"></param>
		/// <param name="reflectionName"></param>
		/// <param name="mDescription"></param>
		/// <param name="internalUsage"></param>
		public NetronGraphShapeAttribute(string mShapeName, string mShapeKey, string mShapeCategory, string reflectionName, string mDescription, bool internalUsage)
		{
			this.mName = mShapeName;
			this.mKey = mShapeKey;
			this.mShapeCategory=mShapeCategory;
			this.mReflectionName = reflectionName;
			this.mDescription = mDescription;
			this.mIsInternal = internalUsage;
		}
		#endregion

		#region Methods

		
		#endregion
	}

	/// <summary>
	/// Attribute to tag a class as a Netron graph connection
	/// </summary>
	[Serializable] public  class NetronGraphConnectionAttribute : NetronGraphAttribute
	{
		#region Fields		
		

		#endregion

		#region Properties
		
		
		#endregion

		#region Constructor
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="connectionName"></param>
		/// <param name="key"></param>
		/// <param name="reflectionName"></param>
		public NetronGraphConnectionAttribute(  string connectionName, string key, string reflectionName)
		{
			this.mKey = key;			
			this.mName = connectionName;			
			this.mReflectionName = reflectionName;
		}
		
		#endregion

		#region Methods

		
		#endregion
	}

	/// <summary>
	/// Attribute to tag a class as a Netron graph connection
	/// </summary>
	[Serializable, AttributeUsage(AttributeTargets.Class)] public  class ConnectionStyleAttribute : System.Attribute
	{
		/// <summary>
		/// ArrayList of extra connection-styles
		/// </summary>
		protected ArrayList mExtraStyles;
		/// <summary>
		/// Constructor, marks a class as custom connection
		/// </summary>
		/// <param name="extra"></param>
		public ConnectionStyleAttribute(ArrayList extra)
		{
			mExtraStyles = extra;
		}
		/// <summary>
		/// Constructor, marks a class as a custom connection
		/// </summary>
		public ConnectionStyleAttribute()
		{
			
		}
		/// <summary>
		/// Gets or sets the ArrayList of custom connection styles
		/// </summary>
		public ArrayList ExtraStyles
		{
			get{return mExtraStyles;}
			set{mExtraStyles = value;}
		}


	}
	/// <summary>
	/// Attribute to tag a class as a Netron graph layer
	/// </summary>
	[Serializable, AttributeUsage(AttributeTargets.Class)] public  class GraphLayerAttribute : System.Attribute
	{
		/// <summary>
		/// STC of layers
		/// </summary>
		protected GraphLayerCollection mLayers;
		/// <summary>
		/// Constructor, marks a class as a graph-layer
		/// </summary>
		/// <param name="layers"></param>
		public GraphLayerAttribute(GraphLayerCollection layers)
		{
			this.mLayers = layers;
		}
		/// <summary>
		/// Constructor, marks a class as a graph-layer
		/// </summary>
		public GraphLayerAttribute()
		{
			
		}
		/// <summary>
		/// Gets or sets the layer-collection
		/// </summary>
		public GraphLayerCollection Layers
		{
			get{return mLayers;}
			set{mLayers = value;}
		}


	}

	/// <summary>
	/// Attribute to tag a class as a reflected enum type.
	/// This solves the problem that an Enum type in a reflected assembly is not available in the propertygrid.
	/// </summary>
	[Serializable] public  class ReflectedEnumAttribute : System.Attribute
	{
		/// <summary>
		/// ArrayList of extra connection-styles
		/// </summary>
		protected ArrayList mEnums;
		/// <summary>
		/// Constructor, marks a class as custom connection
		/// </summary>
		/// <param name="extra"></param>
		public ReflectedEnumAttribute(ArrayList extra)
		{
			mEnums = extra;
		}
		/// <summary>
		/// Constructor, marks a class as a custom connection
		/// </summary>
		public ReflectedEnumAttribute()
		{
			
		}
		/// <summary>
		/// Gets or sets the ArrayList of custom connection styles
		/// </summary>
		public ArrayList Enums
		{
			get{return mEnums;}
			set{mEnums = value;}
		}


	}
	

}
