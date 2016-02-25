using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using Netron.GraphLib;

namespace PADFlowChart
{

    [Serializable]
    public class IfShape : AbstractFlowChartShape
    {
        private Connector m_leftConnector;
        private Connector m_yesConnector;
        private Connector m_noConnector;

        public IfShape():base()
        {
            Init();
        }


        private void Init()
        {
            m_leftConnector = new Connector(this, "Left", true);
            Connectors.Add(m_leftConnector);

            m_yesConnector = new Connector(this, "Yes", true);
            m_yesConnector.AllowNewConnectionsTo = false;
            Connectors.Add(m_yesConnector);

            m_noConnector = new Connector(this, "No", true);
            m_noConnector.AllowNewConnectionsTo = false;
            Connectors.Add(m_noConnector);
        }


        public override PointF ConnectionPoint(Connector c)
        {
            if (c == m_leftConnector)
            {
                return new PointF(Rectangle.Left, Rectangle.Top + Rectangle.Height / 2);
            }

            if (c == m_yesConnector)
            {
                return new PointF(Rectangle.Right, Rectangle.Top);
            }

            if (c == m_noConnector)
            {
                return new PointF(Rectangle.Right, Rectangle.Bottom);
            }

            return new PointF(0, 0);
        }

        public override void Paint(Graphics g)
        {
            base.Paint(g);

            float t_width = Rectangle.Width * 8 / 10;
            float t_height = Rectangle.Height;

            PointF[] points = new PointF[6];
            points[0] = new PointF(Rectangle.Left, Rectangle.Top);
            points[1] = new PointF(Rectangle.Right, Rectangle.Top);
            points[2] = new PointF(Rectangle.Left + t_width, Rectangle.Top + Rectangle.Height/2);
            points[3] = new PointF(Rectangle.Right, Rectangle.Bottom);
            points[4] = new PointF(Rectangle.Left, Rectangle.Bottom);
            points[5] = new PointF(Rectangle.Left, Rectangle.Top);

            GraphicsPath path = new GraphicsPath();
            path.AddLines(points);
            
            
            //Draw shape line and shape background
            g.FillPath(new SolidBrush(ShapeColor), path);
            g.DrawPath(Pen, path);

            //Draw Text
            RectangleF t_textRect = new RectangleF(Rectangle.Location, new SizeF(t_width, t_height));
            t_textRect.Inflate(-1, -1);
            if (!string.IsNullOrEmpty(Text))
                g.DrawString(Text, this.Font, this.TextBrush, t_textRect);
        }

        #region Serialization
        protected IfShape(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            m_leftConnector = (Connector)info.GetValue("m_leftConnector", typeof(Connector));
            m_leftConnector.BelongsTo = this;
            Connectors.Add(m_leftConnector);

            m_yesConnector = (Connector)info.GetValue("m_yesConnector", typeof(Connector));
            m_yesConnector.BelongsTo = this;
            Connectors.Add(m_yesConnector);

            m_noConnector = (Connector)info.GetValue("m_noConnector", typeof(Connector));
            m_noConnector.BelongsTo = this;
            Connectors.Add(m_noConnector);

        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("m_leftConnector", m_leftConnector);
            info.AddValue("m_yesConnector", m_yesConnector);
            info.AddValue("m_noConnector", m_noConnector);
        }
        #endregion

    }
}