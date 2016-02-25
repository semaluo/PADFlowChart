using System;
using System.Windows.Forms;

namespace Netron.GraphLib
{
	/// <summary>
	/// Event argument to pass generic string information
	/// </summary>
	public class InfoEventArgs : EventArgs
	{

		/// <summary>
		/// the message
		/// </summary>
		private string mMessage = string.Empty;


		/// <summary>
		/// Gets or sets the info message
		/// </summary>
		public string Message
		{
			get{return mMessage;}
			set{mMessage = value;}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message"></param>
		public InfoEventArgs(string message) : base()
		{
			mMessage = message;
		}
	}
}
