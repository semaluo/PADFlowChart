using System;
using System.Collections;
namespace Netron.GraphLib
{
	/// <summary>
	/// Utility class to keep track which shapes have been visited or positioned (during the layout process).
	/// </summary>
	class VisitCollection : DictionaryBase
	{
			
		public bool this[string uid]
		{
			get{return (bool) this.InnerHashtable[uid];}
			set{this.InnerHashtable[uid]=value;}
		}

		
	}
}
