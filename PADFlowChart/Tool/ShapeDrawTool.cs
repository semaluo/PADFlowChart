using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Netron.GraphLib;
using Netron.GraphLib.UI;

namespace PADFlowChart
{
    public static class ShapeDrawTool
    {
        private static Type m_shapeType;
        private static Shape m_shape = null;
        private static GraphControl m_graphControl = null;

        private static bool m_startDraw = false;
        private static PointF m_startPoint = new PointF(0,0);

        private static readonly float m_defaultWidth = 150;
        private static float m_defaultHeight = 50;


        //public static Type ShapeType
        //{
        //    get { return m_shapeType; }
        //    set { m_shapeType = value; }
        //}

        public static void DrawShape(Type shapeType, GraphControl graphicControl)
        {
            if (shapeType == null || !(shapeType.IsSubclassOf(typeof(Shape))))
            {
                return;
            }

            if (graphicControl == null)
            {
                return;
            }

            m_shapeType = shapeType;
            m_graphControl = graphicControl;

            m_graphControl.Cursor = MouseCursors.Cross; 

            m_startDraw = true;
            m_graphControl.Locked = true;
            //m_shape.IsVisible = false;
            //if (!m_graphControl.Shapes.Contains(shape))
            //{
            //    m_graphControl.AddShape(m_shape);
            //}

            m_graphControl.MouseDown -= OnMouseDown;
            m_graphControl.MouseMove -= OnMouseMove;
            m_graphControl.MouseUp -= OnMouseUp;


            m_graphControl.MouseDown += OnMouseDown;
            m_graphControl.MouseMove += OnMouseMove;
            m_graphControl.MouseUp += OnMouseUp;



        }

        private static void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (m_graphControl == null || m_shapeType == null) return;

            if ( e.Button == MouseButtons.Right)
            {
                CompleteClear();
            }

            if (m_startDraw)
            {
                //m_startPoint = new PointF(e.Location.X, e.Location.Y);
                m_startPoint = new PointF(e.X - m_graphControl.AutoScrollPosition.X, e.Y - m_graphControl.AutoScrollPosition.Y);
                m_shape = (Shape)Activator.CreateInstance(m_shapeType);
                m_shape.Rectangle = new RectangleF(m_startPoint,new Size(0,0));
                m_shape.IsVisible = true;
                m_graphControl.AddShape(m_shape);
            }
            /*
            this.formStatusLabel.Text = string.Format("MouseDown : X = {0},Y = {1}", e.Location.X, e.Location.Y);
            m_startPoint = new PointF(e.Location.X, e.Location.Y);
            if (_mDrawShape)
            {

                _mShape = new SequenceShape();
                _mShape.Rectangle = new System.Drawing.RectangleF(e.Location, new SizeF(1, 1));
                graphControl.AddShape(_mShape);
            }
            */
        }




        private static void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (m_startDraw && m_graphControl !=null && m_shape != null)
            {
                m_shape.Invalidate();
                PointF t_point = new PointF(e.X - m_graphControl.AutoScrollPosition.X, e.Y - m_graphControl.AutoScrollPosition.Y);
                float t_left = (m_startPoint.X < t_point.X ? m_startPoint.X : t_point.X);
                float t_right = (m_startPoint.X >= t_point.X ? m_startPoint.X : t_point.X);
                float t_top = (m_startPoint.Y < t_point.Y ? m_startPoint.Y : t_point.Y);
                float t_bottom = (m_startPoint.Y >= t_point.Y ? m_startPoint.Y : t_point.Y);
                m_shape.Rectangle = RectangleF.FromLTRB(t_left, t_top, t_right, t_bottom);
                m_shape.Invalidate();
            }

            /*
            this.formStatusLabel.Text = string.Format("X:{0},Y:{1}", e.X, e.Y);
            if (_mDrawShape && _mShape != null)
            {
                _mShape.Invalidate();

                float t_left = (m_startPoint.X < e.Location.X ? m_startPoint.X : e.Location.X);
                float t_right = (m_startPoint.X >= e.Location.X ? m_startPoint.X : e.Location.X);
                float t_top = (m_startPoint.Y < e.Location.Y ? m_startPoint.Y : e.Location.Y);
                float t_bottom = (m_startPoint.Y >= e.Location.Y ? m_startPoint.Y : e.Location.Y);

                _mShape.Rectangle = RectangleF.FromLTRB(t_left, t_top, t_right, t_bottom);
                _mShape.Invalidate();
            }
            */
        }

        private static void OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (m_startDraw && m_graphControl != null && m_shape != null)
            {
                m_graphControl.Cursor = System.Windows.Forms.Cursors.Default;

                PointF t_point = new PointF(e.X - m_graphControl.AutoScrollPosition.X, e.Y - m_graphControl.AutoScrollPosition.Y);
                float t_left = (m_startPoint.X < t_point.X ? m_startPoint.X : t_point.X);
                float t_right = (m_startPoint.X >= t_point.X ? m_startPoint.X : t_point.X);
                float t_top = (m_startPoint.Y < t_point.Y ? m_startPoint.Y : t_point.Y);
                float t_bottom = (m_startPoint.Y >= t_point.Y ? m_startPoint.Y : t_point.Y);

                if (t_right - t_left < 10)
                {
                    t_right = t_left + m_defaultWidth;
                }

                if (t_bottom - t_top < 10)
                {
                    t_bottom = t_top + m_defaultHeight;
                }
                m_shape.Rectangle = RectangleF.FromLTRB(t_left, t_top, t_right, t_bottom);
                m_shape.Invalidate();

                CompleteClear();

            }
            /*
            if (_mDrawShape)
            {
                graphControl.Locked = false;
                float t_left = (m_startPoint.X < e.Location.X ? m_startPoint.X : e.Location.X);
                float t_right = (m_startPoint.X >= e.Location.X ? m_startPoint.X : e.Location.X);
                float t_top = (m_startPoint.Y < e.Location.Y ? m_startPoint.Y : e.Location.Y);
                float t_bottom = (m_startPoint.Y >= e.Location.Y ? m_startPoint.Y : e.Location.Y);

                _mShape.Rectangle = RectangleF.FromLTRB(t_left, t_top, t_right, t_bottom);
                _mShape.Invalidate();

                _mDrawShape = false;
                _mShape = null;

            }

    */


        }

        public static void CompleteClear()
        {
            m_shape = null;
            m_shapeType = null;
            m_startDraw = false;
            if (m_graphControl != null)
            {
                m_graphControl.Locked = false;
                m_graphControl.MouseDown -= OnMouseDown;
                m_graphControl.MouseMove -= OnMouseMove;
                m_graphControl.MouseUp -= OnMouseUp; 
            }

        }

    }
}
