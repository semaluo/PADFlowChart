using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
namespace Netron.GraphLib
{
	/// <summary>
	/// Provides data for the GetValue and SetValue events of the PropertyBag class.
	/// </summary>
	public class PropertySpecEventArgs : EventArgs
	{
		#region Fields
		private PropertySpec property;
		private object val;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the current value of the property.
		/// </summary>
		public object Value
		{
			get { return val; }
			set { val = value; }
		}
		/// <summary>
		/// Gets the PropertySpec that represents the property whose value is being
		/// requested or set.
		/// </summary> 
		public PropertySpec Property
		{
			get
			{ 
				return property;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the PropertySpecEventArgs class.
		/// </summary>
		/// <param name="property">The PropertySpec that represents the property whose
		/// value is being requested or set.</param>
		/// <param name="val">The current value of the property.</param>
		public PropertySpecEventArgs(PropertySpec property, object val)
		{
			this.property = property;
			this.val = val;
		}
		
		
		#endregion
	}

}
