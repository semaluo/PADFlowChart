using System;
using System.Collections;
namespace Netron.GraphLib
{
	/// <summary>
	/// STC of strings
	/// </summary>
	public class StringCollection : CollectionBase
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public StringCollection()
		{
			
		}
		/// <summary>
		/// Adds a string to the collection
		/// </summary>
		/// <param name="value">a string</param>
		/// <returns></returns>
		public int Add(string value)
		{
			return this.InnerList.Add(value);
		}

		/// <summary>
		/// Integer indexer
		/// </summary>
		public string this[int index]
		{
			get
			{
				if(index<0 || index>=this.Count)
					throw new IndexOutOfRangeException("The index does not fall in the range; count: " + this.Count);
				return (string) this.InnerList[index];
			}
		}

		
		/// <summary>
		/// Returns whether the given string is in the collection
		/// </summary>
		/// <param name="value">a string</param>
		/// <returns>Returns the index in the collection if found, otherwise -1.</returns>
		public int Contains(string value)
		{
			for(int k=0; k<this.Count; k++)
			{
				if((string) this.InnerList[k]==value) return k;
			}
			return -1;
		}
	}
}
