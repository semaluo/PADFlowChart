using System;
using System.Collections;
using System.Text;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Implements an has-table data structure
	/// </summary>
	public abstract class AbstractHashTable : AbstractSearchableContainer, IHashTable, ISearchableContainer, IContainer, IComparable, IEnumerable
	{
		/// <summary>
		/// Gets the length of the hashtable
		/// </summary>
		public abstract int Length
		{
			get;
		}
		/// <summary>
		/// Gets the load-factor 
		/// </summary>
		public virtual double LoadFactor
		{
			get
			{
				return (double)base.Count / (double)Length;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected static int F(object obj)
		{
			return obj.GetHashCode();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		protected int G(int x)
		{
			return Math.Abs(x) % this.Length;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected int H(object obj)
		{
			return G(F(obj));
		}


		
		/// <summary>
		/// Returns the octal
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		private static string Octal(int arg)
		{
			int i = arg;
			char[] chs = new char[12];
			int j = 0;
			for (; i != 0; i /= 8)
			{
				chs[j++] = (char)(i % 8 + 48);
			}
			chs[j++] =(char) 48;
			StringBuilder stringBuilder = new StringBuilder();
			while (j > 0)
			{
				stringBuilder.Append(chs[--j]);
			}
			return stringBuilder.ToString();
		}

	
	}
}
