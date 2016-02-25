using System;

namespace Netron.GraphLib.Maths
{
	/// <summary>
	/// Utilities related to angles
	/// </summary>
	public sealed class Angle
	{
		/// <summary>
		/// Converts radians to degrees
		/// </summary>
		/// <param name="radian">an angle in radians</param>
		/// <returns>the angle expressed as degrees</returns>
		public static float RadianToDegree(float radian) 
		{
			return (float) ((radian * 360) / (2 * Math.PI));
		}

		/// <summary>
		/// Converts degrees to radians
		/// </summary>
		/// <param name="degree">an angle in degrees</param>
		/// <returns>an angles expresses as radians</returns>
		public static float DegreeToRadian(float degree) 
		{
			return (float) ((degree * (2 * Math.PI)) / 360);
		}

		/// <summary>
		/// See the 'StaticHolderTypesShouldNotHaveConstructors' error of FxCop
		/// </summary>
		private Angle(){}

	}
}
