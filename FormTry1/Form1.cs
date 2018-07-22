using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Notes:
//設定SizableToolWindow,需要3個設定
//this.BorderStyle = FormBorderStyle.SizableToolWindow;
//this.ControlBox = false;
//this.Text = String.Empty; //caption設定為空
//1. a Rectangle(square) 'moveZone is created that encloses an area of the bottom-right of the Form of size 'MoveZoneSize.
//2. a threshold for "dampening" mouse move events 'ReSizeThreshold is defined. this may make the resizing of the Form visually smoother.
//3. mouse down is detected and if the mouse down location is in the 'moveZone bounds, the mouse move handler is wired up to the Form.
//4. in the mouse move event, the delta of mouse movement is calculated, and if it's smaller than 'ReSizeThreshold the method exits. if the delta is larger, then the Form is resized, and the current bounds of the form and the bounds of the 'moveZone rectangle are re-calculated.
//Other comments:
//1. depending on computer, graphics card, memory, etc., and the possible complexity of Form elements and their use of Anchor and Dock, Padding, Margin, etc. Properties: the visual quality the Form resize by mouse at run-time ... may vary. Of course, you should set the Form's 'DoubleBuffered Property to 'true.
//其他可用參考
// https://stackoverflow.com/questions/19206376/override-resize-behavior-of-winform-window
// youtube move and resize borderless form :https://www.youtube.com/watch?v=knW5lF3CRAY
namespace FormTry1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

 
        const int MoveZoneSize = 16;
        const int ReSizeThreshold = 4;

        private Rectangle scrnBounds;
        private Rectangle moveZone;

        // for use when making the Form movable
        private int mdx, mdy, dx, dy;

        private bool mouseIsUp = true;

        private void Form1_Load(object sender, EventArgs e)
        {
           
            this.TransparencyKey = BackColor; //問題:設定透明以後,視窗resize很不好用
        }

        private void UpdateScreenSize()
        {
            moveZone = new Rectangle(00, 00, MoveZoneSize, MoveZoneSize);
            
            scrnBounds = this.DisplayRectangle; //debug 說 = ClientArea
            
            moveZone.Offset(scrnBounds.Right - MoveZoneSize, scrnBounds.Bottom - MoveZoneSize);
            
            Graphics g = this.CreateGraphics();
            Pen selPen = new Pen(Color.Blue);
            g.FillRectangle(new SolidBrush(Color.Red), moveZone);
            
            g.Dispose();
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (ModifierKeys == Keys.Alt && e.KeyCode == Keys.Escape)
            if (  e.KeyCode == Keys.Escape)
            {
                this.MouseMove -= Form1_MouseMove;
                this.BackColor = Color.White;
                //this.Close();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
             UpdateScreenSize();//draw is after event load
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (moveZone.Contains(e.Location))
            {
                mdx = e.X;
                mdy = e.Y;

                this.MouseMove += Form1_MouseMove;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            this.MouseMove -= Form1_MouseMove;
            UpdateScreenSize();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
            //不能放在form的事件mouseMove中
        {
            dx = Math.Abs(e.X - mdx);//mdx,mdy是上次mouse down的滑鼠座標點
            dy = Math.Abs(e.Y - mdy);

            //if (dx < ReSizeThreshold && dy < ReSizeThreshold) return;

            this.Width = e.X;
            this.Height = e.Y;
        }
    }
}
