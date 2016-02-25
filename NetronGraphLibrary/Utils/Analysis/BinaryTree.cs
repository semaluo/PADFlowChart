using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Implementation of a binary tree
	/// </summary>
	public class BinaryTree : AbstractTree
	{

		#region Fields
		/// <summary>
		/// the key
		/// </summary>
		protected object	mKey;
		/// <summary>
		/// the left branch
		/// </summary>
		protected BinaryTree mLeft;
		/// <summary>
		/// the right branch
		/// </summary>
		protected BinaryTree mRight;

		#endregion
		
		#region Properties
		/// <summary>
		/// Gets the left branch
		/// </summary>
		public BinaryTree Left
		{
			get
			{
				if (base.IsEmpty)
				{
					throw new InvalidOperationException();
				}
				else
				{
					return mLeft;
				}
			}
		}

		/// <summary>
		/// Gets the right branch
		/// </summary>
		public BinaryTree Right
		{
			get
			{
				if (base.IsEmpty)
				{
					throw new InvalidOperationException();
				}
				else
				{
					return mRight;
				}
			}
		}

		/// <summary>
		/// Gets whether the branch is a leaf
		/// </summary>
		public override bool IsLeaf
		{
			get
			{
				return (!base.IsEmpty && mLeft.IsEmpty) ? mRight.IsEmpty : false;
			}
		}

		/// <summary>
		/// Gets the degree of the subtree
		/// </summary>
		public override int Degree
		{
			get
			{
				return !base.IsEmpty ? 2 : 0;
			}
		}

		/// <summary>
		/// Gets whether the key is a null key
		/// </summary>
		public override bool IsEmpty
		{
			get
			{
				return (mKey == null);
			}
		}

		/// <summary>
		/// Gets the key of the subtree
		/// </summary>
		public override object Key
		{
			get
			{
				if (base.IsEmpty)
				{
					throw new InvalidOperationException();
				}
				else
				{
					return mKey;
				}
			}
		}

		#endregion
				
		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="mKey"></param>
		/// <param name="mLeft"></param>
		/// <param name="mRight"></param>
		public BinaryTree(object mKey, BinaryTree mLeft, BinaryTree mRight)
		{
			this.mKey = mKey;
			this.mLeft = mLeft;
			this.mRight = mRight;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public BinaryTree() : this(null, null, null)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="mKey"></param>
		public BinaryTree(object mKey) : this(mKey, new BinaryTree(), new BinaryTree())
		{
		}

		#endregion

		#region Methods
		/// <summary>
		/// Empties the tree
		/// </summary>
		public override void Purge()
		{
			mKey = null;
			mLeft = null;
			mRight = null;
		}

		/// <summary>
		/// Returns a subtree
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override ITree GetSubtree(int i)
		{
			ITree tree;

			Bounds.Check(i, 0, 2);
			if (i == 0)
			{
				tree = Left;
			}
			else
			{
				tree = Right;
			}
			return tree;
		}

		/// <summary>
		/// Attaches a key
		/// </summary>
		/// <param name="obj"></param>
		public virtual void AttachKey(object obj)
		{
			if (!base.IsEmpty)
			{
				throw new InvalidOperationException();
			}
			mKey = obj;
			mLeft = new BinaryTree();
			mRight = new BinaryTree();
		}

		/// <summary>
		/// Detaches a key
		/// </summary>
		/// <returns></returns>
		public virtual object DetachKey()
		{
			if (!IsLeaf)
			{
				throw new InvalidOperationException();
			}
			object local1 = mKey;
			mKey = null;
			mLeft = null;
			mRight = null;
			return local1;
		}

		/// <summary>
		/// Attaches a subtree at the left
		/// </summary>
		/// <param name="t"></param>
		public void AttachLeft(BinaryTree t)
		{
			if (base.IsEmpty || !mLeft.IsEmpty)
			{
				throw new InvalidOperationException();
			}
			mLeft = t;
		}

		/// <summary>
		/// Detaches the left branch
		/// </summary>
		/// <returns></returns>
		public BinaryTree DetachLeft()
		{
			if (base.IsEmpty)
			{
				throw new InvalidOperationException();
			}
			BinaryTree binaryTree1 = mLeft;
			mLeft = new BinaryTree();
			return binaryTree1;
		}

		/// <summary>
		/// Attaches a subtree at the right
		/// </summary>
		/// <param name="t"></param>
		public void AttachRight(BinaryTree t)
		{
			if (base.IsEmpty || !mRight.IsEmpty)
			{
				throw new InvalidOperationException();
			}
			mRight = t;
		}

		/// <summary>
		/// Detaches the right branch
		/// </summary>
		/// <returns></returns>
		public BinaryTree DetachRight()
		{
			if (base.IsEmpty)
			{
				throw new InvalidOperationException();
			}
			BinaryTree binaryTree1 = mRight;
			mRight = new BinaryTree();
			return binaryTree1;
		}

		/// <summary>
		/// DFT of the subtree
		/// </summary>
		/// <param name="visitor"></param>
		public override void DepthFirstTraversal(IPrePostVisitor visitor)
		{
			if (!base.IsEmpty)
			{
				visitor.PreVisit(mKey);
				Left.DepthFirstTraversal(visitor);
				visitor.Visit(mKey);
				Right.DepthFirstTraversal(visitor);
				visitor.PostVisit(mKey);
			}
		}

		/// <summary>
		/// IComparable method
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override int CompareTo(object obj)
		{
			int j;

			BinaryTree binaryTree = (BinaryTree)obj;
			if (base.IsEmpty)
			{
				j = !binaryTree.IsEmpty ? -1 : 0;
			}
			else if (binaryTree.IsEmpty)
			{
				j = 1;
			}
			else
			{
				int i = ((IComparable)mKey).CompareTo(binaryTree.Key);
				if (i == 0)
				{
					i = Left.CompareTo(binaryTree.Left);
				}
				if (i == 0)
				{
					i = Right.CompareTo(binaryTree.Right);
				}
				j = i;
			}
			return j;
		}

		#endregion
		
	
	}
}
