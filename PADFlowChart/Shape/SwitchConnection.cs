using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Netron.GraphLib;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib.UI;

namespace PADFlowChart
{
    [Serializable]
    public class SwitchConnection : Connection
    {
        private int m_index;
        private int m_count;

        public SwitchConnection(int index, int count) : base()
        {
            m_index = index;
            m_count = count;
            Init();
        }

        public SwitchConnection(IGraphSite site) : base(site)
        {
            Init();
        }

        protected SwitchConnection(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            m_index = info.GetInt32("m_index");
            m_count = info.GetInt32("m_count");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("m_index", m_index);
            info.AddValue("m_count", m_count);
        }

        private void Init()
        {
            Text = String.Empty;
            ShowLabel = true;
        }



        public override PointF[] GetConnectionPoints()
        {
            PointF[] points;
            PointF t_to = PointF.Empty;
            PointF t_from = Point.Empty;

            if (From == null) return new PointF[] { PointF.Empty };

            points = new PointF[5];
            t_from = From.Location;
            t_to = (To != null) ? To.Location : ToPoint;

            points[0] = t_from;

            //only consider from.X < to.X
            if (t_from.X >= t_to.X)
            {
                for (int i = 0; i < 5; i++)
                {
                    points[i] = t_from;
                }

                return points;
            }

            float t_midX;
            if (t_from.Y > t_to.Y)
            {
                t_midX = t_from.X + Math.Abs(t_to.X - t_from.X) * m_index / m_count;
            }
            else
            {
                t_midX = t_from.X + Math.Abs(t_to.X - t_from.X) * (m_count - m_index + 1) / m_count;
            }

            points[1] = new PointF(t_midX, t_from.Y);

            float t_midY = 0;
            if (To != null)
            {
                t_midY = To.BelongsTo.Rectangle.Top;
            }
            else
            {
                t_midY = t_to.Y;
            }

            points[2] = new PointF(t_midX, t_midY);
            points[3] = new PointF(t_to.X, t_midY);
            points[4] = t_to;



            return points;
        }

        public override void PaintLabel(Graphics g)
        {
            if (ShowLabel && Text.Trim().Length > 0)
            {
                g.DrawString(this.Text, Font, new SolidBrush(Color.Black), From.Location + new SizeF(10, -15));
            }
        }

        public override void PostDeserialization()
        {
            base.PostDeserialization();
            ShowLabel = true;
        }


    }
}