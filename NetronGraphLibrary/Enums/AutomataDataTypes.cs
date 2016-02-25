using System;

namespace Netron.GraphLib
{
	/// <summary>
	/// The automata data types
	/// </summary>
	[Serializable]
	public enum AutomataDataType
	{
		/// <summary>
		/// Corresponds to .Net's Int32 data type
		/// </summary>
		Integer,
		/// <summary>
		/// Corresponds to .Net's double data type
		/// </summary>
		Double,
		/// <summary>
		/// Corresponds to .Net's System.Drawing.Color data type
		/// </summary>
		Color,
		/// <summary>
		/// A 2D floating vector
		/// </summary>
		Vector,
		/// <summary>
		/// An integer between 0 and 360
		/// </summary>
		Degree,
		/// <summary>
		/// A floating point number interpreted as an angle
		/// </summary>
		Radians,
		/// <summary>
		/// Corresponds to .Net's String data type
		/// </summary>
		String,
		/// <summary>
		/// Corresponds to .Net's Boolean data type
		/// </summary>
		Bool,
		/// <summary>
		/// Corresponds to .Net's Object data type
		/// </summary>
		Object,
		/// <summary>
		/// Corresponds to .Net's datetime data type
		/// </summary>
		DateTime
	}
	/// <summary>
	/// Enumerates the different visualization types
	/// </summary>
	[Serializable]
	public enum VisualizationTypes
	{
		/// <summary>
		/// Chernoff faces; a parametrized facial expression giving for a constraint set
		/// of values a very picturial representation
		/// </summary>
		Chernoff,
		/// <summary>
		/// Using colors
		/// </summary>
		Color,
		/// <summary>
		/// The actual values
		/// </summary>
		Value,
		/// <summary>
		/// A pie-chart
		/// </summary>
		Pie,
		/// <summary>
		/// A gauge representation
		/// </summary>
		Gauge

	}
	/// <summary>
	/// Enumerates the possible initial states of the automata
	/// </summary>
	[Serializable]
	public enum AutomataInitialState
	{
		/// <summary>
		/// A single dot in the middle of the range
		/// </summary>
		SingleDot,
		/// <summary>
		/// An alternating black-and-white pattern
		/// </summary>
		Alternate,
		/// <summary>
		/// All cells black
		/// </summary>
		Black,
		/// <summary>
		/// All cells white
		/// </summary>
		White,
		/// <summary>
		/// Custom definition of a pattern
		/// </summary>
		External
	}
}
