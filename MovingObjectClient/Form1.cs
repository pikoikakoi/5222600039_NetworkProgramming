using System;
using System.Drawing;
using System.Windows.Forms;

namespace MovingObjectClient
{
    public partial class Form1 : Form
    {
        Pen red = new Pen(Color.Red);
        Rectangle rect = new Rectangle(20, 20, 30, 30);
        SolidBrush fillBlue = new SolidBrush(Color.Blue);
        static Form1 formInstance;

        public Form1()
        {
            InitializeComponent();
            formInstance = this;
        }

        public static void UpdateRectanglePosition(int x, int y)
        {
            if (formInstance != null)
            {
                formInstance.rect.X = x;
                formInstance.rect.Y = y;
                formInstance.Invalidate();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawRectangle(red, rect);
            g.FillRectangle(fillBlue, rect);
        }
    }
}
