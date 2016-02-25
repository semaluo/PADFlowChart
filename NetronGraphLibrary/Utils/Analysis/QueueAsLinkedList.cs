using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Queue implemented on the basis of a LinkedList <see cref="LinkedList"/>
	/// </summary>
	/// 
	public class QueueAsLinkedList : AbstractContainer, IQueue, IContainer, IComparable, IEnumerable
	{
		#region Classes
		/// <summary>
		/// The enumerator returned for this queue object
		/// </summary>
		private class Enumerator: IEnumerator
		{
			private QueueAsLinkedList queue;

			private LinkedList.Element position;


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

			internal Enumerator(QueueAsLinkedList queue)
			{
				this.queue = queue;
			}

			public  virtual bool MoveNext()
			{
				if (position == null)
				{
					position = queue.list.Head;
				}
				else
				{
					position = position.Next;
				}
				bool flag = position == null == false;
				return flag;
			}

			public  virtual void Reset()
			{
				position = null;
			}
		}


		#endregion

		#region Fields
		/// <summary>
		/// the internal linked list on which the queue is built
		/// </summary>
		protected LinkedList list;

		#endregion

		#region Properties
		
		/// <summary>
		/// Gets the head of the list
		/// </summary>
		public  virtual object Head
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
		/// Constructor
		/// </summary>
		public QueueAsLinkedList()
		{
			list = new LinkedList();
		}
		#endregion

		#region Methods



		/// <summary>
		/// Empties the queue
		/// </summary>
		public override void Purge()
		{
			list.Purge();
			mCount = 0;
		}

		/// <summary>
		/// Adds an item to the queue
		/// </summary>
		/// <param name="obj"></param>
		public  virtual void Enqueue(object obj)
		{
			list.Append(obj);
			mCount++;
		}

		/// <summary>
		/// Returns the next element from the queue
		/// </summary>
		/// <returns></returns>
		public  virtual object Dequeue()
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
		/// Accepts an IVisitor according to the Visitor pattern
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
		/// Returns the enumerator for this queue
		/// </summary>
		/// <returns></returns>
		public override IEnumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		/// <summary>
		/// Implements the ICompare interface
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		public override int CompareTo(object arg)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
