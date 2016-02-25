using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// An AVL tree is a binary search tree with a "balance" condition that ensures that the depth of the tree is O(log n), with n nodes. (AVL, by the way, comes from the initials of the creators of the data type, Adelson-Velskii and Landis.)
	/// </summary>
	public class AVLTree : BinarySearchTree
	{

		#region Fields
		/// <summary>
		/// the height of the tree
		/// </summary>
		protected int mHeight;
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets the left part of the tree
		/// </summary>
		public new AVLTree Left
		{
			get
			{
				return (AVLTree)base.Left;
			}
		}
		/// <summary>
		/// Gets the right part of the tree
		/// </summary>
		public new AVLTree Right
		{
			get
			{
				return (AVLTree)base.Right;
			}
		}

		/// <summary>
		/// Gets the height of the tree
		/// </summary>
		public override int Height
		{
			get
			{
				return mHeight;
			}
		}

		/// <summary>
		/// Gets the balance factor of the tree
		/// </summary>
		protected int BalanceFactor
		{
			get
			{
				int i;

				if (base.IsEmpty)
				{
					i = 0;
				}
				else
				{
					i = Left.Height - Right.Height;
				}
				return i;
			}
		}


		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public AVLTree()
		{
			mHeight = -1;
		}
		#endregion


		#region Methods
		/// <summary>
		/// Adjusts the height of the tree
		/// </summary>
		protected void AdjustHeight()
		{
			if (base.IsEmpty)
			{
				mHeight = -1;
			}
			else
			{
				mHeight = 1 + Math.Max(Left.Height, Right.Height);
			}
		}


		/// <summary>
		/// Performs a LL rotation
		/// </summary>
		protected void LLRotation()
		{
			if (base.IsEmpty)
			{
				throw new InvalidOperationException();
			}
			AVLTree aVLTree = Right;
			mRight = mLeft;
			mLeft = Right.mLeft;
			Right.mLeft = Right.mRight;
			Right.mRight = aVLTree;
			object local = mKey;
			mKey = Right.mKey;
			Right.mKey = local;
			Right.AdjustHeight();
			AdjustHeight();
		}

		/// <summary>
		///  Performs a RR rotation
		/// </summary>
		protected void RRRotation()
		{
			if (base.IsEmpty)
			{
				throw new InvalidOperationException();
			}
			AVLTree aVLTree = Left;
			mLeft = mRight;
			mRight = Left.mRight;
			Left.mRight = Left.mLeft;
			Left.mLeft = aVLTree;
			object local = mKey;
			mKey = Left.mKey;
			Left.mKey = local;
			Left.AdjustHeight();
			AdjustHeight();
		}

		/// <summary>
		///  Performs a LR rotation
		/// </summary>
		protected void LRRotation()
		{
			if (base.IsEmpty)
			{
				throw new InvalidOperationException();
			}
			Left.RRRotation();
			LLRotation();
		}

		/// <summary>
		///  Performs a RL rotation
		/// </summary>
		protected void RLRotation()
		{
			if (base.IsEmpty)
			{
				throw new InvalidOperationException();
			}
			Right.LLRotation();
			RRRotation();
		}

		/// <summary>
		/// Balances the tree
		/// </summary>
		protected override void Balance()
		{
			AdjustHeight();
			if (BalanceFactor > 1)
			{
				if (Left.BalanceFactor > 0)
				{
					LLRotation();
				}
				else
				{
					LRRotation();
				}
			}
			else if (BalanceFactor < -1)
			{
				if (Right.BalanceFactor < 0)
				{
					RRRotation();
				}
				else
				{
					RLRotation();
				}
			}
		}


		/// <summary>
		/// Attaches a new key
		/// </summary>
		/// <param name="obj"></param>
		public override void AttachKey(object obj)
		{
			if (!base.IsEmpty)
			{
				throw new InvalidOperationException();
			}
			mKey = obj;
			mLeft = new AVLTree();
			mRight = new AVLTree();
			mHeight = 0;
		}
		/// <summary>
		/// Detaches a key
		/// </summary>
		/// <returns></returns>
		public override object DetachKey()
		{
			mHeight = -1;
			return base.DetachKey();
		}

		#endregion
		
		
	
	
	}
}
