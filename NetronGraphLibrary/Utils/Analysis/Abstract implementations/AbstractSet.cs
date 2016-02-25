using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Abstract base class for a set
	/// </summary>
	public abstract class AbstractSet : AbstractSearchableContainer
	{

		#region Fields
		/// <summary>
		/// the size of the set
		/// </summary>
		protected int mUniverseSize;
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets the size of the set
		/// </summary>
		public virtual int UniverseSize
		{
			get
			{
				return mUniverseSize;
			}
		}
		#endregion
		

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="mUniverseSize"></param>
		protected AbstractSet(int mUniverseSize)
		{
			this.mUniverseSize = mUniverseSize;
		}

		#endregion

		#region Methods
		/// <summary>
		/// Inserts an element in the set
		/// </summary>
		/// <param name="i"></param>
		public abstract void Insert(int i);
		/// <summary>
		/// Removes an element from the set
		/// </summary>
		/// <param name="i"></param>
		public abstract void Withdraw(int i);
		/// <summary>
		/// Return whether the given int is in the set
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public abstract bool IsMember(int i);
		/// <summary>
		/// Insert an object in the set
		/// </summary>
		/// <param name="obj"></param>
		/// <remarks>the insert object is cast internally to an int</remarks>
		public override void Insert(ComparableObject obj)
		{
			Insert((int)obj);
		}
		/// <summary>
		/// Removes an object from the set
		/// </summary>
		/// <param name="obj"></param>
		public override void Withdraw(ComparableObject obj)
		{
			Withdraw((int)obj);
		}

		/// <summary>
		/// Returns whether an object is in the set
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool IsMember(ComparableObject obj)
		{
			return IsMember((int)obj);
		}

		/// <summary>
		/// Searches for the given object in the set
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override ComparableObject Find(ComparableObject obj)
		{
			throw new InvalidOperationException();
		}

	
	
		#endregion

		
		
	}
}
