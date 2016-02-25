using System;

namespace Netron.GraphLib.Interfaces
{
	/// <summary>
	/// Required signature for being part of a cellular automata network
	/// </summary>
	public interface IAutomataCell
	{
		/// <summary>
		/// Initialization or reset method
		/// </summary>
		void InitAutomata();
		/// <summary>
		/// Elementary step or update of the cell's state
		/// </summary>
		void Update();

		/// <summary>
		/// Transmits data between connections
		/// </summary>
		void Transmit();
		/// <summary>
		/// Actions before the actual update is performed
		/// </summary>
		void BeforeUpdate();
		/// <summary>
		/// Actions after the update is performed
		/// </summary>
		void AfterUpdate();
	}

	/// <summary>
	/// Used by the scripter-shape in the Automatron application
	/// but more in general; this is part of scripting at runtime.
	/// </summary>
	public interface IScript:IDisposable
	{
		/// <summary>
		/// Initializes with the host
		/// </summary>
		/// <param name="Host"></param>
		void Initialize(IHost Host);
		/// <summary>
		/// Generic method
		/// </summary>
		void Method1();
		/// <summary>
		/// Generic method
		/// </summary>
		void Method2();
		/// <summary>
		/// Generic method
		/// </summary>
		void Method3();
		/// <summary>
		/// Computes something
		/// </summary>
		void Compute();
		
	}
	/// <summary>
	/// Used by the scripter-shape in the Automatron application
	/// but more in general; this is part of scripting at runtime.
	/// </summary>
	public interface IHost
	{
		/// <summary>
		/// Sends a message to the GUI
		/// </summary>
		/// <param name="Message"></param>
		void ShowMessage(string Message);
		/// <summary>
		/// Gets the Out-connector of the shape
		/// </summary>
		Connector Out {get;}
		/// <summary>
		/// Gets the x-value of the connector
		/// </summary>
		Connector XIn {get;}
		/// <summary>
		/// Gets the y-value of the connector
		/// </summary>
		Connector YIn {get;}
	}
}
