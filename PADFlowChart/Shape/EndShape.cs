using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netron.GraphLib;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib.UI;
using System.Runtime.Serialization;

namespace PADFlowChart
{
    [Serializable]
    public class EndShape : AbstractFlowChartShape
    {
        private Connector m_leftConnector;

        public EndShape() : base()
        {
            Init();
        }


        private void Init()
        {
            m_leftConnector = new Connector(this, "Left", true);
            Connectors.Add(m_leftConnector);
        }


        public override PointF ConnectionPoint(Connector c)
        {
            if (c == m_leftConnector)
            {
                return new PointF(Rectangle.Left, Rectangle.Top + Rectangle.Height / 2);
            }

            return new PointF(0, 0);
        }

        public override void Paint(Graphics g)
        {
            base.Paint(g);
            g.FillRectangle(new SolidBrush(ShapeColor), Rectangle);
            g.DrawRectangle(Pen, System.Drawing.Rectangle.Round(Rectangle));

            FillCircleInCenter(g);

            //if (!string.IsNullOrEmpty(Text))
            //    g.DrawString(Text, this.Font, this.TextBrush, System.Drawing.RectangleF.Inflate(Rectangle, 0, -2));
        }

        private void FillCircleInCenter(Graphics g)
        {
            PointF center = new PointF(Rectangle.Left + Rectangle.Width / 2,
                Rectangle.Top + Rectangle.Height / 2);
            RectangleF square = new RectangleF(center, new Size(0, 0));
            float radius = Rectangle.Height < Rectangle.Width ? Rectangle.Height / 4 : Rectangle.Width / 4;
            square.Inflate(radius, radius);

            g.FillEllipse(Brushes.DarkRed, square);
            g.DrawEllipse(Pens.DarkRed, square);

        }

        #region Serialization
        protected EndShape(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            m_leftConnector = (Connector)info.GetValue("m_leftConnector", typeof(Connector));
            m_leftConnector.BelongsTo = this;
            Connectors.Add(m_leftConnector);

        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("m_leftConnector", m_leftConnector);
        }
        #endregion

    }
}
