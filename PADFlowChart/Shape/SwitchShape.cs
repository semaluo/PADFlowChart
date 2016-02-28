using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using Netron.GraphLib;

namespace PADFlowChart
{

    [Serializable]
    public class SwitchShape : AbstractFlowChartShape
    {
        private int m_switchCount = 3;
        private Connector m_leftConnector;
        private ConnectorCollection m_switchConnectors = new ConnectorCollection();


        public SwitchShape(int switchCount):base()
        {
            m_switchCount = switchCount;
            if (m_switchCount < 3)
            {
                m_switchCount = 3;
            }

            Init();
        }


        private void Init()
        {
            m_leftConnector = new Connector(this, "Left", true);
            Connectors.Add(m_leftConnector);

            Connector t_connector = null;
            for (int i = 0; i < m_switchCount; i++)
            {
                t_connector = new Connector(this, i.ToString(), true);
                t_connector.AllowNewConnectionsTo = false;
                m_switchConnectors.Add(t_connector);
            }

            Connectors.AddRange(m_switchConnectors);
        }


        public override PointF ConnectionPoint(Connector c)
        {
            if (c == m_leftConnector)
            {
                return new PointF(Rectangle.Left, Rectangle.Top + Rectangle.Height / 2);
            }

            //if (m_switchCount < 3) m_switchCount = 3;

            float t_height = Rectangle.Height/(m_switchCount - 1);

            for (int i = 0; i < m_switchCount; i++)
            {
                if (c == m_switchConnectors[i])
                {
                    return new PointF(Rectangle.Right, Rectangle.Top + i*t_height);
                }
            }

            return new PointF(0, 0);
        }

        public override void Paint(Graphics g)
        {
            base.Paint(g);

            float t_switchWidth = Rectangle.Width * 8 / 10;
            //if (m_switchCount < 3) m_switchCount = 3;
            float t_swtchHeight = Rectangle.Height / (2*m_switchCount - 2);

            List<PointF> list = new List<PointF>();
            PointF t_Point;

            //start from RightBottom
            t_Point = new PointF(Rectangle.Right, Rectangle.Bottom);
            list.Add(t_Point);

            //bottom edge
            t_Point = new PointF(Rectangle.Left, Rectangle.Bottom);
            list.Add(t_Point);

            //left edge
            t_Point = new PointF(Rectangle.Left, Rectangle.Top);
            list.Add(t_Point);

            //top edge
            t_Point = new PointF(Rectangle.Right, Rectangle.Top);
            list.Add(t_Point);


            //add switch lines
            float t_x = Rectangle.Left + t_switchWidth;
            float t_y;
            for (int i = 0; i < 2*m_switchCount - 3; i++)
            {
                t_y = Rectangle.Top + (i + 1) * t_swtchHeight;
                if (i%2 == 0)
                {
                    t_Point = new PointF(t_x, t_y);
                }
                else
                {
                    t_Point = new PointF(Rectangle.Right, t_y);
                }
                list.Add(t_Point);
            }

            PointF[] points = new PointF[2*m_switchCount + 1];
            list.CopyTo(points);

            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(points);
            
            
            //Draw shape line and shape background
            g.FillPath(new SolidBrush(ShapeColor), path);
            g.DrawPath(Pen, path);

            //Draw Text
            RectangleF t_textRect = new RectangleF(Rectangle.Location, new SizeF(t_switchWidth, Rectangle.Height));
            t_textRect.Inflate(-1, -1);
            if (!string.IsNullOrEmpty(Text))
                g.DrawString(Text, this.Font, this.TextBrush, t_textRect);
        }

        #region Serialization
        protected SwitchShape(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            m_switchCount = info.GetInt32("m_switchCount");
            

            m_leftConnector = (Connector)info.GetValue("m_leftConnector", typeof(Connector));
            m_leftConnector.BelongsTo = this;
            Connectors.Add(m_leftConnector);

            m_switchConnectors = (ConnectorCollection)info.GetValue("m_switchConnectors", typeof(ConnectorCollection));
            foreach (Connector connector in m_switchConnectors)
            {
                connector.BelongsTo = this;
                Connectors.Add(connector);
            }

        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("m_switchCount", m_switchCount);
            info.AddValue("m_leftConnector", m_leftConnector);
            info.AddValue("m_switchConnectors", m_switchConnectors);
        }
        #endregion

        public override Connection CreateConnection(Connector connector)
        {
            if (connector == m_leftConnector)
            {
                return new FlowChartConnection();
            }

            for (int i = 0; i < m_switchConnectors.Count; i++)
            {
                if (connector == m_switchConnectors[i])
                {
                    return new SwitchConnection(i+1, m_switchConnectors.Count);
                }
            }

            return new Connection();
        }
    }
}