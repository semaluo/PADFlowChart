using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Implementation of a stack as a linked list;
	/// a list in which the next item to be removed is the item most recently stored (LIFO)
	/// </summary>
	public class StackAsLinkedList : AbstractContainer, IStack, IContainer, IComparable, IEnumerable
	{
		#region Fields
		/// <summary>
		/// the internal linked list on which the stack is based
		/// </summary>
		protected LinkedList list;
		#endregion

		#region Enumerator class
		/// <summary>
		/// The internal class enumerating the elements
		/// </summary>
		private class Enumerator: IEnumerator
		{
			#region Fields
			/// <summary>
			/// a pointer to the stack/list of this enumarator
			/// </summary>
			private StackAsLinkedList stack;
			/// <summary>
			/// the position or current element
			/// </summary>
			protected LinkedList.Element position;

			#endregion

			#region Properties
			/// <summary>
			/// Gets the current object
			/// </summary>
			public  virtual object Current
			{
				get
				{
					if (position == null)
					{
						throw new InvalidOperationException();
					}
					else
					{
						return position.Datum;
					}
				}
			}
			#endregion

			#region Constructor
			/// <summary>
			/// Constructor, internal
			/// </summary>
			/// <param name="stack"></param>
			internal Enumerator(StackAsLinkedList stack)
			{
				position = null;			
				this.stack = stack;
			}
			#endregion

			#region Methods
			/// <summary>
			/// Moves the pointer to the next element, if any
			/// </summary>
			/// <returns>true if there is a next element</returns>
			public  virtual bool MoveNext()
			{
				if (position == null)
				{
					position = stack.list.Head;
				}
				else
				{
					position = position.Next;
				}
				bool flag = position == null == false;
				return flag;
			}

			/// <summary>
			/// Resets the pointer to the beginning of the list
			/// </summary>
			public  virtual void Reset()
			{
				position = null;
			}
			#endregion
		}
		#endregion

		#region Properties

		/// <summary>
		/// Gets the first element of the stack
		/// </summary>
		public  virtual object Top
		{
			get
			{
				if (mCount == 0)
				{
					throw new ContainerEmptyException();
				}
				else
				{
					return list.First;
				}
			}
		}

		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public StackAsLinkedList()
		{
			list = new LinkedList();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Empties the container
		/// </summary>
		public override void Purge()
		{
			list.Purge();
			mCount = 0;
		}

		/// <summary>
		/// Pushes a new element on the stack
		/// </summary>
		/// <param name="obj"></param>
		public  virtual void Push(object obj)
		{
			list.Prepend(obj);
			mCount++;
		}

		/// <summary>
		/// Picks up the element on the top of the stack
		/// </summary>
		/// <returns></returns>
		public  virtual object Pop()
		{
			if (mCount == 0)
			{
				throw new ContainerEmptyException();
			}
			object local1 = list.First;
			list.Extract(local1);
			mCount--;
			return local1;
		}

		/// <summary>
		/// Accepts a visitor
		/// </summary>
		/// <param name="visitor"></param>
		public override void Accept(IVisitor visitor)
		{
			for (LinkedList.Element element = list.Head; element != null; element = element.Next)
			{
				visitor.Visit(element.Datum);
				if (visitor.IsDone)
				{
					break;
				}
			}
		}

		/// <summary>
		/// Returns an enumerator for the stack
		/// </summary>
		/// <returns></returns>
		public override IEnumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		/// <summary>
		/// Compares this stack to another object
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		/// <remarks>Not implemented yet</remarks>
		public override int CompareTo(object obj)
		{
			//TODO: still have to implement this
			throw new NotImplementedException();
		}

		#endregion

		


		


		
		

	
	}
}
