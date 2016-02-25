using System;
using System.Drawing;
using System.ComponentModel;
using System.Globalization;
namespace Netron.GraphLib
{
	
	internal class SizeFTypeConverter : ExpandableObjectConverter 
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
				try 
				{
					string s = (string) value;
					// parse the format "Last, First (Age)"
					//
					int comma = s.IndexOf(',');
					if (comma != -1) 
					{
						// now that we have the comma, get 
						// the width.
						string w = s.Substring(0, comma);							
						string h = s.Substring(comma + 1, s.Length - comma - 1);
						SizeF sf = new SizeF(float.Parse(w), float.Parse(h));							
						return sf;
						
					}
				}
				catch {}			
				throw new ArgumentException(
					"Can not convert '" + (string)value + 
					"' to type Person");
         
			}
			return base.ConvertFrom(context, info, value);
		}
                                 
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType) 
		{
			if (destType == typeof(string) && value is SizeF) 
			{
				SizeF p = (SizeF)value;
				
				return "(" + p.Width + "," + p.Height +")";
			}
			return base.ConvertTo(context, culture, value, destType);
		}   
		#endregion
	}

}
