using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
namespace Netron.GraphLib
{
	

	/// <summary>
	/// Represents the method that will handle the GetValue and SetValue events of the
	/// PropertyBag class.
	/// </summary>
	[Serializable] public delegate void PropertySpecEventHandler(object sender, PropertySpecEventArgs e);
}
