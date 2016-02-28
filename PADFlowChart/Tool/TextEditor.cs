using Netron.GraphLib;
using Netron.GraphLib.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PADFlowChart
{
    public class TextEditor : TextBox
    {

//        private static TextEditorControl editor = null;
        private Shape currentShape;
        private static TextEditor editor;
        private static readonly object synObject = new object();

        //        private EventHandler<MouseEventArgs> onescape = new EventHandler<MouseEventArgs>(Controller_OnMouseDown);
        public Shape Shape
        {
            get
            {
                return currentShape;
            }
            set
            {
                currentShape = value;
            }
                
        }

        private TextEditor()
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Multiline = true;
            this.ScrollBars = ScrollBars.None;
            this.WordWrap = true;
            this.BackColor = Color.White;
            this.LostFocus += TextEditor_LostFocus;
        }

        private void TextEditor_LostFocus(object sender, EventArgs e)
        {
            Hide();
        }

        public static TextEditor GetEditor(Shape shape)
        {
            if (shape == null)
                throw new Exception("Cannot assign an editor to a 'null' shape.");

            if (editor == null)
            {
                lock (synObject)
                {
                    editor = new TextEditor();
                }
            }

            editor.Shape = shape;
            editor.Location = Point.Round(shape.Site.ZoomPoint(editor.Shape.Rectangle.Location) + new SizeF(shape.Site.AutoScrollPosition.X, shape.Site.AutoScrollPosition.Y));
            editor.Width = (int)editor.Shape.Rectangle.Width;
            editor.Height = (int)editor.Shape.Rectangle.Height;
            Point t_size = new Point(editor.Width, editor.Height);
            editor.Size = (Size)shape.Site.ZoomPoint(t_size);
            editor.Font = new Font("Tahoma", 8.5F); 
            editor.BackColor = shape.ShapeColor;
            editor.Visible = false;
            return editor;
        }


        public new void Show()
        {
            if (currentShape == null)
                return;
            //Selection.Clear();
            //diagramControl.View.ResetTracker();
            //diagramControl.Controller.SuspendAllTools();
            //diagramControl.Controller.Enabled = false;
            //diagramControl.Controller.OnMouseDown += onescape;
            Control control = currentShape.Site as Control;
            if (control == null)
            {
                return;
            }
            control.Controls.Add(this);

            Visible = true;
            Text = currentShape.Text;
            SelectionLength = Text.Length;
            ScrollToCaret();
            Focus();
            //base.Show();
        }

        //static void Controller_OnMouseDown(object sender, MouseEventArgs e)
        //{
        //    Hide();
        //    diagramControl.Controller.OnMouseDown -= onescape;
        //}
        public new void Hide()
        {
            if (currentShape == null)
                return;
            //diagramControl.Controller.Enabled = true;
            //diagramControl.Focus();
            Control control = currentShape.Site as Control;
            if (control == null)
            {
                return;
            }
            control.Controls.Remove(this);

            Visible = false;
            currentShape.Text = Text;
            //diagramControl.Controller.UnsuspendAllTools();
            currentShape = null;
            //base.Hide();
        }


    }
}