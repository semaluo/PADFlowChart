using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Netron.GraphLib;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib.UI;

namespace PADFlowChart
{
    [Serializable]
    public abstract class AbstractFlowChartShape : Shape, IConnectable
    {
        #region private variables
        private Point m_startPoint = Point.Empty;

        public bool m_isAutoConnect = true;
        private int m_autoShiftX = 30;
        private int m_autoShiftY = 60;
        private bool m_isEditable = true;
        private ISite site;

        public AbstractFlowChartShape() : base()
        {
            Init();
        }


        #endregion

        #region public variables

        #region Auto Connect
        public bool IsAutoConnect
        {
            get { return m_isAutoConnect; }
            set { m_isAutoConnect = value; }
        }

        public int AutoShiftX
        {
            get { return m_autoShiftX; }
            set { m_autoShiftX = value; }
        }

        public int AutoShiftY
        {
            get { return m_autoShiftY; }
            set { m_autoShiftY = value; }
        }

        public Point MoveStartPoint
        {
            get { return m_startPoint; }
            set { m_startPoint = value; }
        }

        public Connector LeftConnector
        {
            get { return Connectors["Left"]; }
        }

        #endregion

        public bool IsEditable
        {
            get { return m_isEditable; }
            set { m_isEditable = value; }
        }


        #endregion



        public virtual Connection CreateConnection(Connector connector)
        {
            FlowChartConnection connection = new FlowChartConnection();
            return connection;
        }

        protected override void InitEntity()
        {
            base.InitEntity();

            this.OnMouseDown += AbstractFlowChartShape_OnMouseDown;
            this.OnMouseMove += AbstractFlowChartShape_OnMouseMove;
            this.OnMouseUp += AbstractFlowChartShape_OnMouseUp;
        }

        private void Init()
        {
            Text = String.Empty;
            Font = new Font("宋体", 10);
            Pen = new Pen(Color.FromArgb(167, 58, 95));
            ShapeColor = Color.FromArgb(255, 253, 205);
        }

        private void AbstractFlowChartShape_OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            m_startPoint = e.Location;

            if (e.Clicks == 2 && e.Button == MouseButtons.Left)
            {
                if (IsEditable)
                {
                    //edit the text for double click
                    TextEditor editor = TextEditor.GetEditor(this);
                    editor.Show();
                    return;
                }
            }

        }

        private void AbstractFlowChartShape_OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!(Site is GraphControl) ) return;
            if (Layer != Abstract.CurrentLayer) return;


            if (IsAutoConnect) //Auto connect and disconnect
            {
                if (LeftConnector == null) return;

                //left connector has not been connected
                if (((GraphControl)Site).DoTrack)
                {
                    //Multiple selection
                    //if ((Site as GraphControl).SelectedShapes.Count > 1)
                    //{
                    //    ConnectionCollection t_selectedShapes = (Site as GraphControl).SelectedShapes;
                    //    if (IsLinkedAsOneGroup(t_selectedShapes))
                    //    {
                    //        Connector t_connector = FindConnectableConnectionForGroup(t_firstConnector, t_lastConnector);
                    //        if (t_connector == null) return;


                    //    }

                    //    return;
                    //}

                        //Is moving this shape
                    if (LeftConnector.Connections.Count == 0)
                    {
                        //left connector not connected, check if need to connect automatically
                        if ((Site as GraphControl).SelectedShapes.Count > 1)
                        {// no auto connect for group selection
                            return;
                        }

                        ConnectionCollection t_connections = Abstract.ConnectionsOfLayer(Layer.Name);
                        Connection t_connection = FindConnectableConnection(t_connections);
                        if (t_connection != null)
                        {
                            AttachToConnection(t_connection);
                            m_startPoint = e.Location;
                            return;
                        }

                        ShapeCollection t_shapes = Abstract.ShapesOfLayer(Layer.Name);
                        Connector t_connector = FindConnectableConnector(t_shapes);
                        if (t_connector != null)
                        {
                            AttachToConnector(t_connector);
                            m_startPoint = e.Location;
                            return;
                        }

                    }
                    else  //left connector has been connected
                    {
                        //check if need to remove connection
                        if ( (Site as GraphControl).SelectedShapes.Count > 1 )
                        {
                            DetachConnectionsFromUnselectedShape();
                            return;
                        }
                        Connection t_vertical = GetLeftSideVerticalConnection();
                        if (t_vertical == null) return; //no vertical connection, freely move shape

                        if (IsLeavingFromConnection(t_vertical))
                        {
                            int t_dx = (int) (e.Location.X - m_startPoint.X);
                            int t_dy = (int) (e.Location.Y - m_startPoint.Y);
                            if ((int) Math.Abs(t_dx) < m_autoShiftX)
                            {
                                StickToVerticalConnection(t_vertical);
                            }
                            else
                            {
                                DetachLeftSideConnections();
                            }
                        }
                    }
                }
            }

            
        }

        private void DetachConnectionsFromUnselectedShape()
        {

            ConnectionCollection t_removeConnections = new ConnectionCollection();
            Connector t_leftConnector = null;

            foreach (Shape shape in (Site as GraphControl).SelectedShapes)
            {
                if (shape.Connectors["Left"] == null) continue;

                foreach (Connection connection in shape.Connectors["Left"].Connections)
                {
                    if (connection.From.Name == "Left" && connection.To.Name == "Left")
                    {
                        if ( !(connection.From.BelongsTo.IsSelected && connection.To.BelongsTo.IsSelected) )
                        {
                            if (!t_removeConnections.Contains(connection))
                            {
                                t_removeConnections.Add(connection);
                            }
                        }
                    }
                }
            }

            foreach (Connection connection in t_removeConnections)
            {
                connection.Remove();
            }
        }

        private bool IsLeavingFromConnection(Connection connection)
        {
            if (connection == null)
            {
                return false;
            }

            Connector t_nextConnector = (connection.From == LeftConnector) ? connection.To : connection.From;
            return ((int) t_nextConnector.Location.X != (int) LeftConnector.Location.X);
        }


        private void AbstractFlowChartShape_OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            m_startPoint = Point.Empty;
        }

        private Connection GetLeftSideVerticalConnection()
        {
            foreach (Connection connection in LeftConnector.Connections)
            {
                if (connection.IsSelected) continue;

                if (IsVerticalConnection(connection))
                {
                    return connection;
                }
            }

            return null;
        }

        private void DetachLeftSideConnections()
        {
            Connection t_inConnection = null;
            Connection t_outConnection = null;

            foreach (Connection connection in LeftConnector.Connections)
            {
                if (connection.To == LeftConnector)
                {
                    t_inConnection = connection;
                    continue;
                }

                if (connection.From == LeftConnector)
                {
                    t_outConnection = connection;
                }
            }

            float t_stickX = 0;
            if (t_inConnection != null && t_outConnection != null)
            {
                t_stickX = t_outConnection.To.Location.X;
                Connection t_conn = new FlowChartConnection(Site);
                t_conn.Insert(t_inConnection.From, t_outConnection.To);
            }

            if (t_inConnection != null)
            {
                if (IsVerticalConnection(t_inConnection))
                {
                    t_stickX = t_inConnection.From.Location.X;
                }
                t_inConnection.Remove();
            }

            if (t_outConnection != null)
            {
                if (IsVerticalConnection(t_outConnection))
                {
                    t_stickX = t_outConnection.To.Location.X;
                }
                t_outConnection.Remove();
            }

            t_stickX = t_stickX < Rectangle.X ? t_stickX + m_autoShiftX + 1 : t_stickX - m_autoShiftX - 1;
            HorizonMoveTo(t_stickX);
        }



        private void StickToVerticalConnection(Connection connection)
        {
            if (!IsVerticalConnection(connection)) return;

            if (connection.To == LeftConnector)
            {
                HorizonMoveTo(connection.From.Location.X);
            }
            else
            {
                HorizonMoveTo(connection.To.Location.X);
            }

        }

        private void AttachToConnector(Connector connector)
        {
            if (IsAbove(connector))
            {
                AttachFromAbove(connector);
            }
            else
            {
                AttachFromBelow(connector);
            }
        }

        private void AttachFromBelow(Connector connector)
        {
            Connection t_outConnection = null;
            foreach (Connection connection in connector.Connections)
            {
                if (connection.From == connector)
                {
                    t_outConnection = connection;
                    break;
                }
            }

            Connection t_conn1 = null;
            Connection t_conn2 = null;

            t_conn1 = new FlowChartConnection(Site);
            t_conn1.Insert(connector, LeftConnector);

            if (t_outConnection != null)
            {
                t_conn2 = new FlowChartConnection(Site);
                t_conn2.Insert(LeftConnector, t_outConnection.To);
                t_outConnection.Remove();
            }

            //Horizontally move shape of connector to connection
            HorizonMoveTo(connector.Location.X);
        }

        private void AttachFromAbove(Connector connector)
        {
            Connection t_inConnection = null;

            foreach (Connection connection in connector.Connections)
            {
                if (connection.To == connector)
                {
                    t_inConnection = connection;
                    break;
                }
            }

            Connection t_conn1 = null; //the new connection attached by connector
            Connection t_conn2 = null;
            if (t_inConnection != null)
            {
                t_conn1 = new FlowChartConnection(Site);
                t_conn1.Insert(t_inConnection.From, LeftConnector);
                t_inConnection.Remove();
            }

            t_conn2 = new FlowChartConnection(Site);
            t_conn2.Insert(LeftConnector, connector);

            //Horizontally move shape of connector to connection
            HorizonMoveTo(connector.Location.X);

        }

        private void HorizonMoveTo(float x)
        {
            Rectangle = new RectangleF(x, Rectangle.Y,
                Rectangle.Width, Rectangle.Height);
        }

        private bool IsAbove(Connector connector)
        {
            return LeftConnector.Location.Y < connector.Location.Y;
        }

        private Connector FindConnectableConnector(ShapeCollection shapes)
        {
            RectangleF t_area = GetConnectableArea(LeftConnector);
            int t_minDistance = m_autoShiftY + 999;
            Connector t_connector = null;

            foreach (Shape shape in shapes)
            {
                if (shape == this) continue;
                if (shape.Connectors["Left"] == null) continue;

                //when check the Y distance, only check the distance between edges of two shapes.
                RectangleF t_leftEdge = new RectangleF(shape.X, shape.Y, 0, shape.Height);
                if (!t_area.IntersectsWith(t_leftEdge)) continue;

                int t_dy = (int)(Math.Abs(LeftConnector.Location.Y - shape.Connectors["Left"].Location.Y)
                           - (LeftConnector.BelongsTo.Height + shape.Height) / 2);
                if (t_dy < t_minDistance)
                {
                    t_minDistance = t_dy;
                    t_connector = shape.Connectors["Left"];
                }
            }

            return t_connector;
        }

        private RectangleF GetConnectableArea(Connector connector)
        {
            RectangleF t_area = new System.Drawing.RectangleF(0, 0, 0, 0);
            float t_left = connector.Location.X - m_autoShiftX;
            float t_right = connector.Location.X + m_autoShiftX;
            float t_top = connector.Location.Y - (connector.BelongsTo.Rectangle.Height / 2) - m_autoShiftY;
            float t_bottom = connector.Location.Y + (connector.BelongsTo.Rectangle.Height / 2) + m_autoShiftY;

            t_area = RectangleF.FromLTRB(t_left, t_top, t_right, t_bottom);
            return t_area;
        }

        private Connection AttachToConnection(Connection connection)
        {
            if (connection == null) return null;

            Connector t_from = connection.From;
            Connector t_to = connection.To;
            Connection t_conn1 = null; //the new connection attached by connector
            Connection t_conn2 = null;

            if (LeftConnector.Location.Y <= t_to.Location.Y)
            {
                t_conn1 = new FlowChartConnection(Site);
                t_conn1.Insert(t_from, LeftConnector);

                t_conn2 = new FlowChartConnection(Site);
                t_conn2.Insert(LeftConnector, t_to);

                connection.Remove();
            }

            if (t_to.Location.Y < LeftConnector.Location.Y)
            {
                t_conn1 = new FlowChartConnection(LeftConnector.Site);
                t_conn1.Insert(t_to, LeftConnector);
            }

            //Horizontally move shape of connector to connection
            HorizonMoveTo(t_to.Location.X);
            return t_conn1;
        }

        private Connection FindConnectableConnection(ConnectionCollection connections)
        {
            foreach (Connection t_conn in connections)
            {
                if (IsVerticalConnection(t_conn))
                {
                    Rectangle t_rect = GetConnectableArea(t_conn);
                    if (t_rect.Contains(Point.Round(LeftConnector.Location)))
                    {
                        return t_conn;
                    }
                }
            }

            return null;
        }

        private Rectangle GetConnectableArea(Connection connection)
        {
            Rectangle t_area = new Rectangle(0, 0, 0, 0);
            if (connection == null) return t_area;
            if (connection.From == null || connection.To == null) return t_area;

            int t_left = (int)connection.From.Location.X - m_autoShiftX;
            int t_right = (int)connection.From.Location.X + m_autoShiftX;
            int t_top = (int)connection.From.Location.Y;
            int t_bottom = (int)connection.To.Location.Y;

            t_area = System.Drawing.Rectangle.FromLTRB(t_left, t_top, t_right, t_bottom);
            return t_area;
        }

        private bool IsVerticalConnection(Connection connection)
        {
            if (connection == null) return false;

            if (connection.From == null || connection.To == null) return false;

            return connection.From.Name == "Left" && connection.To.Name == "Left";
        }

        private bool IsBelow(Connector connectorBelow, Connector connectorAbove)
        {
            return connectorAbove.Location.Y < connectorBelow.Location.Y;
        }

        #region serialization
        protected AbstractFlowChartShape(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.m_isEditable = info.GetBoolean("m_isEditable");
        }

        public AbstractFlowChartShape(IGraphSite site):base(site)
        {
            this.Site = site;
            Init();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("m_isEditable", m_isEditable);
        }

        public override void PostDeserialization()
        {
            base.PostDeserialization();
            Pen = new Pen(Color.FromArgb(167, 58, 95));

        }
        #endregion
    }
}