using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Implementation of the partition data structure
	/// </summary>
	public class PartitionAsForest : AbstractSet, IPartition, ISet, ISearchableContainer, IContainer, IComparable, IEnumerable
	{
		#region PartititonTree class definition
		/// <summary>
		/// Partition implementation as a tree
		/// </summary>
		protected class PartitionTree : AbstractSet, ISet, ISearchableContainer, ITree, IContainer, IComparable, IEnumerable
		{
			#region Fields
			/// <summary>
			/// the inner partition
			/// </summary>
			protected PartitionAsForest partition;
			/// <summary>
			/// an item
			/// </summary>
			internal int item;
			/// <summary>
			/// the parent tree
			/// </summary>
			internal PartitionTree parent;
			/// <summary>
			/// the rank
			/// </summary>
			internal int rank;

			#endregion

			#region Properties
			/// <summary>
			/// Gets the number of elements in the tree
			/// </summary>
			public new virtual int Count
			{
				get
				{
					return mCount;
				}

				set
				{
					mCount = value;
				}
			}

			/// <summary>
			/// Gets the height of the tree
			/// </summary>
			public virtual int Height
			{
				get
				{
					return rank;
				}
			}

			/// <summary>
			/// Gets the key of the tree
			/// </summary>
			public virtual object Key
			{
				get
				{
					return item;
				}
			}


			/// <summary>
			/// Gets whether the tree is a leaf
			/// </summary>
			public virtual bool IsLeaf
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			/// <summary>
			/// Gets whether the tree is empty
			/// </summary>
			public override bool IsEmpty
			{
				get
				{
					return false;
				}
			}

			/// <summary>
			/// Gets the degree of the tree
			/// </summary>
			public virtual int Degree
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			#endregion
		
			#region Constructor
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="partition"></param>
			/// <param name="item"></param>
			public PartitionTree(PartitionAsForest partition, int item) : base(partition.UniverseSize)
			{
				this.partition = partition;
				this.item = item;
				parent = null;
				rank = 0;
				mCount = 1;
			}

			#endregion
		
			#region Methods
			/// <summary>
			/// Emtpies the tree
			/// </summary>
			public override void Purge()
			{
				parent = null;
				rank = 0;
				mCount = 1;
			}

			/// <summary>
			/// Return whether the given partition is in the tree
			/// </summary>
			/// <param name="partition"></param>
			/// <returns></returns>
			internal virtual bool IsMemberOf(PartitionAsForest partition)
			{
				return this.partition == partition;
			}

			/// <summary>
			/// Comparison
			/// </summary>
			/// <param name="obj"></param>
			/// <returns></returns>
			public override int CompareTo(object obj)
			{
				PartitionTree partitionTree = (PartitionTree)obj;
				return item - partitionTree.item;
			}

			/// <summary>
			/// Returns the hashcode for this object
			/// </summary>
			/// <returns></returns>
			public override int GetHashCode()
			{
				return item;
			}

			/// <summary>
			/// ToString override
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				string str1 = String.Concat("PartitionTree {", item);
				if (parent != null)
				{
					str1 = String.Concat(str1, ", ", parent);
				}
				string str2 = String.Concat(str1, "}");
				return str2;
			}

			/// <summary>
			/// Inserts an element in the collection
			/// </summary>
			/// <param name="i"></param>
			public override void Insert(int i)
			{
				throw new InvalidOperationException();
			}

			/// <summary>
			/// Removes an element
			/// </summary>
			/// <param name="i"></param>
			public override void Withdraw(int i)
			{
				throw new InvalidOperationException();
			}

			/// <summary>
			/// Returns whether the given int is a member
			/// </summary>
			/// <param name="i"></param>
			/// <returns></returns>
			public override bool IsMember(int i)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Returns a subtree
			/// </summary>
			/// <param name="i"></param>
			/// <returns></returns>
			public virtual ITree GetSubtree(int i)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// DFT of the tree
			/// </summary>
			/// <param name="visitor"></param>
			public virtual void DepthFirstTraversal(IPrePostVisitor visitor)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// BFT of the tree
			/// </summary>
			/// <param name="visitor"></param>
			public virtual void BreadthFirstTraversal(IVisitor visitor)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Return the union of the tree with the given set
			/// </summary>
			/// <param name="set"></param>
			/// <returns></returns>
			public virtual ISet Union(ISet set)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Returns the intersection of the tree with the given set
			/// </summary>
			/// <param name="set"></param>
			/// <returns></returns>
			public virtual ISet Intersection(ISet set)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Returns the difference of the tree with the given set
			/// </summary>
			/// <param name="set"></param>
			/// <returns></returns>
			public virtual ISet Difference(ISet set)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Tests equality of the tree witht the given set
			/// </summary>
			/// <param name="set"></param>
			/// <returns></returns>
			public virtual bool Equals(ISet set)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Returns whether the given set is a subset of the tree
			/// </summary>
			/// <param name="set"></param>
			/// <returns></returns>
			public virtual bool IsSubset(ISet set)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Returns an enumerator for the tree
			/// </summary>
			/// <returns></returns>
			public override IEnumerator GetEnumerator()
			{
				throw new NotImplementedException();
			}
			#endregion
			
		}

		#endregion
		
		#region Fields
		/// <summary>
		/// the internal forrest
		/// </summary>
		protected PartitionTree[] forrest;
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="n">the size of the partition</param>
		public PartitionAsForest(int n) : base(n)
		{
			forrest = new PartitionTree[mUniverseSize];
			for (int j = 0; j < mUniverseSize; j++)
			{
				forrest[j] = new PartitionTree(this, j);
			}
			mCount = mUniverseSize;
		}

		#endregion

		#region Methods
		
		/// <summary>
		/// Empties the partition
		/// </summary>
		public override void Purge()
		{
			for (int i = 0; i < mUniverseSize; i++)
			{
				forrest[i].Purge();
			}
		}


		#region 'Find' overloads
		/// <summary>
		/// Returns a subset of the partition
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual ISet Find(int item)
		{
			PartitionTree partitionTree;
			
			partitionTree = forrest[item];
			
			if(partitionTree ==null) 
			{
			
				
					return null;
			}
			
		
			while ( partitionTree.parent != null)
			{
			
				partitionTree = partitionTree.parent;
			}
			
			return partitionTree;
		}

		/// <summary>
		/// 'Find' of a given ComparableObject
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override ComparableObject Find(ComparableObject obj)
		{
			return (ComparableObject)Find((int)obj);
		}

		#endregion

		/// <summary>
		/// Check for two sets if they are valid arguments for a set-join
		/// </summary>
		/// <param name="s"></param>
		/// <param name="t"></param>
		protected virtual void CheckArguments(PartitionTree s, PartitionTree t)
		{
			if (!IsMember(s) || s.parent != null || !IsMember(t) || t.parent != null || s == t)
			{
				throw new ArgumentException("incompatible sets");
			}
			else
			{
				return;
			}
		}

		
		/// <summary>
		/// Union of two sets
		/// </summary>
		/// <param name="s"></param>
		/// <param name="t"></param>
		public virtual void Join(ISet s, ISet t)
		{
			PartitionTree partitionTree1 = (PartitionTree)s;
			PartitionTree partitionTree2 = (PartitionTree)t;
			CheckArguments(partitionTree1, partitionTree2);
			partitionTree2.parent = partitionTree1;
			mCount--;
		}

		
		/// <summary>
		/// Returns whether a given object is in the partition
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool IsMember(ComparableObject obj)
		{
			return ((PartitionTree) obj).IsMemberOf(this); 
		}

		
		/// <summary>
		/// Accepts an IVisitor
		/// </summary>
		/// <param name="visitor"></param>
		public override void Accept(IVisitor visitor)
		{
			for (int i = 0; i < mUniverseSize; i++)
			{
				visitor.Visit(forrest[i]);
				if (visitor.IsDone)
				{
					break;
				}
			}
		}

		
		/// <summary>
		/// Inserts a subset
		/// </summary>
		/// <param name="i"></param>
		public override void Insert(int i)
		{
			throw new InvalidOperationException();
		}

		
		/// <summary>
		/// Removes a subset
		/// </summary>
		/// <param name="i"></param>
		public override void Withdraw(int i)
		{
			throw new InvalidOperationException();
		}

		
		/// <summary>
		/// Returns whether the given integer is a subset
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override bool IsMember(int i)
		{
			throw new InvalidOperationException();
		}

		
		/// <summary>
		/// Returns an enumerator for the partition
		/// </summary>
		/// <returns></returns>
		public override IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}

		
		/// <summary>
		/// Union of the partition with the given set
		/// </summary>
		/// <param name="set"></param>
		/// <returns></returns>
		public  virtual ISet Union(ISet set)
		{
			throw new InvalidOperationException();
		}

		
		/// <summary>
		/// Intersection of the partition with the given set
		/// </summary>
		/// <param name="set"></param>
		/// <returns></returns>
		public  virtual ISet Intersection(ISet set)
		{
			throw new InvalidOperationException();
		}

		
		/// <summary>
		/// Takes the difference of the partition with the given set
		/// </summary>
		/// <param name="set"></param>
		/// <returns></returns>
		public  virtual ISet Difference(ISet set)
		{
			throw new InvalidOperationException();
		}


		/// <summary>
		/// Tests the equality of the partition with the given set
		/// </summary>
		/// <param name="set"></param>
		/// <returns></returns>
		public  virtual bool Equals(ISet set)
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// Returns whether the given set is a subset of the partition
		/// </summary>
		/// <param name="set"></param>
		/// <returns></returns>
		public  virtual bool IsSubset(ISet set)
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// Overrides the base method. 
		/// Returns 1 if the argument is the partition, otherwise 0.
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		public override int CompareTo(object arg)
		{
			//PartitionAsForest forest = arg as PartitionAsForest;
			if((object) arg==(object) this)
				return 0;
					else
				return 1;
			//throw new NotImplementedException();
		}

	
		#endregion	
	}
}
