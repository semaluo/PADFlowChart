using System;
using System.ComponentModel;
namespace Netron.GraphLib.Utils
{
	/// <summary>
	/// Summary description for NListItem.
	/// </summary>
	[DefaultProperty("Text")] public class NListItem
	{
		#region Fields
		/// <summary>
		/// the text
		/// </summary>
		protected string mText = string.Empty;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the text
		/// </summary>
		public string Text
		{
			get{return mText;}
			set{mText = value;}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text"></param>
		public NListItem(string text)
		{
			this.mText = text;
		}
		/// <summary>
		/// Constructor
		/// </summary>
		public NListItem(){}

		#endregion

		#region Methods

		/// <summary>
		/// Overrides the base method to return the text
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return mText;
		}
		#endregion

	}
}
