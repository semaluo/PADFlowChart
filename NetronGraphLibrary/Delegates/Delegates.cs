using System;
using System.IO;
namespace Netron.GraphLib
{
	/// <summary>
	/// The signature of a show-properties event
	/// </summary>
	public delegate void PropertiesInfo(object sender, object[] props);

	/// <summary>
	/// The signature of the show-description-on-hover event
	/// </summary>
	public delegate void ItemDescription(object sender, InfoEventArgs e);

	/// <summary>
	/// when a new connection is added event 
	/// </summary>
	public delegate bool ConnectionInfo(object sender, ConnectionEventArgs e);
	/// <summary>
	/// The general purpose output delegate 
	/// </summary>
	public delegate void InfoDelegate(object sender, OutputInfoLevels level);

	/// <summary>
	/// when a new shape is added
	/// </summary>
	public delegate void ShapeInfo(object sender, Shape shape);

	/// <summary>
	/// File information delegate
	/// </summary>
	public delegate void FileInfo(object sender, System.IO.FileInfo info);



}