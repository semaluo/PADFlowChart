using System;
using System.Drawing;
using System.Windows.Forms;
using Netron.GraphLib.Configuration;
namespace Netron.GraphLib.Interfaces
{
	/// <summary>
	/// Entity interface
	/// </summary>
	public interface IEntity : IPaintable
	{
		#region Properties
		/// <summary>
		/// Gets or sets the unique identifier of the entity
		/// </summary>
		Guid UID {get; set;}
		
		/// <summary>
		/// Gets the layer to which the entity belongs
		/// </summary>
		GraphLayer Layer {get;}
		/// <summary>
		/// Gets or sets the tag-object attached to the entity
		/// </summary>
		object Tag {get; set;}
		/// <summary>
		/// Gets the propertybag of the entity
		/// </summary>
		PropertyBag Properties {get;}
		/// <summary>
		/// Gets or sets the text of the entity
		/// </summary>
		string Text {get; set;}
		/// <summary>
		/// Gets the tracker of the entity
		/// </summary>
		Tracker Tracker {get; set;}
		#endregion

		#region Methods
		/// <summary>
		/// Sets the layer to which the entity belongs
		/// </summary>
		/// <param name="name"></param>
		void SetLayer(string name);
		/// <summary>
		/// Sets the layer to which the entity belongs
		/// </summary>
		/// <param name="index"></param>
		void SetLayer(int index);
		/// <summary>
		/// Adds the properties of the entity to the bag
		/// </summary>
		void AddProperties();
		
		/// <summary>
		/// Gets the Summary for this entity
		/// </summary>
		Summary Summary {get;}
		/// <summary>
		/// Gets the cursor when the mouse is hovering the given point in the entity
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		Cursor GetCursor(PointF p);
		/// <summary>
		/// Says wether, for the given rectangle, the underlying shape is contained in it.
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		bool Hit(RectangleF r);
		/// <summary>
		/// Post-deserialization actions
		/// </summary>
		void PostDeserialization();
		
		#endregion

		
	}
}
