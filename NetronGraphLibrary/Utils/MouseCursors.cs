using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Runtime.Serialization.Formatters.Soap;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.UI;

namespace Netron.GraphLib
{
	///<summary>
	///This class contains the five basic cursors necessary to visualize the various actions you can perform on the elements
	///</summary>
	///<remarks>Might be better to implement it as a structure...?</remarks>
	public class MouseCursors
	{
		/// <summary>
		/// Arrow with plus cursor
		/// </summary>
		public static Cursor Add = null;
		/// <summary>
		/// Cross cursor
		/// </summary>
		public static Cursor Cross = null;
		/// <summary>
		/// Grip cursor for connection point
		/// </summary>
		public static Cursor Grip = null;
		/// <summary>
		/// Traditional move cursor
		/// </summary>
		public static Cursor Move = null;
		/// <summary>
		/// Selection cursor
		/// </summary>
		public static Cursor Select = null;  
	}	
}
