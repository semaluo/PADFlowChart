using System;
using System.Reflection;
using System.Configuration;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Collections;
using Netron.GraphLib.Configuration;

namespace Netron.GraphLib.Configuration
{
	/// <summary>
	/// Reads the custom configuration section by implementing the 
	/// IConfigurationSectionHandler interface.
	/// </summary>
	public class GraphLibConfigurationHandler :IConfigurationSectionHandler
	{

		/// <summary>
		/// Returns an  ArrayList object with all the paths		
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="configContext"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public object Create(object parent, object configContext, XmlNode section)
		{
			//read the section and populate the collections
			XmlNodeList rootnodes=section.ChildNodes;
			ArrayList libs = null;
			if(rootnodes.Count>0)
			{
				libs = new ArrayList();
				foreach(XmlNode node in rootnodes)
				{
					if (node.Name=="GraphLib") 
					{
						
						libs.Add(node.Attributes["location"].Value);
						Trace.WriteLine("Found the GraphLib with value '" + node.Attributes["location"].Value + "'","ConfigurationHandler");
					}
					
					
					
					
				}
			}
			return libs;
		}

	
	}
	
}
