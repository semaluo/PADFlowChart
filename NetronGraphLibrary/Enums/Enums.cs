using System;

namespace Netron.GraphLib
{
	/// <summary>
	/// The types of backgrounds the control can have
	/// </summary>
	public enum CanvasBackgroundType
	{
			
		/// <summary>
		/// Uniform flat colored
		/// </summary>
		FlatColor,
		/// <summary>
		/// Two-color gradient
		/// </summary>
		Gradient,
		/// <summary>
		/// A user defined image
		/// </summary>
		Image

	}
	/// <summary>
	/// The types of graph layouts
	/// </summary>
	public enum GraphLayoutAlgorithms
	{
		/// <summary>
		/// The spring embedder algorithm
		/// </summary>
		SpringEmbedder,
		/// <summary>
		/// The tree layout algorithm
		/// </summary>
		Tree,
		/// <summary>
		/// Randomizes the nodes on the canvas
		/// </summary>
		Randomizer
	}

	/// <summary>
	/// The basic types of shapes
	/// </summary>
	public enum BasicShapeType
	{
		/// <summary>
		/// A resizable node with four connectors
		/// </summary>
		BasicNode,
		/// <summary>
		/// A non-resiable node with one connector
		/// </summary>
		SimpleNode,
		/// <summary>
		/// A resizable text label node with no connectors
		/// </summary>
		TextLabel

	}
	/// <summary>
	/// The possible weights of connections
	/// </summary>
	public enum ConnectionWeight
	{
		/// <summary>
		/// Thin weight
		/// </summary>
		Thin,
		/// <summary>
		/// Medium weight
		/// </summary>
		Medium,
		/// <summary>
		/// Fat weight
		/// </summary>
		Fat
	}

	/// <summary>
	/// The types of connection ends
	/// </summary>
	public enum ConnectionEnd
	{
		/// <summary>
		/// Filled arrow at the start
		/// </summary>
		LeftFilledArrow,
		/// <summary>
		/// Filled arrow at the end
		/// </summary>
		RightFilledArrow,
		/// <summary>
		/// Filled arrow at both sides
		/// </summary>
		BothFilledArrow,
		/// <summary>
		/// Open arrow at the start
		/// </summary>
		LeftOpenArrow,
		/// <summary>
		/// Open arrow at the end
		/// </summary>
		RightOpenArrow,
		/// <summary>
		/// Open arrows at both ends
		/// </summary>
		BothOpenArrow,
		/// <summary>
		/// No arrows at all
		/// </summary>
		NoEnds
		
	}
	/// <summary>
	/// Connector locations
	/// </summary>
	public enum ConnectorLocation
	{
		/// <summary>
		/// The connector's offset will point north of the connector
		/// </summary>
		North,
		/// <summary>
		/// The connector's offset will point east of the connector
		/// </summary>
		East,
		/// <summary>
		/// The connector's offset will point south of the connector
		/// </summary>
		South,
		/// <summary>
		/// The connector's offset will point west of the connector
		/// </summary>
		West,
		/// <summary>
		/// No connector's offset
		/// </summary>
		Omni,
		/// <summary>
		/// Unknown or not set
		/// </summary>
		Unknown
	}

	

	/// <summary>
	/// The various ways you can display the shapes in the viewer
	/// </summary>
	public enum ShapesView
	{
		/// <summary>
		/// Display as a tree
		/// </summary>
		Tree,
		/// <summary>
		/// Display as large icons
		/// </summary>
		Icons
	}
	/// <summary>
	/// The types of handles of a Bezier curve
	/// </summary>
	public enum HandleTypes
	{
		/// <summary>
		/// Only one tangent
		/// </summary>
		Single,
		/// <summary>
		/// Two independent tangents
		/// </summary>
		Free,
		/// <summary>
		/// Two tangent symmetric on both sides of the handle
		/// </summary>
		Symmetric
	}

	/// <summary>
	/// Enumerates the basic math functions available in automata nodes
	/// </summary>
	[Serializable]
	public enum BasicMathFunction
	{
		/// <summary>
		/// Cosine
		/// </summary>
		Cos,
		/// <summary>
		/// Sine
		/// </summary>
		Sin,
		/// <summary>
		/// Tangens
		/// </summary>
		Tan,
		/// <summary>
		/// Inverse cosine
		/// </summary>
		ACos,
		/// <summary>
		/// Inverse sine
		/// </summary>
		ASin,
		/// <summary>
		/// Inverse tangens
		/// </summary>
		ATan,
		/// <summary>
		/// Hyperbolic cosine
		/// </summary>
		Cosh,
		/// <summary>
		/// Exponential
		/// </summary>
		Exp,
		/// <summary>
		/// Logarithm
		/// </summary>
		Log,
		/// <summary>
		/// Hyperbolic sine
		/// </summary>
		Sinh,
		/// <summary>
		/// Hyperbolic tangens
		/// </summary>
		Tanh,
		/// <summary>
		/// Absolute value
		/// </summary>
		Abs
	}
	/// <summary>
	/// Basic math operations available to automata nodes
	/// </summary>
	[Serializable]
	public enum BasicMathOperator
	{
		/// <summary>
		/// Multiplication
		/// </summary>
		Times,
		/// <summary>
		/// Division
		/// </summary>
		Divide,
		/// <summary>
		/// Modulo function
		/// </summary>
		Mod
	}

	/// <summary>
	/// Enumerates the type of sortings
	/// </summary>
	public enum SortByType
	{
		/// <summary>
		/// By method
		/// </summary>
		Method = 0,
		/// <summary>
		/// By property
		/// </summary>
		Property = 1
	}

	/// <summary>
	/// Enumerates the sorting direction
	/// </summary>
	public enum SortDirection
	{
		/// <summary>
		/// Ascending
		/// </summary>
		Ascending = 0,
		/// <summary>
		/// Descending
		/// </summary>
		Descending = 1
	}

	
	/// <summary>
	/// The different levels of information send out to the outside world 
	/// by the graph control.
	/// </summary>
	public enum OutputInfoLevels
	{
		/// <summary>
		/// Thrown exception message
		/// </summary>
		Exception,
		/// <summary>
		/// Informative message.
		/// </summary>
		Info,
		/// <summary>
		/// Unspecified message
		/// </summary>
		None

	}
}
