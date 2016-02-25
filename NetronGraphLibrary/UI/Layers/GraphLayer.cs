using System;
using System.Drawing;
using System.Runtime.Serialization;
//using System.Security;
using System.Security.Permissions;
namespace Netron.GraphLib
{
	/// <summary>
	/// Allows to have shapes in different layers. 
	/// </summary>
	[Serializable] public class GraphLayer : ISerializable
	{

		#region Fields
		/// <summary>
		/// opacity in percent
		/// </summary>
		private int mOpacity = 100;
		/// <summary>
		/// the layer's color
		/// </summary>
		private Color mLayerColor = Color.Gray;
		/// <summary>
		/// whether the shapes on this layer should use the layer's color
		/// instead of their own
		/// </summary>
		private bool mUseColor = true;
		/// <summary>
		/// the name of the layer
		/// </summary>
		private string mName = string.Empty;
		/// <summary>
		/// whether the layer is locked
		/// </summary>
		private bool mLocked;
		/// <summary>
		/// the layer's number in the collection
		/// </summary>
		private int mNumber ;
		/// <summary>
		/// whether the layer is visible
		/// </summary>
		private bool mVisible = true;
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets whether the layer is visible
		/// </summary>
		public bool Visible
		{
			get{return mVisible;}
			set{mVisible = value;}

		}

		/// <summary>
		/// Gets the number of the layer in the collection
		/// </summary>
		 public int Number
		{
			get{return mNumber;}
		}
		/// <summary>
		/// Gets or sets the name of the layer
		/// </summary>
		public string Name
		{
			get{return mName;}
			set{mName = value;}
		}

		/// <summary>
		/// Gets or sets whether the shapes on this layer should use the layer's color
		/// instead of their own
		/// </summary>
		public bool UseColor
		{
			get{return mUseColor;}
			set{mUseColor = value;}
		}
		/// <summary>
		/// Gets or sets the opacity of the layer
		/// </summary>
		public int Opacity
		{
			get{return mOpacity;}
			set{mOpacity = value;}
		}

		/// <summary>
		/// Gets or sets the color of the alyer
		/// </summary>
		public Color LayerColor
		{
			get{return mLayerColor;}
			set{mLayerColor = value;}
		}

		/// <summary>
		/// Gets or sets whether the layer is locked
		/// </summary>
		public bool Locked
		{
			get{return mLocked;}
			set{mLocked = value;}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default, empty constructor
		/// </summary>
		public GraphLayer()
		{
						
		}

		/// <summary>
		/// Constructor specifying the name of the layer
		/// </summary>
		/// <param name="name"></param>
		public GraphLayer(string  name)
		{
			mName = name;
		}
		/// <summary>
		/// Constructor specifying the name and the color of the layer
		/// </summary>
		/// <param name="name"></param>
		/// <param name="color"></param>
		public GraphLayer(string  name, Color color)
		{
			mName = name;
			mLayerColor = color;
		}
		/// <summary>
		/// Constructor specifying the color of the layer
		/// </summary>
		/// <param name="color"></param>
		public GraphLayer(Color color)
		{
			mLayerColor = color;
		}
		/// <summary>
		/// Constructor specifying the opacity of the layer
		/// </summary>
		/// <param name="opacity"></param>
		public GraphLayer(int opacity)
		{
			if(opacity <=100 && opacity >=0)
			{
				mOpacity = opacity;
			}
			else
				throw new Exception("The given opacity is not valid");
		}
		/// <summary>
		/// Constructor specifying the color and the opacity of the layer
		/// </summary>
		/// <param name="color"></param>
		/// <param name="opacity"></param>
		public GraphLayer(Color color, int opacity)
		{
			if(opacity <=100 && opacity >=0)
			{
				mOpacity = opacity;
				mLayerColor = color;
			}
			else
				throw new Exception("The given opacity is not valid");
		}
		/// <summary>
		/// Constructor specifying the name, the color and the opacity of the layer
		/// </summary>
		/// <param name="name"></param>
		/// <param name="color"></param>
		/// <param name="opacity"></param>
		public GraphLayer(string name, Color color, int opacity): this(color,opacity)
		{
			this.mName = name;
		}


		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public GraphLayer(SerializationInfo info, StreamingContext context)
		{
			this.mLayerColor = (Color) info.GetValue("mLayerColor", typeof(Color));

			this.mLocked = info.GetBoolean("mLocked");

			this.mName = info.GetString("mName");

			this.mNumber = info.GetInt32("mNumber");

			this.mOpacity = info.GetInt32("mOpacity");

			this.UseColor = info.GetBoolean("mUseColor");

			this.mVisible = info.GetBoolean("mVisible");
		}

		
		#endregion		
		
		#region Methods

		/// <summary>
		/// This method is not supposed to be used, the collection will set this number
		/// </summary>
		/// <param name="number"></param>
		internal void SetNumber(int number)
		{
			this.mNumber = number;
		}
		/// <summary>
		/// Overrides the default behavior to return the name of the layer
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Name;	
		}
		#endregion

		#region ISerializable Members

		/// <summary>
		/// ISerializable serialization
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("mLayerColor", this.mLayerColor, typeof(Color));

			info.AddValue("mLocked", this.mLocked);

			info.AddValue("mName", this.mName);

			info.AddValue("mNumber", this.mNumber);

			info.AddValue("mOpacity", this.mOpacity);

			info.AddValue("mUseColor", this.mUseColor);

			info.AddValue("mVisible", this.mVisible);


		}

		#endregion
	}
}
