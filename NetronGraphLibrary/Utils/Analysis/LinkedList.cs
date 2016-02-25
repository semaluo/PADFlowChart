using System;
using System.Text;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// A list implemented by each item having a link to the next item. 
	/// </summary>
	public class LinkedList
	{
		#region Classes
		/// <summary>
		/// Enbodies an element of a linked list
		/// </summary>
		public sealed class Element
		{
			#region Fields
			/// <summary>
			/// the list to which this element belongs
			/// </summary>
			internal LinkedList list;

			/// <summary>
			/// the data bucket of this element
			/// </summary>
			internal object datum;

			/// <summary>
			/// the element coming after this one
			/// </summary>
			internal Element next;

			#endregion

			#region Properties
			/// <summary>
			/// Gets the data attached to this element
			/// </summary>
			public object Datum
			{
				get
				{
					return datum;
				}
			}

			/// <summary>
			/// Gets the next element from the list
			/// </summary>
			public Element Next
			{
				get
				{
					return next;
				}
			}

			#endregion

			#region Methods
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="list"></param>
			/// <param name="datum"></param>
			/// <param name="next"></param>
			internal Element(LinkedList list, object datum, Element next)
			{
				this.list = list;
				this.datum = datum;
				this.next = next;
			}

			/// <summary>
			/// Inserts the element after the given item
			/// </summary>
			/// <param name="item"></param>
			public void InsertAfter(object item)
			{
				next = new Element(list, item, next);
				if (list.mTail == this)
				{
					list.mTail = next;
				}
			}

			/// <summary>
			/// Inserts the element before the given element
			/// </summary>
			/// <param name="item"></param>
			public void InsertBefore(object item)
			{
				Element element1 = new Element(list, item, this);
				if (this == list.mHead)
				{
					list.mHead = element1;
				}
				else
				{
					Element element2;

					for (element2 = list.mHead; element2 != null && element2.next != this; element2 = element2.next)
					{
					}
					element2.next = element1;
				}
			}

			/// <summary>
			/// Extract the element from the list
			/// </summary>
			public void Extract()
			{
				Element element = null;
				if (list.mHead == this)
				{
					list.mHead = next;
				}
				else
				{
					for (element = list.mHead; element != null && element.next != this; element = element.next)
					{
					}
					if (element == null)
					{
						throw new InvalidOperationException();
					}
					element.next = next;
				}
				if (list.mTail == this)
				{
					list.mTail = element;
				}
			}

			#endregion
		}

		#endregion

		#region Fields
		/// <summary>
		/// the mHead of the list
		/// </summary>
		protected Element mHead;

		/// <summary>
		/// the mTail or last element of the list
		/// </summary>
		protected Element mTail;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the mHead or first element of the list
		/// </summary>
		public Element Head
		{
			get
			{
				return mHead;
			}
		}

		/// <summary>
		/// Gets the mTail or last element of the list
		/// </summary>
		public Element Tail
		{
			get
			{
				return mTail;
			}
		}

		/// <summary>
		/// Gets whether the list is empty
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return mHead == null;
			}
		}

		/// <summary>
		/// Gets the first element of the list
		/// </summary>
		public object First
		{
			get
			{
				if (mHead == null)
				{
					throw new ContainerEmptyException();
				}
				else
				{
					return mHead.datum;
				}
			}
		}

		/// <summary>
		/// Gets the last element of the list
		/// </summary>
		public object Last
		{
			get
			{
				if (mTail == null)
				{
					throw new ContainerEmptyException();
				}
				else
				{
					return mTail.datum;
				}
			}
		}


		#endregion

		#region Methods
		/// <summary>
		/// Purges or empties the list
		/// </summary>
		public void Purge()
		{
			mHead = null;
			mTail = null;
		}

		/// <summary>
		/// Adds an item before the mHead of the list
		/// </summary>
		/// <param name="item"></param>
		public void Prepend(object item)
		{
			Element element = new Element(this, item, mHead);
			if (mHead == null)
			{
				mTail = element;
			}
			mHead = element;
		}


		/// <summary>
		/// Appends an item to the end of the list
		/// </summary>
		/// <param name="item"></param>
		public void Append(object item)
		{
			Element element = new Element(this, item, null);
			if (mHead == null)
			{
				mHead = element;
			}
			else
			{
				mTail.next = element;
			}
			mTail = element;
		}

		/// <summary>
		/// Copies the elements of the given list to this list
		/// </summary>
		/// <param name="list"></param>
		public void Copy(LinkedList list)
		{
			if (list != this)
			{
				Purge();
				for (Element element = list.mHead; element != null; element = element.next)
				{
					Append(element.datum);
				}
			}
		}

		/// <summary>
		/// Removes the given item from the list
		/// </summary>
		/// <param name="item"></param>
		public void Extract(object item)
		{
			Element element1 = mHead;
			Element element2 = null;
			for (; element1 != null && element1.datum != item; element1 = element1.next)
			{
				element2 = element1;
			}
			if (element1 == null)
			{
				throw new ArgumentException("item not found");
			}
			if (element1 == mHead)
			{
				mHead = element1.next;
			}
			else
			{
				element2.next = element1.next;
			}
			if (element1 == mTail)
			{
				mTail = element2;
			}
		}

		/// <summary>
		/// Returns a string representation of the list
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("LinkedList {");
			for (Element element = mHead; element != null; element = element.next)
			{
				stringBuilder.Append(element.datum);
				if (element.next != null)
				{
					stringBuilder.Append(",");
				}
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		#endregion

	}
}
