
using System;
using System.Collections;
using System.Reflection;
using Netron.GraphLib.IO.NML;
namespace Netron.GraphLib.Attributes
{
	/// <summary>
	/// Attribute class for designating which properties will be serialized
	/// by the NML serializer.
	/// </summary>	
	[AttributeUsage(AttributeTargets.Property,AllowMultiple = false)]
	public class GraphMLDataAttribute : System.Attribute
	{		
	
		/// <summary>
		/// Constructor
		/// </summary>
		public GraphMLDataAttribute()
		{

		}

		/// <summary>
		/// Returns a PropertiesHashtable of name-values for the given object's properties
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static PropertiesHashtable GetValuesOfTaggedFields(object value) 
		{
			PropertiesHashtable props = new PropertiesHashtable();

			foreach (PropertyInfo pi in value.GetType().GetProperties()) 
			{	
				if (Attribute.IsDefined(pi, typeof(GraphMLDataAttribute))) 
				{
					props.Add(pi.Name,pi.GetValue(value,null));
				}
			}

			return props;
		}
	}
}
