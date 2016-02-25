using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;


namespace Netron.GraphLib.Maths
{
	/// <summary>
	/// Implementation of a 3D vector 
	/// </summary>
	public class NetronVector
	{
		#region Fields
		/// <summary>
		/// the x-coordinate
		/// </summary>
		private double mX;
		/// <summary>
		/// the y-coordinate
		/// </summary>
		private double mY;
		/// <summary>
		/// the z-coordinate
		/// </summary>
		private double mZ;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the X-coordinate of the vector
		/// </summary>
		public double X
		{
			get{return mX;}
			set{mX = value;}
		}
		/// <summary>
		/// Gets or sets the Y-coordinate of the vector
		/// </summary>
		public double Y
		{
			get{return mY;}
			set{mY = value;}
		}
		/// <summary>
		/// Gets or sets the Z-coordinate of the vector
		/// </summary>
		public double Z
		{
			get{return mZ;}
			set{mZ = value;}
		}
		#endregion

		#region Constructors		
		/// <summary>
		/// Constructor; the zero vector
		/// </summary>
		public NetronVector(){}
		/// <summary>
		/// Constructor; based on another NetronVector
		/// </summary>
		/// <param name="v">a vector</param>
		public NetronVector(NetronVector v) 
		{
			mX=v.mX; mY=v.mY; mZ=v.mZ;
		} 
		/// <summary>
		/// Common constructor; based on the coordinates
		/// </summary>
		/// <param name="ax">the x-coordinate of the vector</param>
		/// <param name="ay">the y-coordinate of the vector</param>
		/// <param name="az">the z-coordinate of the vactor</param>
		public NetronVector(double ax, double ay, double az) 
		{
			mX=ax; mY=ay; mZ=az;
		} 
		#endregion

		#region Methods

		#region Operator overloading

		#endregion

		#region Basic operations as methods

		/// <summary>
		/// Returns the addition of the vector with the given one
		/// </summary>
		/// <param name="v">a vector</param>
		/// <returns></returns>
		public NetronVector Add(NetronVector v) 
		{     
			return new NetronVector(mX+v.mX,mY+v.mY,mZ+v.mZ);
		}
		/// <summary>
		/// Adds a vector to the current one
		/// </summary>
		/// <param name="v">a vector</param>
		public void AddEquals(NetronVector v) 
		{
			mX += v.mX; mY += v.mY; mZ += v.mZ;
		}
		/// <summary>
		/// Substract the vector with the given one
		/// </summary>
		/// <param name="v">a vector</param>
		/// <returns>the result of the vector operation</returns>
		public NetronVector Substract(NetronVector v) 
		{
			return new NetronVector(mX-v.mX,mY-v.mY,mZ-v.mZ);
		}
		/// <summary>
		/// Substract the vector with the given one
		/// </summary>
		/// <param name="v">a vector</param>
		public void SubstractEquals(NetronVector v) 
		{
			mX -= v.mX; mY -= v.mY; mZ -= v.mZ;
		}
		/// <summary>
		/// Returns the multiplied vector with the given quantity
		/// </summary>
		/// <param name="scalingFactor"></param>
		/// <returns>the resulting vector</returns>
		public NetronVector Multiply(double scalingFactor) 
		{
			return new NetronVector(mX*scalingFactor,mY*scalingFactor,mZ*scalingFactor);
		}
		/// <summary>
		/// Multiplies the vector with the given quantity
		/// </summary>
		/// <param name="scalingFactor"></param>
		public void MultiplyEquals(double scalingFactor) 
		{
			mX = mX*scalingFactor; mY = mY*scalingFactor; mZ = mZ*scalingFactor;
		}
		/// <summary>
		/// Divides the vector with the given value, if not zero.
		/// If zero, returns the zero vector.
		/// </summary>
		/// <param name="s">the value</param>
		/// <returns>the resulting vector</returns>
		public NetronVector Div(double s) 
		{
			if (s!=0) 
			{
				return new NetronVector(mX/s,mY/s,mZ/s);
			} 
			else 
			{
				return new NetronVector();
			}
		}
		/// <summary>
		/// Divides the vector by the given quantity, if not zero.
		/// If zero, does nothing
		/// </summary>
		/// <param name="s">the value to divide with</param>
		public void DivEquals(double s) 
		{
			if (s!=0) 
			{
				mX = mX/s; mY = mY/s; mZ = mZ/s;
			}
		}
		/// <summary>
		/// Returns the inverted vector
		/// </summary>
		/// <returns>the resulting vector</returns>
		public NetronVector Negate() 
		{
			return new NetronVector(-mX,-mY,-mZ);
		}
   
		/// <summary>
		/// Inverts the vector
		/// </summary>
		public void NegateEquals() 
		{
			mX = -mX; mY = -mY; mZ = -mZ;
		}
		/// <summary>
		/// Sets the coordinates of this vector to those of the given one
		/// </summary>
		/// <param name="v">a vector</param>
		public void SetTo(NetronVector v) 
		{
			mX = v.mX; mY = v.mY; mZ = v.mZ;
		}
		#endregion

		#region More advanced operations based on methods
		
		/// <summary>
		/// Scalar product of this vector with another
		/// </summary>
		/// <param name="v">the second vector to make the scalar product with</param>
		/// <returns>the result of the scalar product</returns>
		public double DotProduct(NetronVector v) 
		{
			return mX*v.mX+mY*v.mY+mZ*v.mZ;
		}
		/// <summary>
		/// Returns the projection of this vector on the given one
		/// </summary>
		/// <param name="v">the vector onto which this vector is projected</param>
		/// <returns>the resulting vector</returns>
		public NetronVector Projection(NetronVector v) 
		{
			return v.Multiply((this.DotProduct(v))/(v.DotProduct(v)));
		}
		/// <summary>
		/// Sets this vector to the project of the original onto the given vector
		/// </summary>
		/// <param name="v">the vector onto which this vector is projected</param>
		public void ProjectionEquals(NetronVector v) 
		{
			this.SetTo(v.Multiply((this.DotProduct(v))/(v.DotProduct(v))));
		}

		/// <summary>
		/// The vector product with another vector
		/// </summary>
		/// <param name="v">the second vector of the product</param>
		/// <returns>the resulting vector</returns>
		public NetronVector CrossProduct(NetronVector v) 
		{
			return new NetronVector(this.mY*v.mZ-v.mY*this.mZ, this.mZ*v.mX-v.mZ*this.mX, this.mX*v.mY-v.mX*this.mY);
		}
		/// <summary>
		/// The vector product and sets the result equal to this vector
		/// </summary>
		/// <param name="v">a vector</param>
		public void CrossProductEquals(NetronVector v) 
		{
			this.SetTo(new NetronVector(this.mY*v.mZ-v.mY*this.mZ, this.mZ*v.mX-v.mZ*this.mX, this.mX*v.mY-v.mX*this.mY));
		}

		
		/// <summary>
		/// Return whether another vector is equal to this one
		/// </summary>
		/// <param name="v">a NetronVector vector</param>
		/// <returns>true if the two are component-wise equal, otherwise false</returns>
		public bool IsEqual(NetronVector v) 
		{
			if(this.mX!=v.X) { return false; }
			if(this.mY!=v.Y) { return false; }
			if(this.mZ!=v.Z) { return false; }
			return true;
		}
		/// <summary>
		/// Return the length of this vector
		/// </summary>
		/// <returns></returns>
		public double Length() 
		{
			return Math.Sqrt(mX*mX+mY*mY+mZ*mZ);
		}
		/// <summary>
		/// Returns the square length of the vector,
		/// good for comparing disances. faster than length.
		/// </summary>
		/// <returns>the square</returns>
		public double LengthSquared() 
		{                         
			return mX*mX+mY*mY+mZ*mZ;
		}
		/// <summary>
		/// Returns a unit vector in the direction of this vector
		/// </summary>
		/// <returns>the resulting unit vector</returns>
		public NetronVector Unit() 
		{
			return this.Div(this.Length());
		}
		/// <summary>
		/// Resizes the vector to a unit-vector
		/// </summary>
		public void SetUnit() 
		{
			this.DivEquals(this.Length());
		}
		/// <summary>
		/// Sets the length of the vector to the given value
		/// </summary>
		/// <param name="scalingFactor">the scaling factor</param>
		public void SetLength(double scalingFactor) 
		{
			this.Unit().Multiply(scalingFactor);
		}
		/// <summary>
		/// Overrides the default behavior and returns the coordinates of
		/// the vector
		/// </summary>
		/// <returns>the resulting string</returns>
		public override string ToString()
		{
			return "(" + mX + "," + mY + "," + mZ + ")";
		}
	
		/// <summary>
		/// Returns the distance to the given vector
		/// </summary>
		/// <param name="v">a vector</param>
		/// <returns>the distance to the given vector</returns>
		public double Distance(NetronVector v)
		{
			return Math.Sqrt((mX-v.X)*(mX-v.X) + (mY-v.Y)*(mY-v.Y)+(mZ-v.Z)*(mZ-v.Z));
		}
		/// <summary>
		/// Returns the angle between two NetronVectors
		/// </summary>
		/// <param name="v1">a vector</param>
		/// <param name="v2">a vector</param>
		/// <returns></returns>
		public static double Angle(NetronVector v1,NetronVector v2)
		{
			if((v1.Length() >0) && (v2.Length()>0))
				return Math.Acos(v1.DotProduct(v2)/(v1.Length()*v2.Length()));
			else
				return 0;
		}		 
		/// <summary>
		/// Rotates a 3D point along the origin of its coordinate system
		/// </summary>
		/// <param name="phi">the angle around the x-axis</param>
		/// <param name="theta">the angle around the y-axis</param>
		/// <param name="psi">the angle around the z-axis</param>
		/// <returns></returns>
		public NetronVector Rotate(double phi,double theta,double psi) 
		{//Euler angles
			NetronVector t = new NetronVector(); // Temporary variables for holding old values
			double sX = Math.Sin(phi), cX = Math.Cos(phi), //to avoid reduntant calculations.
				sY = Math.Sin(theta), cY = Math.Cos(theta),
				sZ = Math.Sin(psi), cZ = Math.Cos(psi);
			NetronVector tmp = new NetronVector(this);
    
			t.mY = tmp.mY*cX - tmp.mZ*sX;  //rotate around the mX-axis
			t.mZ = tmp.mY*sX + tmp.mZ*cX;
			tmp.mY=t.mY;              //update vars needed in algorithm
			tmp.mZ=t.mZ;

			t.mX = tmp.mX*cY - tmp.mZ*sY;  //rotate around the mY-axis
			t.mZ =tmp. mX*sY + tmp.mZ*cY;
			tmp.mX=t.mX;              //update vars needed in algorithm again
			tmp.mZ=t.mZ;

			t.mX = tmp.mX*cZ - tmp.mY*sZ;  //rotate around the mZ-axis
			t.mY = tmp.mX*sZ + tmp.mY*cZ;
			tmp.mX=t.mX;              //update vars for final storage
			tmp.mY=t.mY;
			return t;
		}
		/// <summary>
		/// Rotates the vector 
		/// </summary>
		/// <param name="phi">the angle around the x-axis</param>
		/// <param name="theta">the angle around the y-axis</param>
		/// <param name="psi">the angle around the z-axis</param>
		public void RotateEquals(double phi,double theta,double psi) 
		{
			NetronVector t = new NetronVector(); // Temporary variables for holding old values
			double sX = Math.Sin(phi), cX = Math.Cos(phi), //to avoid reduntant calculations.
				sY = Math.Sin(theta), cY = Math.Cos(theta),
				sZ = Math.Sin(psi), cZ = Math.Cos(psi);

			t.mY = mY*cX - mZ*sX;  //rotate around the mX-axis
			t.mZ = mY*sX + mZ*cX;
			mY=t.mY;              //update vars needed in algorithm
			mZ=t.mZ;

			t.mX = mX*cY - mZ*sY;  //rotate around the mY-axis
			t.mZ = mX*sY + mZ*cY;
			mX=t.mX;              //update vars needed in algorithm again
			mZ=t.mZ;

			t.mX = mX*cZ - mY*sZ;  //rotate around the mZ-axis
			t.mY = mX*sZ + mY*cZ;
			mX=t.mX;              //update vars for final storage
			mY=t.mY;
			mZ=t.mZ;   
		}
		#endregion
		#endregion
	} 


	

}