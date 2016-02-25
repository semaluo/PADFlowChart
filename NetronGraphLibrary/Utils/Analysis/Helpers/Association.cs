using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// An association is an ordered pair of objects. 
	/// The first element of the pair is called the key ; the second element is the value  associated with the given key. 
	/// </summary>
	public class Association : ComparableObject
	{
		#region Fields
		/// <summary>
		/// the key of the association
		/// </summary>
		protected IComparable mKey;
		/// <summary>
		/// the value of the association
		/// </summary>
		protected object mValue;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the key of the association
		/// </summary>
		public IComparable Key
		{
			get
			{
				return mKey;
			}
		}

		/// <summary>
		/// Gets the value of the association
		/// </summary>
		public object Value
		{
			get
			{
				return mValue;
			}
		}

		#endregion

		#region Constructors
		/// <summary>
		/// Constructor; creates an association on the basis of a key and a value.
		/// </summary>
		/// <param name="mKey">the key of the association</param>
		/// <param name="mValue">the value of the association</param>
		public Association(IComparable mKey, object mValue)
		{
			this.mKey = mKey;
			this.mValue = mValue;
		}

		/// <summary>
		/// Constructor; creates an association with the given key and null value
		/// </summary>
		/// <param name="mKey"></param>
		public Association(IComparable mKey) : this(mKey, null)
		{
		}

		#endregion

		#region Methods
		/// <summary>
		/// Compares this association to another one.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override int CompareTo(object obj)
		{
			Association association = obj as Association;
			return mKey.CompareTo(association.mKey);
		}
		/// <summary>
		/// Overrides the default ToString to return more
		/// useful information of this association
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string str1 = String.Concat("Association {", mKey);
			if (mValue != null)
			{
				str1 = String.Concat(str1, ", ", mValue);
			}
			string str2 = String.Concat(str1, "}");
			return str2;
		}

		/// <summary>
		/// Returns a hash code.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return mKey.GetHashCode();
		}
		#endregion
	}
}
