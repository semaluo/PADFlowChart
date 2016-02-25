using System;
using System.Drawing;
using System.ComponentModel;
using System.Globalization;
namespace Netron.GraphLib
{

	/// <summary>
	/// Custom type-converted for PointF objects
	/// </summary>
	internal class PointFTypeConverter : ExpandableObjectConverter 
	{
		#region Methods  
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type t) 
		{
			if (t == typeof(string)) 
			{
				return true;
			}
			return base.CanConvertFrom(context, t);
		}

		public override object ConvertFrom(	ITypeDescriptorContext context, CultureInfo info,	object value) 
		{
			if (value is string) 
			{
				string s = (string) value;
				try 
				{	
					// parse the format "Last, First (Age)"
					//
					int comma = s.IndexOf(',');
					if (comma != -1) 
					{
						// now that we have the comma, get 
						// the width.
						string x = s.Substring(0, comma);							
						string y = s.Substring(comma + 1, s.Length - comma - 1);
						SizeF sf = new SizeF(float.Parse(x), float.Parse(y));							
						return sf;
						
					}
				}
				catch {}			
				throw new ArgumentException(
					"Can not convert '" + s + 
					"' to type Person");
         
			}
			return base.ConvertFrom(context, info, value);
		}
                                 
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType) 
		{
			if (destType == typeof(string) && value is PointF) 
			{
				PointF p = (PointF)value;
				
				return "(" + p.X + "," + p.Y +")";
			}
			return base.ConvertTo(context, culture, value, destType);
		}   
		#endregion
	}

}
