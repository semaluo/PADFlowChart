using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Abstract base class implementing the IComparable interface
	/// </summary>
	public abstract class ComparableObject: IComparable
	{
		#region Methods
		/// <summary>
		/// Abstract IComparable.CompareTo method
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public abstract int CompareTo(object obj);
		/// <summary>
		/// Comparison method
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		private int Compare(object obj)
		{
			int i;

			if (base.GetType() == obj.GetType())
			{
				i = CompareTo(obj);
			}
			else
			{
				i = base.GetType().FullName.CompareTo(obj.GetType().FullName);
			}
			return i;
		}

		/// <summary>
		/// Equals method
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return Compare(obj) == 0;
		}

		/// <summary>
		/// Returns the hashcode
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion

		#region Operator overloading
		/// <summary>
		/// == operator overloading
		/// </summary>
		/// <param name="c"></param>
		/// <param name="o"></param>
		/// <returns></returns>
		public static bool operator == (ComparableObject c, object o)
		{
			if ( o == null || c==null )
			{
				 return ((o as ComparableObject == c )== true);
			}		
			else
			{
				return  (c.Compare(o) == 0);
			}
			
		}

		/// <summary>
		/// != operator overloading
		/// </summary>
		/// <param name="c"></param>
		/// <param name="o"></param>
		/// <returns></returns>
		public static bool operator != (ComparableObject c, object o)
		{
			bool flag;

			if (c == null || o == null)
			{
				flag = c == o == false;
			}
			else
			{
				flag = c.Compare(o) == 0 == false;
			}
			return flag;
		}

		/// <summary>
		/// smaller-than operator overloading
		/// </summary>
		/// <param name="c"></param>
		/// <param name="o"></param>
		/// <returns></returns>
		public static bool operator < (ComparableObject c, object o)
		{
			return c.Compare(o) < 0;
		}

		/// <summary>
		/// bigger-than operator overloading
		/// </summary>
		/// <param name="c"></param>
		/// <param name="o"></param>
		/// <returns></returns>
		public static bool operator > (ComparableObject c, object o)
		{
			return c.Compare(o) > 0;
		}

		/// <summary>
		/// smaller-than-or-equal operator overloading
		/// </summary>
		/// <param name="c"></param>
		/// <param name="o"></param>
		/// <returns></returns>
		public static bool operator <= (ComparableObject c, object o)
		{
			return c.Compare(o) > 0 == false;
		}

		/// <summary>
		/// bigger-than-or-equal operator overloading
		/// </summary>
		/// <param name="c"></param>
		/// <param name="o"></param>
		/// <returns></returns>
		public static bool operator >= (ComparableObject c, object o)
		{
			return c.Compare(o) < 0 == false;
		}

		/// <summary>
		/// ComparableObject with char operator overloading
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static implicit operator ComparableObject(char c)
		{
			return new ComparableChar(c);
		}

		/// <summary>
		/// char operator overloading
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static explicit operator char(ComparableObject c)
		{
			return (char)(ComparableChar)c;
		}

		/// <summary>
		/// ComparableObject with int operator overloading
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public static implicit operator ComparableObject(int i)
		{
			return new ComparableInt32(i);
		}

		/// <summary>
		/// int operator overloading
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static explicit operator int(ComparableObject c)
		{
			return (int)(ComparableInt32)c;
		}

		/// <summary>
		/// ComparableObject with double operator overloading
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public static implicit operator ComparableObject(double d)
		{
			return new ComparableDouble(d);
		}

		/// <summary>
		/// double operator overloading
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static explicit operator double(ComparableObject c)
		{
			return (double)(double)(ComparableDouble)c;
		}

		/// <summary>
		/// ComparableObject with string operator overloading
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static implicit operator ComparableObject(string s)
		{
			return new ComparableString(s);
		}

		/// <summary>
		/// string operator overloading
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static explicit operator string(ComparableObject c)
		{
			return (string)(ComparableString)c;
		}

		
		#endregion
	}
}
