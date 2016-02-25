using System;
using System.Text;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Abstract base class for the other classes
	/// </summary>
	public abstract class AbstractContainer : ComparableObject, IContainer, IComparable, IEnumerable
	{
		
		#region Fields

		/// <summary>
		/// the numer of elements in this container
		/// </summary>
		protected int mCount;

		#endregion
	
		#region Properties

		/// <summary>
		/// Gets the number of elements in the container
		/// </summary>
		public virtual int Count
		{
			get
			{
				return mCount;
			}
		}

		/// <summary>
		/// Gets whether the container is empty
		/// </summary>
		public virtual bool IsEmpty
		{
			get
			{
				return Count == 0;
			}
		}

		/// <summary>
		/// Gets whether the container is full
		/// </summary>
		public virtual bool IsFull
		{
			get
			{
				return false;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Accepts a visitor to the container
		/// </summary>
		/// <param name="visitor"></param>
		public virtual void Accept(IVisitor visitor)
		{
			IEnumerator iEnumerator = GetEnumerator();
			while (iEnumerator.MoveNext() && !visitor.IsDone)
			{
				visitor.Visit(iEnumerator.Current);
			}
		}

		/// <summary>
		/// Overrides the ToString method to return more useful information
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			IVisitor visitor = new ToStringVisitor();
			Accept(visitor);
			return String.Concat(new object[]{base.GetType().FullName, " {", visitor, "}"});
		}

		/// <summary>
		/// Overrides the GetHashCode to return a better hascode
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			IVisitor visitor = new HashCodeVisitor();
			Accept(visitor);
			return base.GetType().GetHashCode() + visitor.GetHashCode();
		}

		/// <summary>
		/// Empties the container
		/// </summary>
		public abstract void Purge();

		/// <summary>
		/// Returns an enumerator to loop over the container elements
		/// </summary>
		/// <returns></returns>
		public abstract IEnumerator GetEnumerator();
		#endregion
		
	}
}
