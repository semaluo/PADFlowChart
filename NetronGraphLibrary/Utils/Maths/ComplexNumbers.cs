using System;

namespace Netron.GraphLib.Maths
{
	/// <summary>
	/// Complex number struct
	/// </summary>
	public struct Complex
	{
		#region Fields
		/// <summary>
		/// the real part of the complex number
		/// </summary>
		private double mX;
		/// <summary>
		/// the imaginary part of the complex number
		/// </summary>
		private double mY; 

		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="x">the real part of the complex number</param>
		/// <param name="y">the imaginary part of the complex number</param>
		public  Complex(double x, double y )
		{
			mX = x;
			mY = y;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the real part of the complex number
		/// </summary>
		public double  X
		{
			get{return mX;}		
			set{mX = value;}
		}
		/// <summary>
		/// Gets or sets the imaginary part of the complex number
		/// </summary>
		public double Y
		{
			get{return mY;}
			set{mY = value;}
		}
		/// <summary>
		/// Gets or sets the real part of the complex number
		/// </summary>
		public double Real
		{
			get{return mX;}
			set{mX = value;}
		}
		/// <summary>
		/// Gets or sets the imaginary part of the complex number
		/// </summary>
		public double Imaginary
		{
			get{return mY;}
			set{mY = value;}
		}

		#endregion

		#region Operator implementations
		/// <summary>
		/// == operator for Complex objects
		/// </summary>
		/// <param name="z1">a complex number</param>
		/// <param name="z2">a complex number</param>
		/// <returns></returns>
		public static bool operator == (Complex z1, Complex z2)
		{
			return (z1.X==z2.X) && (z1.Y==z2.Y);
		}
		/// <summary>
		/// != operator for Complex objects
		/// </summary>
		/// <param name="z1">a complex number</param>
		/// <param name="z2">a complex number</param>
		/// <returns></returns>
		public static bool operator != (Complex z1, Complex z2)
		{
			return (z1.X!=z2.X) || (z1.Y!=z2.Y);
		}
		/// <summary>
		/// * operator for Complex objects
		/// </summary>
		/// <param name="z1">a complex number</param>
		/// <param name="z2">a complex number</param>
		/// <returns></returns>
		public static Complex operator * (Complex z1, Complex z2)
		{
			return new Complex(z1.X*z2.X - z1.Y*z2.Y,z1.X*z2.Y + z1.Y*z2.X);
		}

		#endregion

		#region Named alternatives for non-C# coders

		/// <summary>
		/// Named alternative to the '==' operator overloading
		/// for non-C# coders
		/// </summary>
		/// <param name="z1">a complex number</param>
		/// <param name="z2">a complex number</param>
		/// <returns></returns>
		public static bool Equals(Complex z1, Complex z2)
		{
			return z1==z2;
		}

		/// <summary>
		/// Named alternative to the '!=' operator overloading
		/// for non-C# coders
		/// </summary>
		/// <param name="z1">a complex number</param>
		/// <param name="z2">a complex number</param>
		/// <returns></returns>
		public static bool NotEquals(Complex z1, Complex z2)
		{
			return z1!=z2;
		}
		/// <summary>
		/// Named alternative to the '*' operator overloading
		/// for non-C# coders
		/// </summary>
		/// <param name="z1">a complex number</param>
		/// <param name="z2">a complex number</param>
		/// <returns></returns>
		public static Complex Multiply(Complex z1, Complex z2)
		{
			return z1*z2;
		}

		#endregion

		#region Methods
		/// <summary>
		/// Overrides the base method as is requested when overriden the operators.
		/// </summary>
		/// <returns>an integer hash</returns>
		public override int GetHashCode()
		{
			//TODO: invent a better hash here
			return base.GetHashCode();
		}
		#endregion

		/// <summary>
		/// Equal override
		/// </summary>
		/// <param name="obj">an object</param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			//TODO: make this a bit smarter...
			return base.Equals (obj);
		}

	}
	/// <summary>
	/// Static utilities to manipulate complex numbers
	/// </summary>
	[Obsolete("These static utilities have become obsolete, use the properties and methods of the Complex class instead.",false)]
	public class ComplexNumbers
	{
		/// <summary>
		/// Multiplication of two complex numbers
		/// </summary>
		/// <param name="z1">a complex number</param>
		/// <param name="z2">a complex number</param>
		/// <returns></returns>		
		public static Complex Times(Complex z1 ,Complex  z2 ) 
		{
			return new Complex(z1.X * z2.X - z1.Y * z2.Y, z1.X * z2.Y + z1.Y * z2.X);
		}
		/// <summary>
		/// Sum of two complex numbers
		/// </summary>
		/// <param name="z1">a complex number</param>
		/// <param name="z2">a complex number</param>
		/// <returns></returns>
		public static Complex  Sum(Complex z1 ,Complex  z2 ) 
		{
			return new Complex(z1.X + z2.X, z1.Y + z2.Y);
		}
		/// <summary>
		/// Real part of a complex number
		/// </summary>
		/// <param name="z">a complex number</param>
		/// <returns></returns>
		public static double Real(Complex z )
		{
			return z.X;
		}
		/// <summary>
		/// Imaginary part of a complex number
		/// </summary>
		/// <param name="z">a complex number</param>
		/// <returns></returns>
		public static double Imaginary(Complex z ) 
		{
			return z.Y;
		}
		/// <summary>
		/// Norm of a complex number
		/// </summary>
		/// <param name="z">a complex number</param>
		/// <returns>the norm</returns>
		public static double Norm(Complex z ) 
		{
			return Math.Sqrt(z.X * z.X + z.Y * z.Y);
		}
		/// <summary>
		/// Sine of a complex number
		/// </summary>
		/// <param name="z">a complex number</param>
		/// <returns>the sine value</returns>
		public static Complex Sin(Complex z ) 
		{
			return new Complex(Math.Sin(z.X) * Math.Cosh(z.Y), Math.Cos(z.X) * Math.Sinh(z.Y));
		}
		/// <summary>
		/// Square of a complex number
		/// </summary>
		/// <param name="z">a complex number</param>
		/// <returns>the square of the number</returns>
		public static Complex Square(Complex z ) 
		{
			return new Complex(z.X * z.X - z.Y * z.Y, 2 * z.X * z.Y);
		}
	}
}
