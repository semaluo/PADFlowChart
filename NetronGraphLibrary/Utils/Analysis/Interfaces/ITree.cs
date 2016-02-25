using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Tree interface
	/// </summary>
	public interface ITree: IContainer, IComparable, IEnumerable
	{
		/// <summary>
		/// Gets the key of the (sub)tree
		/// </summary>
		object Key
		{
			get;
		}
		/// <summary>
		/// Gets whether the tree is a leaf
		/// </summary>
		bool IsLeaf
		{
			get;
		}
		/// <summary>
		/// Gets the degree of the tree
		/// </summary>
		int Degree
		{
			get;
		}
		/// <summary>
		/// Gets the height of the tree
		/// </summary>
		int Height
		{
			get;
		}
		/// <summary>
		/// Gets the subtree of the i-th node
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		ITree GetSubtree(int i);
		/// <summary>
		/// DFT of the tree
		/// </summary>
		/// <param name="visitor"></param>
		void DepthFirstTraversal(IPrePostVisitor visitor);
		/// <summary>
		/// BFT of the tree
		/// </summary>
		/// <param name="visitor"></param>
		void BreadthFirstTraversal(IVisitor visitor);
	}
}
