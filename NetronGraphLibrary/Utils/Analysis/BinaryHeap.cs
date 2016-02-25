using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Throwable exception when the container is empty
	/// </summary>
	public class ContainerEmptyException : InvalidOperationException
	{
	}
	/// <summary>
	/// Implements a binary heap
	/// </summary>
	public class BinaryHeap : AbstractContainer, IPriorityQueue, IContainer, IComparable, IEnumerable
	{
		#region Enumerator class
		private class Enumerator: IEnumerator
		{
			private BinaryHeap heap;

			private int position;


			public  virtual object Current
			{
				get
				{
					if (position == 0)
					{
						throw new InvalidOperationException();
					}
					else
					{
						return heap.array[position];
					}
				}
			}

			internal Enumerator(BinaryHeap heap)
			{
				position = 0;
			
				this.heap = heap;
			}

			public  virtual bool MoveNext()
			{
				position++;
				if (position > heap.mCount)
				{
					position = 0;
				}
				bool flag = position > 0;
				return flag;
			}

			public  virtual void Reset()
			{
				position = 0;
			}
		}
		#endregion

		#region Fields
		/// <summary>
		/// internal array of the heap
		/// </summary>
		protected ComparableObject[] array;
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public virtual ComparableObject Min
		{
			get
			{
				if (mCount == 0)
				{
					throw new ContainerEmptyException();
				}
				else
				{
					return array[1];
				}
			}
		}
		/// <summary>
		/// Gets whether the container is full
		/// </summary>
		public override bool IsFull
		{
			get
			{
				return mCount == (int)array.Length - 1;
			}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="length"></param>
		public BinaryHeap(int length)
		{
			array = new ComparableObject[(uint)(length + 1)];
		}
		#endregion

		#region Methods
		/// <summary>
		/// Empties the container/heap
		/// </summary>
		public override void Purge()
		{
			while (mCount > 0)
			{
				array[mCount--] = null;
			}
		}
		/// <summary>
		/// Enqueues a new objects on the heap
		/// </summary>
		/// <param name="obj"></param>
		public virtual void Enqueue(ComparableObject obj)
		{
			int i;

			if (mCount == (int)array.Length - 1)
			{
				throw new Netron.GraphLib.Analysis.ContainerEmptyException();
			}
			mCount++;

			for (i = mCount; i > 1 && array[i / 2] > obj; i /= 2)
			{
				array[i] = array[i / 2];
				Console.WriteLine(i/2 + "->" + i);
			}
			array[i] = obj;
			//Console.WriteLine(mCount + " enqueued");
		}


		/// <summary>
		/// Dequeue an object
		/// </summary>
		/// <returns></returns>
		public virtual ComparableObject DequeueMin()
		{
			int i;

			if (mCount == 0)
			{
				throw new ContainerEmptyException();
			}
			ComparableObject comparableObject1 = array[1];
			ComparableObject comparableObject2 = array[mCount];
			mCount--;
			int j;

			for (i = 1; 2 * i < mCount + 1; i = j)
			{
				j = 2 * i;
				if (j + 1 < mCount + 1 && array[j + 1] < array[j])
				{
					j++;
				}
				if (comparableObject2 <= array[j])
				{
					break;
				}
				array[i] = array[j];
			}
			array[i] = comparableObject2;
			return comparableObject1;
		}

		/// <summary>
		/// Accpets an IVisitor
		/// </summary>
		/// <param name="visitor"></param>
		public override void Accept(IVisitor visitor)
		{
			for (int i = 1; i < mCount + 1; i++)
			{
				visitor.Visit(array[i]);
				if (visitor.IsDone)
				{
					break;
				}
			}
		}

		/// <summary>
		/// Returns an enumerator for the heap
		/// </summary>
		/// <returns></returns>
		public override IEnumerator GetEnumerator()
		{
			return new Enumerator(this);
		}


		/// <summary>
		/// Not implemented yet
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
