using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Implementation of a chained hastable data structure
	/// </summary>
	public class ChainedHashTable : AbstractHashTable
	{
		#region Enumerator implementation
		private class Enumerator: IEnumerator
		{

			#region Fields
			/// <summary>
			/// internal hashtable
			/// </summary>
			private ChainedHashTable hashtable;
			/// <summary>
			/// an element in the table
			/// </summary>
			private LinkedList.Element element;
			/// <summary>
			/// the current position
			/// </summary>
			private int position;
			#endregion
			
			#region Properties
			/// <summary>
			/// Gets the current element in the enumeration
			/// </summary>
			public   virtual object Current
			{
				get
				{
					if (element == null)
					{
						throw new InvalidOperationException();
					}
					else
					{
						return element.Datum;
					}
				}
			}

			#endregion
			
			#region Constructor
			/// <summary>
			/// Internal constructor
			/// </summary>
			/// <param name="hashtable"></param>
			internal Enumerator(ChainedHashTable hashtable)
			{
				element = null;
				position = -1;

				this.hashtable = hashtable;
			}

			#endregion
			
			#region Methods
			/// <summary>
			/// Moves the pointer to the next element in the enumeration
			/// </summary>
			/// <returns></returns>
			public  virtual bool MoveNext()
			{
				if (element != null)
				{
					element = element.Next;
				}
				if (element == null)
				{
					for (position++; position < hashtable.Length; position++)
					{
						element = hashtable.array[position].Head;
						if (element != null)
						{
							break;
						}
					}
					if (position == hashtable.Length)
					{
						position = -1;
					}
				}
				bool flag = element == null == false;
				return flag;
			}

			/// <summary>
			/// Resets the enumeration pointer
			/// </summary>
			public  virtual void Reset()
			{
				element = null;
				position = -1;
			}
			#endregion
			
		}
		#endregion

		#region Fields
		/// <summary>
		/// the inner array
		/// </summary>
		protected LinkedList[] array;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the length of the chained hashtable
		/// </summary>
		public override int Length
		{
			get
			{
				return (int)array.Length;
			}
		}

		#endregion
		
		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="length"></param>
		public ChainedHashTable(int length)
		{
			array = new LinkedList[(uint)length];
			for (int i = 0; i < length; i++)
			{
				array[i] = new LinkedList();
			}
		}

		#endregion

		#region Methods
		/// <summary>
		/// Empties the container or hashtable
		/// </summary>
		public override void Purge()
		{
			for (int i = 0; i < (int)array.Length; i++)
			{
				array[i].Purge();
			}
			mCount = 0;
		}

		/// <summary>
		/// Inserts an element in the hashtable
		/// </summary>
		/// <param name="obj"></param>
		public override void Insert(ComparableObject obj)
		{
			array[base.H(obj)].Append(obj);
			mCount++;
		}

		/// <summary>
		/// Removes an elements from the hashtable
		/// </summary>
		/// <param name="obj"></param>
		public override void Withdraw(ComparableObject obj)
		{
			array[H(obj)].Extract(obj);
			mCount--;
		}

		/// <summary>
		/// Returns whether the given object is contained in the hashtable
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool IsMember(ComparableObject obj)
		{
			bool flag;

			for (LinkedList.Element element = array[base.H(obj)].Head; element != null; element = element.Next)
			{
				ComparableObject comparableObject = (ComparableObject)element.Datum;
				if (obj != comparableObject)
				{
					continue;
				}
				flag = true;
				return flag;
			}
			flag = false;
			return flag;
		}

		/// <summary>
		/// Searches the given object in the hashtable
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override ComparableObject Find(ComparableObject obj)
		{
			ComparableObject comparableObject2;

			for (LinkedList.Element element = array[base.H(obj)].Head; element != null; element = element.Next)
			{
				ComparableObject comparableObject1 = (ComparableObject)element.Datum;
				if (obj != comparableObject1)
				{
					continue;
				}
				comparableObject2 = comparableObject1;
				return comparableObject1;
			}
			comparableObject2 = null;
			return comparableObject2;
		}

		/// <summary>
		/// Accepts an IVisitor
		/// </summary>
		/// <param name="visitor"></param>
		public override void Accept(IVisitor visitor)
		{
			
			for (int i = 0; i < this.Length; i++)
			{
				for (LinkedList.Element element = array[i].Head; element != null; element = element.Next)
				{
					visitor.Visit(element.Datum);
					if (visitor.IsDone)
					{
						return;
					}
				}
			}
		}

		/// <summary>
		/// Returns an enumerator for the hashtable
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
