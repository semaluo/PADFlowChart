using System;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Diagnostics;
namespace Netron.GraphLib
{
	/// <summary>
	/// Allows to add meta-info to the graph; author, description, date and so on
	/// </summary>
	[Serializable] public class GraphInformation : ISerializable
	{

		#region Fields
		/// <summary>
		/// the description of the graph
		/// </summary>
		private string mDescription = string.Empty;
		/// <summary>
		/// the author of the graph
		/// </summary>
		private string mAuthor = string.Empty;
		/// <summary>
		/// the creation date of the graph
		/// </summary>
		private string mCreationDate = string.Empty;
		/// <summary>
		/// the subject of the graph
		/// </summary>
		private string mSubject = string.Empty;
		/// <summary>
		/// the title of the graph
		/// </summary>
		private string mTitle = string.Empty;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the description of the graph
		/// </summary>
		public string Description
		{
			get{return mDescription;}
			set{mDescription = value;}
		}

		/// <summary>
		/// Gets or sets the author of the graph
		/// </summary>
		public string Author
		{
			get{return mAuthor;}
			set{mAuthor = value;}
		}
		/// <summary>
		/// Gets or sets the creation date of the graph
		/// </summary>
		public string CreationDate
		{
			get{return mCreationDate;}			
			set{mCreationDate = value;}
		}
		/// <summary>
		/// Gets or sets the subject of the graph
		/// </summary>
		public string Subject
		{
			get{return mSubject;}
			set{mSubject = value;}
		}
		/// <summary>
		/// Gets or sets the title of the graph
		/// </summary>
		public string Title
		{
			get{return mTitle;}
			set{mTitle = value;}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default ctor
		/// </summary>
		public GraphInformation()
		{
			mCreationDate = DateTime.Now.ToUniversalTime().ToString();
		}

		/// <summary>
		/// Constructs an ew instance with the given title
		/// </summary>
		/// <param name="title"></param>
		public GraphInformation(string title):this()
		{
			mTitle = title;
		}
		/// <summary>
		/// Constructs a new instance with the given title and author
		/// </summary>
		/// <param name="title"></param>
		/// <param name="author"></param>
		public GraphInformation(string title, string author):this(title)
		{
			mAuthor = author;
		}

		/// <summary>
		/// Constructs a new instance with the given title, subject and author
		/// </summary>
		/// <param name="title"></param>
		/// <param name="author"></param>
		/// <param name="subject"></param>
		public GraphInformation(string title, string author, string subject):this(title, author)
		{
			mSubject = subject;
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public GraphInformation(SerializationInfo info, StreamingContext context)
		{
			this.mAuthor =  info.GetString("mAuthor");
			this.mCreationDate = info.GetString("mCreationDate");
			this.mDescription = info.GetString("mDescription");
			this.mSubject = info.GetString("mSubject");
			this.mTitle = info.GetString("mTitle");
		}
		#endregion

		/// <summary>
		/// ISerializable implementation
		/// </summary>
		/// <param name="info">the serialization info</param>
		/// <param name="context">the streaming context</param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			
			info.AddValue("mAuthor",this.mAuthor);
			

			info.AddValue("mCreationDate",this.mCreationDate);
			

			info.AddValue("mDescription",this.mDescription);
			

			info.AddValue("mTitle",this.mTitle);
			

			info.AddValue("mSubject",this.mSubject);
			
			
		}
	}
}
