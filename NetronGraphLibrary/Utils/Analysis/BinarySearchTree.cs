using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Implementation of a binary search tree
	/// </summary>
	public class BinarySearchTree : BinaryTree, ISearchTree, ITree, ISearchableContainer, IContainer, IComparable, IEnumerable
	{

		#region Properties
		/// <summary>
		/// Gets the min-elements
		/// </summary>
		public virtual ComparableObject Min
		{
			get
			{
				ComparableObject comparableObject;

				if (base.IsEmpty)
				{
					comparableObject = null;
				}
				else if (Left.IsEmpty)
				{
					comparableObject = Key;
				}
				else
				{
					comparableObject = Left.Min;
				}
				return comparableObject;
			}
		}

		/// <summary>
		/// Gets the max-element
		/// </summary>
		public virtual ComparableObject Max
		{
			get
			{
				ComparableObject comparableObject;

				if (base.IsEmpty)
				{
					comparableObject = null;
				}
				else if (Right.IsEmpty)
				{
					comparableObject = Key;
				}
				else
				{
					comparableObject = Right.Max;
				}
				return comparableObject;
			}
		}
		/// <summary>
		/// Gets the key of the tree
		/// </summary>
		public new virtual ComparableObject Key
		{
			get
			{
				return (ComparableObject)base.Key;
			}
		}

		/// <summary>
		/// Gets the left branch
		/// </summary>
		public new virtual BinarySearchTree Left
		{
			get
			{
				return (BinarySearchTree)base.Left;
			}
		}

		/// <summary>
		/// Gets the right branch
		/// </summary>
		public new BinarySearchTree Right
		{
			get
			{
				return (BinarySearchTree)base.Right;
			}
		}

		#endregion
		
		#region Constructor
		
		#endregion

		#region Methods

		/// <summary>
		/// Returns whether an object is in the tree
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public virtual bool IsMember(ComparableObject obj)
		{
			bool flag;

			if (base.IsEmpty)
			{
				flag = false;
			}
			else if (obj == Key)
			{
				flag = true;
			}
			else if (obj < Key)
			{
				flag = Left.IsMember(obj);
			}
			else if (obj > Key)
			{
				flag = Right.IsMember(obj);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		/// <summary>
		/// Searches an object in the tree
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public virtual ComparableObject Find(ComparableObject obj)
		{
			ComparableObject comparableObject;

			if (base.IsEmpty)
			{
				comparableObject = null;
			}
			else
			{
				int i = obj.CompareTo(Key);
				if (i == 0)
				{
					comparableObject = Key;
				}
				else if (i < 0)
				{
					comparableObject = Left.Find(obj);
				}
				else
				{
					comparableObject = Right.Find(obj);
				}
			}
			return comparableObject;
		}

		/// <summary>
		/// Inserts an object in the tree
		/// </summary>
		/// <param name="obj"></param>
		public virtual void Insert(ComparableObject obj)
		{
			if (base.IsEmpty)
			{
				base.AttachKey(obj);
			}
			else
			{
				int i = obj.CompareTo(Key);
				if (i == 0)
				{
					throw new ArgumentException("duplicate mKey");
				}
				if (i < 0)
				{
					Left.Insert(obj);
				}
				else
				{
					Right.Insert(obj);
				}
			}
			Balance();
		}
		/// <summary>
		/// Attaches a key
		/// </summary>
		/// <param name="obj"></param>
		public override void AttachKey(object obj)
		{
			if (!base.IsEmpty)
			{
				throw new InvalidOperationException();
			}
			mKey = obj;
			mLeft = new BinarySearchTree();
			mRight = new BinarySearchTree();
		}

		/// <summary>
		/// Balances the tree
		/// </summary>
		protected virtual void Balance()
		{
		}
		/// <summary>
		/// Removes an object from the tree
		/// </summary>
		/// <param name="obj"></param>
		public virtual void Withdraw(ComparableObject obj)
		{
			if (base.IsEmpty)
			{
				throw new ArgumentException("object not found");
			}
			int i = obj.CompareTo(Key);
			if (i == 0)
			{
				if (!Left.IsEmpty)
				{
					ComparableObject comparableObject1 = Left.Max;
					mKey = comparableObject1;
					Left.Withdraw(comparableObject1);
				}
				else if (!Right.IsEmpty)
				{
					ComparableObject comparableObject2 = Right.Min;
					mKey = comparableObject2;
					Right.Withdraw(comparableObject2);
				}
				else
				{
					base.DetachKey();
				}
			}
			else if (i < 0)
			{
				Left.Withdraw(obj);
			}
			else
			{
				Right.Withdraw(obj);
			}
			Balance();
		}

	
		#endregion
		
	
	}
}
