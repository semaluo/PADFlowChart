using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Abstract base class implementaing the IComparable interface
	/// </summary>
	public abstract class ComparableValue : ComparableObject
	{
		/// <summary>
		/// an IComparable object
		/// </summary>
		protected IComparable obj;

		/// <summary>
		/// Gets the actual object
		/// </summary>
		public virtual object Object
		{
			get
			{
				return obj;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="obj"></param>
		protected ComparableValue(IComparable obj)
		{
			this.obj = obj;
		}
		/// <summary>
		/// IComparable.CompareTo implementation
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		public override int CompareTo(object arg)
		{
			int i;

			ComparableValue comparableValue = arg as ComparableValue;
			if (obj.GetType() == comparableValue.obj.GetType())
			{
				i = obj.CompareTo(comparableValue.obj);
			}
			else
			{
				i = obj.GetType().FullName.CompareTo(comparableValue.obj.GetType().FullName);
			}
			return i;
		}
		/// <summary>
		/// Gets the hashcode
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return obj.GetHashCode();
		}	
		/// <summary>
		/// ToString overriden base method
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return obj.ToString();
		}
	}

}
