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
    public class FlowChartConnection: Connection
    {
        private bool m_changing = false;
        private Point m_startPoint = Point.Empty;
        

        public FlowChartConnection(): base()
        {
            Init();
        }

        public FlowChartConnection(IGraphSite site):base(site)
        {
            Init();
        }

        protected FlowChartConnection(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            BindEvent();
        }

        private void Init()
        {
            Text = String.Empty;
            ShowLabel = true;
            BindEvent();
        }

        private void BindEvent()
        {
            OnMouseDown += FlowChartConnection_OnMouseDown;
            OnMouseMove += FlowChartConnection_OnMouseMove;
            OnMouseUp += FlowChartConnection_OnMouseUp;
        }

        private bool HitChangePoint(Point point)
        {
            return false; //temporayly close this function

            PointF t_to = PointF.Empty;
            PointF t_from = Point.Empty;
            if (From == null || To == null)
            {
                return false;
            }

            if (From.Name == "Left" && To.Name == "Left" )
            {
                return false;
            }

            t_to = To.Location;
            t_from = From.Location;

            Rectangle r = new Rectangle((int)t_to.X, (int)t_from.Y, 0, 0);

            r.Inflate(3, 3);
            return r.Contains(point);
        }

        private void FlowChartConnection_OnMouseDown(object sender, MouseEventArgs e)
        {
            if (HitChangePoint(e.Location))
            {
                m_changing = true;
                m_startPoint = e.Location;

                if (Site != null && Site is GraphControl)
                {
                    ((GraphControl) Site).StartCapture(this);
                    ((GraphControl) Site).Cursor = Cursors.SizeNS;
                }

            }
        }



        private void FlowChartConnection_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (m_changing)
            {
                if (To == null)
                {
                    return;
                }

                if (Site != null && Site is GraphControl)
                {
                    ((GraphControl)Site).Cursor = Cursors.SizeWE;
                }

                this.Invalidate();
                To.BelongsTo.Invalidate();

                int t_dx = e.Location.X - m_startPoint.X;
                int t_dy = 0;
                m_startPoint = e.Location;
                PointF t_rectLocation = To.BelongsTo.Rectangle.Location + new Size(t_dx, t_dy);
                To.BelongsTo.Rectangle = new RectangleF(t_rectLocation, To.BelongsTo.Rectangle.Size);

                this.Invalidate();
                To.BelongsTo.Invalidate();

            }
        }
        private void FlowChartConnection_OnMouseUp(object sender, MouseEventArgs e)
        {
            if (m_changing)
            {
                m_changing = false;
                m_startPoint = Point.Empty;

                if (Site != null && Site is GraphControl)
                {
                    ((GraphControl)Site).EndCapture();
                    ((GraphControl)Site).Cursor = Cursors.Arrow;

                }

            }
        }


        public override PointF[] GetConnectionPoints()
        {
            PointF[] points;
            PointF t_to = PointF.Empty;
            PointF t_from = Point.Empty;

            if (From == null) return new PointF[] { PointF.Empty };

            if (From != null && To != null && From.Name == "Left" && To.Name == "Left")
            {
                //From and To connector both are Left connector, 
                //The connection must be vertical connection
                points = new PointF[2];
                points[0] = From.Location;
                points[1] = new PointF(From.Location.X, To.Location.Y);
                return points;
            }

            points = new PointF[3];
            t_from = From.Location;
            t_to = (To != null) ? To.Location : ToPoint;

            points[0] = t_from;
            points[1] = new PointF(t_to.X, t_from.Y);
            points[2] = t_to;

            return points;
        }

        public override void Paint(Graphics g)
        {
            
            base.Paint(g);

            if (IsSelected)
            {
                //Draw change point if necessary
                PointF[] points = GetConnectionPoints();
                if (points.Length < 3)
                {//Only 2 points, then do not draw changing point
                    return;
                }
                RectangleF r = new RectangleF(points[1].X, points[1].Y, 0, 0);
                r.Inflate(+3, +3);
                g.FillRectangle(new SolidBrush(Color.White), r);
                g.DrawRectangle(new Pen(Color.Black, 1), Rectangle.Round(r));
            }
        }

        public override Cursor GetCursor(PointF p)
        {
            return Cursors.Arrow;

            if (IsSelected)
            {
                PointF[] points = GetConnectionPoints();
                RectangleF r = new RectangleF(points[1].X, points[1].Y, 0, 0);
                r.Inflate(+3, +3);
                if (r.Contains(p))
                {
                    return Cursors.SizeWE;
                }
            }
            return Cursors.Arrow;
        }

        public override void PaintLabel(Graphics g)
        {
            if (ShowLabel && Text.Trim().Length > 0)
            {
                g.DrawString(this.Text, Font, new SolidBrush(Color.Black), From.Location + new SizeF(10,-15));
            }
        }

        public override void PostDeserialization()
        {
            base.PostDeserialization();

            ShowLabel = true;


        }

        public override string Text
        {
            get
            {
                if (From != null && (From.Name == "Yes" || From.Name == "No") )
                {
                    return From.Name;
                }

                return base.Text;
            }
        }

    }
}