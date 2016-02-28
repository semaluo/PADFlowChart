using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Netron.GraphLib;
using Netron.GraphLib.Interfaces;

namespace PADFlowChart
{
    [Serializable]
    public class BlockShape : AbstractFlowChartShape
    {
        private Connector m_leftConnector;
        //private RectangleF m_clickRectangle;
        private GraphLayer m_linkedLayer;

        public GraphLayer LinkedLayer
        {
            get { return m_linkedLayer; }
            set { m_linkedLayer = value; }
        }

        public RectangleF LinkRectangle
        {
            get
            {
                float t_width = Rectangle.Width / 5;
                float t_height = Rectangle.Height / 5;
                float t_x = Rectangle.Right - t_width;
                float t_y = Rectangle.Top;
                return new RectangleF(t_x, t_y, t_width, t_height);
            }
        }

        public BlockShape() : base()
        {
            Init();

            BindingEventHandler();
        }

        public BlockShape(IGraphSite site): base(site)
        {
            Init();

            m_linkedLayer = GenerateLayer(UID.ToString().Trim());
            site.Abstract.Insert(m_linkedLayer);
            BindingEventHandler();

        }

        private GraphLayer GenerateLayer(string layerName)
        {
            GraphLayer layer = null;
            if (Site != null)
            {
                layer = Site.Abstract.Layers[layerName];
            }

            if (layer == null)
            {
                layer = new GraphLayer(layerName);
                layer.Visible = false;
                layer.UseColor = false;
            }

            return layer;

        }

        private void BindingEventHandler()
        {
            this.OnMouseUp += BlockShape_OnMouseUp;
        }

        private void BlockShape_OnMouseUp(object sender, MouseEventArgs e)
        {
            PointF p = new PointF(e.X - Site.AutoScrollPosition.X, e.Y - Site.AutoScrollPosition.Y);

            if (LinkRectangle.Contains(p))
            {
                if (m_linkedLayer != null)
                {
                    Abstract.ActiveLayer(m_linkedLayer.Name);
                }
                else
                {
                    m_linkedLayer = GenerateLayer(UID.ToString().Trim());
                    Abstract.Layers.Add(m_linkedLayer);
                    Abstract.ActiveLayer(m_linkedLayer.Name);
                }
                Site.Invalidate();
            }
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


            //Draw Rectangle
            g.FillRectangle(new SolidBrush(ShapeColor), Rectangle);
            g.DrawRectangle(Pen, System.Drawing.Rectangle.Round(Rectangle));

            //Draw clickable block
            g.DrawRectangle(Pen, System.Drawing.Rectangle.Round(LinkRectangle));

            //Draw Text
            RectangleF t_textRect = new RectangleF(Rectangle.Location, new SizeF(Rectangle.Width - LinkRectangle.Width, Rectangle.Height));
            t_textRect.Inflate(-1, -1);
            if (!string.IsNullOrEmpty(Text))
                g.DrawString(Text, Font, TextBrush, t_textRect);
        }

        public override Cursor GetCursor(PointF p)
        {
            if (LinkRectangle.Contains(p))
            {
                return Cursors.Hand;
            }

            return base.GetCursor(p);
        }

        #region Serialization
        protected BlockShape(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            m_leftConnector = (Connector)info.GetValue("m_leftConnector", typeof(Connector));
            m_leftConnector.BelongsTo = this;
            Connectors.Add(m_leftConnector);

            try
            {
                this.URL = string.Empty;
                this.URL = info.GetString("m_linkedLayer");
            }
            catch (Exception)
            {
            }

            BindingEventHandler();

        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("m_leftConnector", m_leftConnector);
            if (m_linkedLayer != null)
            {
                info.AddValue("m_linkedLayer", m_linkedLayer.Name);
            }
        }

        public override void PostDeserialization()
        {
            base.PostDeserialization();
            if (!string.IsNullOrEmpty(URL))
            {
                m_linkedLayer = Abstract.Layers[URL.Trim()];
                URL = string.Empty;
            }
        }
        #endregion

    }
}