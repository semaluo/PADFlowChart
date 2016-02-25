using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Abstract base class for a tree implementation
	/// </summary>
	public abstract class AbstractTree : AbstractContainer, ITree, IContainer, IComparable, IEnumerable
	{
		#region Enumerator class
		/// <summary>
		/// Enumerator for the tree
		/// </summary>
		protected class Enumerator: IEnumerator
		{
			#region Fields
			/// <summary>
			/// the underlying ITree
			/// </summary>
			private ITree tree;
			/// <summary>
			/// a stack object to walk through the tree
			/// </summary>
			private IStack stack;
			#endregion

			#region Properties
			/// <summary>
			/// Gets the current object in the enumerator
			/// </summary>
			public  virtual object Current
			{
				get
				{
					if (stack.IsEmpty)
					{
						throw new InvalidOperationException();
					}
					else
					{
						return ((ITree)stack.Top).Key;
					}
				}
			}
			#endregion

			#region Constructor
			/// <summary>
			/// Default constructor
			/// </summary>
			/// <param name="tree"></param>
			public Enumerator(ITree tree)
			{
				this.tree = tree;
				stack = new StackAsLinkedList();
			}
			#endregion

			#region Methods
			/// <summary>
			/// Moves the internal pointer to the next object in the tree
			/// </summary>
			/// <returns></returns>
			public  virtual bool MoveNext()
			{
				if (!stack.IsEmpty)
				{
					ITree tree1 = (ITree)stack.Pop();
					for (int i = tree1.Degree - 1; i >= 0; i--)
					{
						ITree tree2 = tree1.GetSubtree(i);
						if (!tree2.IsEmpty)
						{
							stack.Push(tree2);
						}
					}
				}
				else if (!tree.IsEmpty)
				{
					stack.Push(tree);
				}
				bool flag = stack.IsEmpty == false;
				return flag;
			}

			/// <summary>
			/// Resets the internal pointer of the enumerator
			/// </summary>
			public  virtual void Reset()
			{
				stack.Purge();
			}
			#endregion
		}

		#endregion

		#region Properties
		/// <summary>
		/// Gets the height of the tree, i.e. the number of sub-levels.
		/// </summary>
		public virtual int Height
		{
			get
			{
				int k;

				if (base.IsEmpty)
				{
					k = -1;
				}
				else
				{
					int i = -1;
					for (int j = 0; j < Degree; j++)
					{
						i = Math.Max(i, GetSubtree(j).Height);
					}
					k = i + 1;
				}
				return k;
			}
		}

		/// <summary>
		/// Gets the number of elements in the tree
		/// </summary>
		public override int Count
		{
			get
			{
				int k;

				if (IsEmpty)
				{
					k = 0;
				}
				else
				{
					int i = 1;
					for (int j = 0; j < Degree; j++)
					{
						i += GetSubtree(j).Count;
					}
					k = i;
				}
				return k;
			}
		}

		/// <summary>
		/// Abstract property, the implementation needs to return a key object
		/// </summary>
		public abstract object Key
		{
			get;
		}

		/// <summary>
		/// Abstract property, the implementation needs to return if the element is a leaf
		/// </summary>
		public abstract bool IsLeaf
		{
			get;
		}

		/// <summary>
		/// The degree of a node is the number of subtrees associated with that node
		/// </summary>
		public abstract int Degree
		{
			get;
		}

		#endregion

		#region Methods
		/// <summary>
		/// Performs a DFT of the tree
		/// </summary>
		/// <param name="visitor"></param>
		public virtual void DepthFirstTraversal(IPrePostVisitor visitor)
		{
			if (!visitor.IsDone && !base.IsEmpty)
			{
				visitor.PreVisit(Key);
				for (int i = 0; i < Degree; i++)
				{
					GetSubtree(i).DepthFirstTraversal(visitor);
				}
				visitor.PostVisit(Key);
			}
		}

		/// <summary>
		/// Performs a BFT of the tree
		/// </summary>
		/// <param name="visitor"></param>
		public virtual void BreadthFirstTraversal(IVisitor visitor)
		{
			QueueAsLinkedList queue = new QueueAsLinkedList() ;
			if (!base.IsEmpty)
			{
				queue.Enqueue(this);
			}
			while (!queue.IsEmpty && !visitor.IsDone)
			{
				ITree tree1 = (ITree)queue.Dequeue();
				visitor.Visit(tree1.Key);
				for (int i = 0; i < tree1.Degree; i++)
				{
					ITree tree2 = tree1.GetSubtree(i);
					if (!tree2.IsEmpty)
					{
						queue.Enqueue(tree2);
					}
				}
			}
		}

		/// <summary>
		/// Accepts a visiting process with the given visitor
		/// </summary>
		/// <param name="visitor"></param>
		public override void Accept(IVisitor visitor)
		{
			DepthFirstTraversal(new PreOrderVisitor(visitor));
		}

		/// <summary>
		/// Return an IEnumerator for the tree
		/// </summary>
		/// <returns></returns>
		public override IEnumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		/// <summary>
		/// Returns the sub-tree of the i-th node
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public abstract ITree GetSubtree(int i);
		#endregion
		
	}
}
