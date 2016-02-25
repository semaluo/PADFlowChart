using System;
using System.Drawing;
namespace Netron.GraphLib.Utils
{
	/// <summary>
	/// Allows to paint arrows on the canvas which are not necessarily connected to
	/// the diagram.
	/// </summary>
	public class FreeArrow
	{

		#region Fields
		/// <summary>
		/// the starting point
		/// </summary>
		protected PointF mStartPoint = new PointF(0,0);
		/// <summary>
		/// the end point
		/// </summary>
		protected PointF mEndPoint;
		/// <summary>
		/// the color of the arrow
		/// </summary>
		protected Color mArrowColor = Color.Red;
		/// <summary>
		/// whether the arrowhead is filled
		/// </summary>
		protected bool mFilled = true;
		/// <summary>
		/// whether to show the label
		/// </summary>
		protected bool mShowLabel = false;
		/// <summary>
		/// the text
		/// </summary>
		protected string mText = string.Empty;
		/// <summary>
		/// the name of the arrow
		/// </summary>
		protected string mName = string.Empty;

		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the starting point of the arrow
		/// </summary>
		public PointF StartPoint
		{
			get{return mStartPoint;}
			set{mStartPoint = value;}
		}
		/// <summary>
		/// Gets or sets the endpoint of the arrow
		/// </summary>
		public PointF EndPoint
		{
			get{return mEndPoint;}
			set{mEndPoint = value;}
		}	
		/// <summary>
		/// Gets or sets the arrow's color
		/// </summary>
		public Color ArrowColor
		{
			get{return mArrowColor;}
			set{mArrowColor = value;}
		}
		/// <summary>
		/// Gets or sets whether the arrowhead is filled
		/// </summary>
		public bool Filled
		{
			get{return mFilled;}
			set{mFilled = value;}
		}
		/// <summary>
		/// Gets or sets whether the label is shown
		/// </summary>
		public bool ShowLabel
		{
			get{return mShowLabel;}
			set{mShowLabel = value;}
		}
		/// <summary>
		/// Gets or sets the text of the arrow
		/// </summary>
		public string Text
		{
			get{return mText;}
			set{mText = value;}
		}
		/// <summary>
		/// Gets or sets the name of the arrow
		/// </summary>
		public string Name
		{
			get{return mName;}
			set{mName = value;}
		}

		#endregion

		#region Constructors
		/// <summary>
		/// Default constrcutor
		/// </summary>
		public FreeArrow()
		{
			
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="startPoint"></param>
		/// <param name="endPoint"></param>
		/// <param name="color"></param>
		/// <param name="filled"></param>
		/// <param name="showLabel"></param>
		/// <param name="text"></param>
		public FreeArrow(	PointF startPoint, PointF endPoint, Color color,  bool filled, bool showLabel, string text)
		{
			this.mStartPoint = startPoint;
			this.mEndPoint = endPoint;
			this.mArrowColor = color;
			this.mText = text;
			this.mShowLabel = showLabel;
			this.mFilled = filled;

		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="startPoint"></param>
		/// <param name="endPoint"></param>
		public FreeArrow(	PointF startPoint, PointF endPoint)
		{
			this.mStartPoint = startPoint;
			this.mEndPoint = endPoint;						
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="startPoint"></param>
		/// <param name="endPoint"></param>
		/// <param name="text"></param>
		public FreeArrow(	PointF startPoint, PointF endPoint, string text)
		{
			this.mStartPoint = startPoint;
			this.mEndPoint = endPoint;						
			this.mText = text;
			this.mShowLabel = true;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Paints the arrow on the canvas
		/// </summary>
		/// <param name="g"></param>
		public  void PaintArrow(Graphics g)
		{
			try
			{
				g.DrawLine(new Pen(mArrowColor,1F),mStartPoint,mEndPoint);

				SolidBrush brush=new SolidBrush(mArrowColor);
				double angle = Math.Atan2(mEndPoint.Y - mStartPoint.Y,mEndPoint.X-mStartPoint.X);
				double length = Math.Sqrt((mEndPoint.X - mStartPoint.X)*(mEndPoint.X - mStartPoint.X)+(mEndPoint.Y - mStartPoint.Y)*(mEndPoint.Y - mStartPoint.Y))-10;
				double delta = Math.Atan2(7,length);
				PointF left = new PointF(Convert.ToSingle(mStartPoint.X + length*Math.Cos(angle-delta)),Convert.ToSingle(mStartPoint.Y+length*Math.Sin(angle-delta)));
				PointF right = new PointF(Convert.ToSingle(mStartPoint.X+length*Math.Cos(angle+delta)),Convert.ToSingle(mStartPoint.Y+length*Math.Sin(angle+delta)));

				PointF[] points={left, mEndPoint, right};
				if (mFilled)
					g.FillPolygon(brush,points);
				else
				{
					Pen p=new Pen(brush,1F);
					g.DrawLines(p,points);
				}
				if(mShowLabel)
				{
					if(mText==string.Empty)
						g.DrawString("(" + mEndPoint.X + "," + mEndPoint.Y +")",new Font("Verdana",10),brush,new PointF(mEndPoint.X-20,mEndPoint.Y-20));
					else
						g.DrawString(mText,new Font("Verdana",10),brush,new PointF(mEndPoint.X-20,mEndPoint.Y-20));

				}
			}
			catch(Exception )
			{
				//Trace.WriteLine(exc.Message);
			}
				
		}


		#endregion
	}
}
