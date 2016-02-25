using System;
using System.Diagnostics;
using System.IO;
using Netron.GraphLib.UI;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Xml;
using Netron.GraphLib.Interfaces;
namespace Netron.GraphLib.IO.Binary
{
	/// <summary>
	/// Utility class to binary (de)serialize a diagram (from) to file
	/// </summary>
	public class BinarySerializer
	{
		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public BinarySerializer()
		{
			
		}
		#endregion

		#region Methods


		/// <summary>
		/// Returns the ambiance of the GraphControl
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		private static BinaryAmbiance GetControlAmbiance(GraphControl site)
		{

			BinaryAmbiance ambiance = new BinaryAmbiance();

			ambiance.ShowGrid = site.ShowGrid;

			ambiance.GradientBottom = site.GradientBottom;

			ambiance.GradientTop = site.GradientTop;

			ambiance.AllowAddConnection = site.AllowAddConnection;

			ambiance.AllowAddShape = site.AllowAddShape;

			ambiance.AllowDeleteShape = site.AllowDeleteShape;

			ambiance.AllowMoveShape = site.AllowMoveShape;

			ambiance.AutomataPulse = site.AutomataPulse;

			ambiance.BackgroundColor = site.BackgroundColor;

			ambiance.BackgroundImagePath = site.BackgroundImagePath;

			ambiance.BackgroundType = site.BackgroundType;

			ambiance.EnableContextMenu = site.EnableContextMenu;

			ambiance.GradientMode = site.GradientMode;

			ambiance.GridSize = site.GridSize;

			ambiance.RestrictToCanvas = site.RestrictToCanvas;

			ambiance.Snap = site.Snap;

			ambiance.DefaultConnectionPath = site.DefaultConnectionPath;

			ambiance.DefaultConnectionEnd = site.DefaultConnectionEnd;

			ambiance.EnableLayout = site.EnableLayout;

			ambiance.GraphLayoutAlgorithm = site.GraphLayoutAlgorithm;

			ambiance.Locked = site.Locked;

			ambiance.EnableTooltip = site.EnableToolTip;

			ambiance.ShowAutomataController = site.ShowAutomataController;

			return ambiance;
		}

		/// <summary>
		/// Sets the GraphControl's properties with the given deserialized ambiance
		/// </summary>
		/// <param name="site"></param>
		/// <param name="ambiance"></param>
		private static void SetControlAmbiance(GraphControl site, BinaryAmbiance ambiance)
		{
			site.ShowGrid = ambiance.ShowGrid;

			site.GradientTop = ambiance.GradientTop;

			site.GradientBottom = ambiance.GradientBottom;

			site.AllowAddConnection=ambiance.AllowAddConnection ;

			 site.AllowAddShape = ambiance.AllowAddShape ;

			site.AllowDeleteShape = ambiance.AllowDeleteShape ;

			 site.AllowMoveShape = ambiance.AllowMoveShape;

			site.AutomataPulse = ambiance.AutomataPulse ;

			site.BackgroundColor = ambiance.BackgroundColor;

			site.BackgroundImagePath = ambiance.BackgroundImagePath ;

			site.BackgroundType = ambiance.BackgroundType;

			site.EnableContextMenu = ambiance.EnableContextMenu;

			site.GradientMode = ambiance.GradientMode;

			site.GridSize = ambiance.GridSize;

			site.RestrictToCanvas = ambiance.RestrictToCanvas;

			site.Snap = ambiance.Snap;

			site.DefaultConnectionPath = ambiance.DefaultConnectionPath;

			site.DefaultConnectionEnd = ambiance.DefaultConnectionEnd;

			site.EnableLayout = ambiance.EnableLayout;

			site.GraphLayoutAlgorithm = ambiance.GraphLayoutAlgorithm;

			site.EnableToolTip = ambiance.EnableTooltip;

			site.Locked = ambiance.Locked;

			site.ShowAutomataController = ambiance.ShowAutomataController;
		}

		/// <summary>
		/// Binary saves the diagram
		/// </summary>
		/// <param name="fileName">the file-path</param>
		/// <param name="site">the graph-control instance to be serialized</param>
		/// <returns></returns>
		public static  bool SaveAs(string fileName, GraphControl site)
		{

			FileStream fs = new FileStream(fileName, FileMode.Create);

			BinaryFormatter f = new BinaryFormatter();			
			
			try
			{
				BinaryCapsule capsule = new BinaryCapsule();
				capsule.Ambiance = GetControlAmbiance(site);
				capsule.Abstract = site.extract;
				
				capsule.Thumbnail = site.GetControlThumbnail(150,150);
				

				//Warning!: cleaning up, you need to unhook all events since unserializable classes hooked to events will give problems				
				f.Serialize(fs, capsule);
				return true;
			}			
			catch(Exception exc)			
			{
				site.OutputInfo("The graph was not saved, because some graph events were attached to non-serializable classes.\r\n This is a known issue and will be resolved in a later stadium.",OutputInfoLevels.Exception);
				Trace.WriteLine(exc.Message, "BinarySerializer.SaveAs");
				
				//DumpInfo();
			}
			catch
			{
				Trace.WriteLine("Non-CLS exception caught.","BinarySerializer.SaveAs");
			}
			finally
			{
				fs.Close();
			}
			return false;
		}
		/// <summary>
		/// Opens the binary-saved diagram
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="site"></param>
		public static  void Open (string fileName, GraphControl site)
		{
			FileStream fs=null;
			
			try
			{
				fs= File.OpenRead(fileName);
			}
			catch (System.IO.DirectoryNotFoundException exc)
			{
				System.Windows.Forms.MessageBox.Show(exc.Message);
			}
			catch(System.IO.FileLoadException exc)
			{				
				System.Windows.Forms.MessageBox.Show(exc.Message);
			}
			catch (System.IO.FileNotFoundException exc)
			{
				System.Windows.Forms.MessageBox.Show(exc.Message);
			}
			catch
			{				
				site.OutputInfo("Non-CLS exception caught.","BinarySerializer.SaveAs", OutputInfoLevels.Exception);
			}
			//donnot open anything if filestream is not there
			if (fs==null) return;
			try
			{
				
				BinaryFormatter f = new BinaryFormatter();

				BinaryCapsule capsule = (BinaryCapsule) f.Deserialize(fs); //so simple, so powerful
				
				GraphAbstract tmp = capsule.Abstract;

				SetControlAmbiance(site, capsule.Ambiance);

				tmp.Site=site;
				//site.extract = new GraphAbstract();
				site.extract = tmp;
				

				//the paintables are not serialized and need to be filled
				site.extract.paintables.AddRange(site.extract.Shapes);
				site.extract.paintables.AddRange(site.extract.Connections);
				site.extract.SortPaintables();


				UnwrapBundle(tmp, site);
			}
			catch(SerializationException exc)			
			{
				System.Windows.Forms.MessageBox.Show(exc.Message);
			}
			catch(System.Reflection.TargetInvocationException exc)
			{
				site.OutputInfo(exc.Message, "BinarySerializer.Open", OutputInfoLevels.Exception);
			}
			catch(Exception exc)
			{
				site.OutputInfo(exc.Message, "BinarySerializer.Open", OutputInfoLevels.Exception);
			}
			catch
			{
				site.OutputInfo("Non-CLS exception caught.", "BinarySerializer.Open", OutputInfoLevels.Exception);
			}
			finally
			{
				if(fs!=null)
					fs.Close();				
			}
		}

		/// <summary>
		/// Unwraps the IEntityBundle to the given site
		/// </summary>
		/// <param name="bundle"></param>
		/// <param name="site"></param>
		public static void UnwrapBundle(IEntityBundle bundle, GraphControl site)
		{
			if(bundle==null) return;

			foreach (Shape o in bundle.Shapes)
			{
				o.Site = site;										
				o.PostDeserialization();

			}

			bool fromFound, toFound;

			foreach(Connection con in bundle.Connections)
			{
				con.Site =site;
				fromFound = false;
				toFound = false;
				foreach (Shape o in bundle.Shapes)
				{
					foreach(Connector cr in o.Connectors)	
					{
						if(cr.UID==con.To.UID) 
						{
							con.To = cr;
							cr.Connections.Add(con);
							toFound = true;
						}
						else if(cr.UID==con.From.UID) 
						{
							con.From = cr;
							cr.Connections.Add(con);
							fromFound = true;
						}
					}
					if(fromFound && toFound) break;
				}
				if(!fromFound || !toFound)
				{
					site.OutputInfo("Could not find matching connectors for a connection.","BinarySerializer.Open",OutputInfoLevels.Info);
					if(site.extract.paintables.Contains(con)) site.extract.paintables.Remove(con);
					continue;
				}

				if(!(con.Painter is BezierPainter)) //because the BezierPainter has the Points of the BezierPainter serialized						
					con.Painter.Points=con.GetConnectionPoints();					
				con.Tracker = new ConnectionTracker(con.InsertionPoints, true);
				con.PostDeserialization();
										
			}

		}
		/// <summary>
		/// Dumps info related to the binary serialization
		/// </summary>
		public static void DumpInfo()
		{
			System.Reflection.MemberInfo[] mi;
			

			mi= System.Runtime.Serialization.FormatterServices.GetSerializableMembers(typeof(GraphAbstract));
			Trace.WriteLine(  Environment.NewLine + "________________________" + Environment.NewLine + "GraphAbstract" + Environment.NewLine + "________________________" + Environment.NewLine);
			DumpInfo(mi);

			mi= System.Runtime.Serialization.FormatterServices.GetSerializableMembers(typeof(Shape));
			Trace.WriteLine("Shape" + Environment.NewLine + Environment.NewLine);
			DumpInfo(mi);

		
		}
		/// <summary>
		/// Dumps info related to the binary serialization
		/// </summary>
		/// <param name="array"></param>
		private static void DumpInfo(System.Reflection.MemberInfo[] array)
		{
			for(int k=0; k<array.Length; k++)
			{
				Trace.WriteLine(array[k].Name + "   [" + array[k].ToString() + "]");
			}
		}
		#endregion
	}
}
